using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CallOnStart : MonoBehaviour
{
    public static CallOnStart instance;

    void Awake()
    {
        instance = this;

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SoundManager.instance.StopMusic();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SoundManager.instance.PlayerMusic();

            if (GameManager.instance.isReadComic)
            {
                GameObject.Find("Comic").gameObject.SetActive(false);
            }
        }

        GameManager.instance.SetLevel();
    }

    public void LoadGameSetup()
    {
        if (PlaceMapScript.instance.isSetPosition)
        {
            PlayerController.instance.GetComponent();
            GameManager.instance.GetComponent();
            ExperieneManager.instance.GetComponent();
            SoundManager.instance.PlayerMusic();
        }
    }
}
