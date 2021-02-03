using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        Sound foley = AudioManager.instance.GetSound("Foley");
        StartCoroutine(AudioManager.instance.FadeIn(foley.source, 1f, foley.volume));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    //TODO: Change it so you have different transition effects based on which scenes you activated it from.
    //Reloading the same scene shouldnt do the full animation.
    //Make animation entry state do nothing. after, make start trigger and end trigger. each one doing the other part of the animation.
    //also different fadeout audio when you die and respawn uknow
    {
        transition.SetTrigger("Start");

        Sound foley = AudioManager.instance.GetSound("Foley");
        StartCoroutine(AudioManager.instance.FadeOut(foley.source, 1f));

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(levelIndex);
        StartCoroutine(AudioManager.instance.FadeIn(foley.source, 1f, foley.volume));
    }
}
