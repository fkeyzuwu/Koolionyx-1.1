using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public void Start()
    {
        AudioManager.instance.Play("Foley");
        Sound foley = AudioManager.instance.GetSound("Foley");
        StartCoroutine(AudioManager.instance.FadeIn(foley.source, 1f, 1f));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(levelIndex);   
    }
}
