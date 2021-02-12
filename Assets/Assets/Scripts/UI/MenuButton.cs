using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerDownHandler
{
    public AudioClip clip;
    public AudioManager audioManager;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = audioManager.GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audioSource.PlayOneShot(clip, 0.3f);
    }
}
