using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource musicSource;
    public AudioClip songClip;

    public float AudioStartDelay;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlaySoundWithDelay()
    {
        Invoke(nameof(PlaySound), AudioStartDelay);
        Debug.Log($"Delay {AudioStartDelay} and play");
    }

    void PlaySound()
    {
        musicSource.clip = songClip;
        musicSource.loop = false;
        musicSource.Play();
    }
}
