using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    [Range(0f, 1f)]
    public float masterVolume = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            if (s == null || s.clip == null)
            {
                Debug.LogWarning("Skipping null sound or missing clip.");
                continue;
            }

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * masterVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    void Start()
    {
        Play("MainTheme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound != null && sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound != null && sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        s.source.Stop();
    }

    public void SetVolume(float volume)
    {
        masterVolume = volume;

        foreach (Sound s in sounds)
        {
            if (s == null || s.source == null)
            {
                Debug.LogWarning("A sound is missing or has no AudioSource assigned!");
                continue;
            }

            float finalVolume = s.volume * Mathf.Max(0.05f, volume);
            s.source.volume = finalVolume;
            Debug.Log($"Setting volume for {s.name}: base={s.volume}, slider={volume}, final={finalVolume}");
        }
    }
}
