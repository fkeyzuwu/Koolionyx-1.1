using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator crossfade;
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

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        StartCoroutine(audioManager.FadeIn("Foley", 1f));
        crossfade.SetTrigger("FadeIn");
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
            StartCoroutine(audioManager.FadeOut("Foley", 1f));
            crossfade.SetTrigger("FadeOut");

            yield return new WaitForSeconds(1f);

            StartCoroutine(audioManager.FadeIn("Main Theme", 1f));
            StartCoroutine(audioManager.FadeIn("Foley", 1f));
            crossfade.SetTrigger("FadeIn");
            SceneManager.LoadScene(levelIndex);

            //ResetAnimationTriggers();
        }

        if(methodName == "RestartLevel") //When you reload the current scene
        {
            //ResetAnimationTriggers();

            audioManager.Stop("Main Theme");

            SceneManager.LoadScene(levelIndex);
            StartCoroutine(audioManager.FadeIn("Main Theme", 1f));
            StartCoroutine(audioManager.FadeIn("Foley", 1f));
            crossfade.SetTrigger("FadeIn");
        }
    }

    private void ResetAnimationTriggers()
    {
        crossfade.ResetTrigger("Start");
        crossfade.ResetTrigger("End");
        crossfade.SetTrigger("Reset");
    }
}
