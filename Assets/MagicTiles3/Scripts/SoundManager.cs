using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource musicSource;
    public AudioClip songClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayTheSong()
    {
        musicSource.clip = songClip;
        musicSource.loop = false;
        musicSource.Play();
    }
}
