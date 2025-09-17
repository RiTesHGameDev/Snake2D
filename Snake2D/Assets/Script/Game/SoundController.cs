using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController SoundInstance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip foodPickupSound;
    [SerializeField] private AudioClip powerPickupSound;

    [Header("Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float backgroundMusicVolume;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume;

    private void Awake()
    {
        if (SoundInstance == null)
        {
            SoundInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (backgroundMusicSource == null)
        {
            backgroundMusicSource = gameObject.AddComponent<AudioSource>();
            backgroundMusicSource.loop = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        SetBackgroundMusicVolume(backgroundMusicVolume);
        SetSFXVolume(sfxVolume);
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && backgroundMusicSource != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.Play();
        }
    }
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (backgroundMusicSource != null)
            {
                backgroundMusicSource.volume = backgroundMusicVolume;
            }
            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume;
            }
        }
        else
        {
            SetBackgroundMusicVolume(backgroundMusicVolume);
            SetSFXVolume(sfxVolume);
        }
    }
    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }
    public void PlayFoodPickupSound()
    {
        PlaySFX(foodPickupSound);
    }

    public void PlayPowerPickupSound()
    {
        PlaySFX(powerPickupSound);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        backgroundMusicVolume = Mathf.Clamp01(volume);
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = backgroundMusicVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }
    }

    public void PauseBackgroundMusic()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Pause();
        }
    }

    public void ResumeBackgroundMusic()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.UnPause();
        }
    }
}
