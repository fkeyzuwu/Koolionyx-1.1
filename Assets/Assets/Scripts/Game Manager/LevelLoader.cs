using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    private AudioManager audioManager;
    public static LevelLoader instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }

    public void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        StartCoroutine(audioManager.FadeIn("Foley", 1f));
        transition.SetTrigger("Start");
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1, "LoadNextLevel"));
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex, "RestartLevel"));
    }

    IEnumerator LoadLevel(int levelIndex, string methodName)
    {
        if(methodName == "LoadNextLevel") //When you load a new scene
        {
            transition.SetTrigger("End");
            StartCoroutine(audioManager.FadeOut("Foley", 1f));

            yield return new WaitForSeconds(1f);

            transition.SetTrigger("Start");
            StartCoroutine(audioManager.FadeIn("Main Theme", 1f));
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(levelIndex);
        }

        if(methodName == "RestartLevel") //When you reload the current scene
        {
            audioManager.Stop("Main Theme");
            transition.SetTrigger("End");
            SceneManager.LoadScene(levelIndex);
            StartCoroutine(audioManager.FadeIn("Main Theme", 1f));
        }
    }
}
