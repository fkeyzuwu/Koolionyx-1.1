using UnityEngine;

public class SnakeController : MonoBehaviour
{
    #region Private Properties

    private Animator snakeAnimator;
    private GameObject kooli;
    private LayerMask defaultLayer;
    private bool isAlert = false;
    private bool isAttacking = false;

    #endregion

    #region Public Properties

    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public float health = 70f;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        kooli = GameObject.Find("Kooli");
        snakeAnimator = GetComponent<Animator>();
        defaultLayer = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlert)
        {
            if (kooli.transform.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 180f);
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, 0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Kooli")
        {
            if (!isAlert)
            {
                snakeAnimator.SetBool("inAlertRange", true);
                isAlert = true;
            }
            else if (!isAttacking)
            {
                snakeAnimator.SetBool("inAttackRange", true);
                isAttacking = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Kooli")
        {
            if (isAttacking)
            {
                snakeAnimator.SetBool("inAttackRange", false);
                isAttacking = false;
            }
            else if (isAlert)
            {
                snakeAnimator.SetBool("inAlertRange", false);
                isAlert = false;
            }
        }
    }
}
