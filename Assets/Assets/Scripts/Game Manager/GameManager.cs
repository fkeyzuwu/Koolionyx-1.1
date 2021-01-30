using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Vector2 spawnPoint = new Vector2(-7.5f, 1.5f);
    public GameObject player;
    public GameObject home;

    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("singleton kaki");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore()
    {
        scoreText.text = "Eucalyptus left: " + Eucalyptus.count;
    }

    public void RestartLevel()
    {
        Eucalyptus.count = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UpdateScore();
    }

    public void ReturnedHome()
    {
        if(Eucalyptus.count == 0)
        {
            GameWon();
        }
    }

    public void GameWon()
    {
        Debug.Log("You won the game!");
        //change to a game win scene
    }
}
