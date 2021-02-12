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
        Collider2D snakeAlertCollider = Physics2D.OverlapCircle(transform.position, 2f, defaultLayer);
        
        if (snakeAlertCollider != null)
        {
            if (kooli.transform.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f,180f);
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, 0f);
            }

            Collider2D snakeAttackCollider = Physics2D.OverlapCircle(transform.position, 1.0f, defaultLayer);

            if (snakeAttackCollider != null)
            {
                if (!isAttacking)
                {
                    snakeAnimator.SetBool("inAttackRange", true);
                    isAttacking = true;
                }
            }
            else
            {
                if (!isAlert)
                {
                    snakeAnimator.SetBool("inAlertRange", true);
                    isAlert = true;
                }

                if (isAttacking)
                {
                    snakeAnimator.SetBool("inAttackRange", false);
                    isAttacking = false;
                }
            }
        }
        else if(isAlert)
        {
            snakeAnimator.SetBool("inAlertRange", false);
            isAlert = false;
        }
    }
}
