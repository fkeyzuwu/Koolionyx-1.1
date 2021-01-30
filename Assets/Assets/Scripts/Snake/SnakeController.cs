using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Animator snakeAnim;
    private PlayerController playerControllerScript;
    private GameObject kooli;
    private LayerMask defaultLayer;
    private bool isAlert = false;
    private bool isAttack = false;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Kooli").GetComponent<PlayerController>();
        kooli = GameObject.Find("Kooli");
        snakeAnim = GetComponent<Animator>();
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
                if (!isAttack)
                {
                    snakeAnim.SetBool("inAttackRange", true);
                    isAttack = true;
                }
            }
            else
            {
                if (!isAlert)
                {
                    snakeAnim.SetBool("inAlertRange", true);
                    isAlert = true;
                }

                if (isAttack)
                {
                    snakeAnim.SetBool("inAttackRange", false);
                    isAttack = false;
                }
            }
        }
        else if(isAlert)
        {
            snakeAnim.SetBool("inAlertRange", false);
            isAlert = false;
        }
    }

    
}
