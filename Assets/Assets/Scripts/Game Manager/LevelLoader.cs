using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
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
        Sound foley = AudioManager.instance.GetSound("Foley");
        StartCoroutine(AudioManager.instance.FadeIn(foley.source, 1f, foley.volume));
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
            Sound foley = AudioManager.instance.GetSound("Foley");
            StartCoroutine(AudioManager.instance.FadeOut(foley.source, 1f));

            yield return new WaitForSeconds(1f);

            transition.SetTrigger("Start");
            SceneManager.LoadScene(levelIndex);
        }
        if(methodName == "RestartLevel")//When you reload the current scene
        {
            transition.SetTrigger("End");
            SceneManager.LoadScene(levelIndex);
            Sound foley = AudioManager.instance.GetSound("Foley");
            StartCoroutine(AudioManager.instance.FadeIn(foley.source, 1f, foley.volume));
        }
    }
}
