using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private LayerMask solidObjectsLayer;
    private LayerMask eucalyptusLayer;
    private LayerMask defaultLayer;
    private LayerMask enemiesLayer;
    private Vector2 input;
    private bool isMoving;
    private bool isHorizontal;
    private int direction;
    private Animator animator;
    private AudioManager audioManager;
    private float maxEnergy = 100f;
    private float minEnergy = 0f;
    private float energy = 100f;
    private float attackCooldownSeconds = 0.5f;
    private float nextAttackTime;

    [HideInInspector]
    public float moveSpeed;
    public Image energyBar;
    public TextMeshProUGUI energyText;
    public int stepCounter = 0;
    public int loseEnergyStep = 5;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        solidObjectsLayer = LayerMask.GetMask("SolidObjects");
        eucalyptusLayer = LayerMask.GetMask("Eucalyptus");
        defaultLayer = LayerMask.GetMask("Default");
        enemiesLayer = LayerMask.GetMask("Enemies");
    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Remove Diagonal Movement
            if(input.x != 0)
            {
                input.y = 0;
                isHorizontal = true;

                if (input.x < 0)
                    direction = -1;
                else
                    direction = 1;
            }
            else if(input.y != 0)
            {
                isHorizontal = false;

                if (input.y < 0)
                    direction = -1;
                else
                    direction = 1;
            }

            if(input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if(IsWalkabale(targetPos))
                    StartCoroutine(Move(targetPos));
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                EatEucalyptus();
            }

            if (Input.GetKeyDown(KeyCode.Space) && (nextAttackTime <= Time.time))
            {
                nextAttackTime = Time.time + attackCooldownSeconds;
                AttackPython();
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

        stepCounter++;
        CountSteps();
    }

    public void CountSteps()
    {
        if(stepCounter == loseEnergyStep)
        {
            ChangeEnergy(-5f);
            
            stepCounter = 0;
        }
    }

    private bool IsWalkabale(Vector3 targetPos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPos, 0.2f);

        foreach(Collider2D collider in colliders)
        {
            if(!collider.isTrigger)
            {
                return false;
            }
        }

        return true;
    }

    private void EatEucalyptus()
    {
        Vector2 eatPosition;
        
        if(isHorizontal)
            eatPosition = new Vector2(transform.position.x + direction, transform.position.y);
        else
            eatPosition = new Vector2(transform.position.x, transform.position.y + direction);

        Collider2D eucalyptusCollider = Physics2D.OverlapCircle(eatPosition, 0.2f, eucalyptusLayer);

        if (eucalyptusCollider != null && !eucalyptusCollider.GetComponent<Eucalyptus>().isEaten)
        {
            ChangeEnergy(20f);

            audioManager.PlayOneShot("Eat");
            eucalyptusCollider.GetComponent<Animator>().SetTrigger("isEaten");
            eucalyptusCollider.GetComponent<Eucalyptus>().OnEucalyptusEaten();
        }
    }

    private void AttackPython()
    {
        Vector2 attackPos;

        if (isHorizontal)
            attackPos = new Vector2(transform.position.x + direction, transform.position.y);
        else
            attackPos = new Vector2(transform.position.x, transform.position.y + direction);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos, 0.2f, enemiesLayer);

        foreach(Collider2D collider in colliders)
        {
            if (!collider.isTrigger)
            {
                SnakeController snakeController = collider.gameObject.GetComponent<SnakeController>();
                if (!snakeController.isDead && energy > minEnergy)
                {
                    snakeController.health -= energy;

                    ChangeEnergy(-10f);

                    if (snakeController.health <= 0f)
                    {
                        snakeController.isDead = true;
                        audioManager.Play("Snake Death", true);
                        collider.gameObject.GetComponent<Animator>().SetTrigger("isDead");
                    }

                    if (!snakeController.isDead)
                    {
                        audioManager.Play("Snake Hit", true);
                    }
                }
            }
        }
    }

    private void ChangeEnergy(float amount)
    {
        energy += amount;

        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }

        if(energy < minEnergy)
        {
            energy = minEnergy;
        }

        energyBar.fillAmount = energy / maxEnergy;
        energyText.text = (int)energy + " / 100";

        GameManager.instance.OnEnergyChanged(energy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
     {
        if (collision.gameObject.name == "Home")
        {
            GameManager.instance.ReturnedHome();
        }
    }
} 
