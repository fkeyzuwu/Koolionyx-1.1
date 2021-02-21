using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] audioClipsArray;
    private Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in audioClipsArray)
        {
            sounds.Add(sound.name, sound);
        }
        audioClipsArray = null;
    }

    public void Play(string name)
    {
        AudioSource audioToPlay = FindAudioSource(name);

        if (audioToPlay == null)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            Sound sound = sounds[name];

            newSource.clip = sound.clip;
            newSource.volume = sound.volume;
            newSource.pitch = sound.pitch;
            newSource.loop = sound.loop;
            newSource.Play();
        }
        else
        {
            audioToPlay.Play();
        }
    }

    public void Play(string name, bool useRandomPitch)
    {
        if (useRandomPitch)
        {
            AudioSource audioToPlay = FindAudioSource(name);

            if (audioToPlay == null)
            {
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                Sound sound = sounds[name];

                newSource.clip = sound.clip;
                newSource.volume = sound.volume;
                newSource.pitch = Random.Range(0.9f, 1.1f);
                newSource.loop = sound.loop;
                newSource.Play();
            }
            else
            {
                if(audioToPlay.time > audioToPlay.clip.length / 2 || !audioToPlay.isPlaying)
                {
                    audioToPlay.pitch = Random.Range(0.9f, 1.1f);
                    audioToPlay.Play();
                }
            }
        }
        else
        {
            Play(name);
        }
    }

    public void PlayOneShot(string name)
    {
        AudioSource audioToPlay = FindAudioSource(name);

        if (audioToPlay == null)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            Sound sound = sounds[name];

            newSource.clip = sound.clip;
            newSource.volume = sound.volume;
            newSource.loop = sound.loop;

            newSource.pitch = Random.Range(0.9f, 1.1f);
            newSource.PlayOneShot(newSource.clip);
        }
        else
        {
            audioToPlay.pitch = Random.Range(0.9f, 1.1f);
            audioToPlay.PlayOneShot(audioToPlay.clip);
        }
    }

    public void Stop(string name)
    {
        AudioSource audioToStop = FindAudioSource(name);
        //audioToStop?.Stop();
        if(audioToStop != null)
        {
            audioToStop.Stop();
        }
    }

    private AudioSource FindAudioSource(string name)
    {
        return Array.Find(gameObject.GetComponents<AudioSource>(), source => source.clip.name == name);
    }

    public IEnumerator FadeIn(string name, float fadeTime)
    {
        Sound sound = sounds[name];
        AudioSource audioToFadeIn = FindAudioSource(name);

        if(FindAudioSource(name) == null)
        {
            audioToFadeIn = gameObject.AddComponent<AudioSource>();
        }

        float startVolume = 0.2f;

        audioToFadeIn.volume = 0f;
        audioToFadeIn.clip = sound.clip;
        audioToFadeIn.pitch = sound.pitch;
        audioToFadeIn.loop = sound.loop;
        audioToFadeIn.Play();

        while (audioToFadeIn.volume < sound.volume)
        {
            audioToFadeIn.volume += startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }
    }

    public IEnumerator FadeOut(string name, float fadeTime)
    {
        Sound sound = sounds[name];
        AudioSource audioToFadeOut = FindAudioSource(name);

        if (FindAudioSource(name) == null)
        {
            audioToFadeOut = gameObject.AddComponent<AudioSource>();
        }

        float startVolume = sound.volume;

        while (audioToFadeOut.volume > 0)
        {
            audioToFadeOut.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioToFadeOut.Stop();
        audioToFadeOut.volume = startVolume;
    }
}
