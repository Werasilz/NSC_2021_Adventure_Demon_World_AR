using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public static LevelSelectManager instance = null;

    public bool statsSet1;
    public bool statsSet2;
    public bool statsSet3;
    public bool statsSet4;
    public bool statsSet5;

    public bool level1IsClear;
    public bool level2IsClear;
    public bool level3IsClear;
    public bool level4IsClear;
    public bool level5IsClear;

    public bool level6IsClear;
    public bool level7IsClear;
    public bool level8IsClear;
    public bool level9IsClear;
    public bool level10IsClear;

    private Button buttonLv1;
    private Button buttonLv2;
    private Button buttonLv3;
    private Button buttonLv4;
    private Button buttonLv5;

    private Button buttonLv6;
    private Button buttonLv7;
    private Button buttonLv8;
    private Button buttonLv9;
    private Button buttonLv10;

    public bool titleClear;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        #endregion
    }

    void GetCompoent()
    {
        if (SceneManager.GetActiveScene().name == "LevelSelect")
        {
            buttonLv1 = GameObject.Find("LevelButton1").GetComponent<Button>();
            buttonLv2 = GameObject.Find("LevelButton2").GetComponent<Button>();
            buttonLv3 = GameObject.Find("LevelButton3").GetComponent<Button>();
            buttonLv4 = GameObject.Find("LevelButton4").GetComponent<Button>();
            buttonLv5 = GameObject.Find("LevelButton5").GetComponent<Button>();

            buttonLv6 = GameObject.Find("BossButton1").GetComponent<Button>();
            buttonLv7 = GameObject.Find("BossButton2").GetComponent<Button>();
            buttonLv8 = GameObject.Find("BossButton3").GetComponent<Button>();
            buttonLv9 = GameObject.Find("BossButton4").GetComponent<Button>();
            buttonLv10 = GameObject.Find("BossButton5").GetComponent<Button>();
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!titleClear)
            {
                titleClear = true;
                GameObject.Find("CanvasTitle").SetActive(false);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            titleClear = false;
        }

        if (SceneManager.GetActiveScene().name == "LevelSelect")
        {
            GetCompoent();

            if (!level1IsClear)
            {
                buttonLv6.interactable = false;
            }
            else
            {
                buttonLv6.interactable = true;
            }

            if (!level2IsClear)
            {
                buttonLv7.interactable = false;
            }
            else
            {
                buttonLv7.interactable = true;
            }

            if (!level3IsClear)
            {
                buttonLv8.interactable = false;
            }
            else
            {
                buttonLv8.interactable = true;
            }

            if (!level4IsClear)
            {
                buttonLv9.interactable = false;
            }
            else
            {
                buttonLv9.interactable = true;
            }

            if (!level5IsClear)
            {
                buttonLv10.interactable = false;
            }
            else
            {
                buttonLv10.interactable = true;
            }

            if (!level6IsClear)
            {
                buttonLv2.interactable = false;
            }
            else
            {
                buttonLv2.interactable = true;
            }

            if (!level7IsClear)
            {
                buttonLv3.interactable = false;
            }
            else
            {
                buttonLv3.interactable = true;
            }

            if (!level8IsClear)
            {
                buttonLv4.interactable = false;
            }
            else
            {
                buttonLv4.interactable = true;
            }

            if (!level9IsClear)
            {
                buttonLv5.interactable = false;
            }
            else
            {
                buttonLv5.interactable = true;
            }
        }
    }
}
