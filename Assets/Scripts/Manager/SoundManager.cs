using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;
    public AudioClip music0;
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    private bool isPlayMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayerMusic()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && !isPlayMusic)
        {
            musicSource.clip = music0;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 && !isPlayMusic)
        {
            musicSource.clip = music1;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2 && !isPlayMusic)
        {
            musicSource.clip = music2;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3 && !isPlayMusic)
        {
            musicSource.clip = music1;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4 && !isPlayMusic)
        {
            musicSource.clip = music2;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5 && !isPlayMusic)
        {
            musicSource.clip = music1;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 6 && !isPlayMusic)
        {
            musicSource.clip = music3;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 7 && !isPlayMusic)
        {
            musicSource.clip = music3;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 8 && !isPlayMusic)
        {
            musicSource.clip = music3;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 9 && !isPlayMusic)
        {
            musicSource.clip = music3;
            musicSource.Play();
            isPlayMusic = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 10 && !isPlayMusic)
        {
            musicSource.clip = music3;
            musicSource.Play();
            isPlayMusic = true;
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
        isPlayMusic = false;
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }
}
