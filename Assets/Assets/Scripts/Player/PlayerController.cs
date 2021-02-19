using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private LayerMask solidObjectsLayer;
    private LayerMask eucalyptusLayer;
    private LayerMask defaultLayer;
    private LayerMask enemiesLayer;
    private bool isMoving;
    private Vector2 input;
    private bool isHorizontal;
    private int direction;
    private Animator animator;

    [HideInInspector]
    public float moveSpeed;
    public float energy = 100f; //TODO: UI

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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AttackPython(); //TODO: if python isnt dead, do a different animation - hit animation.
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
            energy += 20f;
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

        Collider2D attackCollider = Physics2D.OverlapCircle(attackPos, 0.2f, enemiesLayer);

        if (attackCollider != null)
        {
            SnakeController snakeController = attackCollider.gameObject.GetComponent<SnakeController>();
            if (!snakeController.isDead && energy > 0f)
            {
                snakeController.health -= energy;
                energy -= 10f;

                if (snakeController.health <= 0f)
                {
                    snakeController.isDead = true;
                    attackCollider.gameObject.GetComponent<Animator>().SetTrigger("isDead");
                }
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
     {
        if (collision.gameObject.name == "Home")
        {
            GameManager.instance.ReturnedHome();
        }
    }
} 
