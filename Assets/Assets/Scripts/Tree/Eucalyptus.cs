using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eucalyptus : MonoBehaviour
{
    [SerializeField]
    public static int count = 0;
    public bool isEaten = false;
    
    void Awake()
    {
        count++;
    }

    public void OnEucalyptusEaten()
    {
        if (!isEaten)
        {
            count--;
            isEaten = true;
            GameManager.instance.UpdateScore();

            if (count == 0)
            {
                Debug.Log("You ate all the eucalyptus! Return home to win the game!");
            }
        }
    }
}
