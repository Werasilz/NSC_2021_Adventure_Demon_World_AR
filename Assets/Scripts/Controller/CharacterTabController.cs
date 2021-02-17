using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterTabController : MonoBehaviour
{
    private float adminCheck = 0;
    private int i = 0;
    private GameObject canvasAdmin;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI criticalText;

    public Button shard1;
    public Button shard2;
    public Button shard3;
    public Button shard4;
    public Button shard5;

    private int minDamage = 50;
    private int maxDamage = 85;

    private void Awake()
    {
        canvasAdmin = GameObject.Find("CanvasAdmin");
        canvasAdmin.SetActive(false);
    }

    void Update()
    {
        Admin();

        levelText.text = "Level | เลเวล : " + ExperieneManager.instance.levelPlayer;
        scoreText.text = "Score | คะแนน : " + GameManager.instance.score;
        hpText.text = "HP | พลังชีวิต : " + GameManager.instance.maxHpPlayer;
        damageText.text = "STR | พลังโจมตี : " + (minDamage * ExperieneManager.instance.increaseDamagePercent).ToString("0") + " - " + (maxDamage * ExperieneManager.instance.increaseDamagePercent).ToString("0");
        defenceText.text = "DEF | พลังป้องกัน : " + (GameManager.instance.defence * 10).ToString() + " %";
        criticalText.text = "CRI | อัตราคริติคอล : " + (100 / GameManager.instance.critical).ToString("0") + " %";

        if (LevelSelectManager.instance != null)
        {
            if (LevelSelectManager.instance.level1IsClear)
            {
                shard1.interactable = true;
            }

            if (LevelSelectManager.instance.level2IsClear)
            {
                shard2.interactable = true;
            }

            if (LevelSelectManager.instance.level3IsClear)
            {
                shard3.interactable = true;
            }

            if (LevelSelectManager.instance.level4IsClear)
            {
                shard4.interactable = true;
            }

            if (LevelSelectManager.instance.level5IsClear)
            {
                shard5.interactable = true;
            }
        }
    }

    public void Shard1()
    {
        if (GameObject.Find("CharacterTab").transform.GetChild(1).gameObject.activeInHierarchy == false)
        {
            GameObject.Find("CharacterTab").transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("CharacterTab").transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void Shard2()
    {
        if (GameObject.Find("CharacterTab").transform.GetChild(2).gameObject.activeInHierarchy == false)
        {
            GameObject.Find("CharacterTab").transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("CharacterTab").transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void Shard3()
    {
        if (GameObject.Find("CharacterTab").transform.GetChild(3).gameObject.activeInHierarchy == false)
        {
            GameObject.Find("CharacterTab").transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("CharacterTab").transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    public void Shard4()
    {
        if (GameObject.Find("CharacterTab").transform.GetChild(4).gameObject.activeInHierarchy == false)
        {
            GameObject.Find("CharacterTab").transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("CharacterTab").transform.GetChild(4).gameObject.SetActive(false);
        }
    }

    public void Shard5()
    {
        if (GameObject.Find("CharacterTab").transform.GetChild(5).gameObject.activeInHierarchy == false)
        {
            GameObject.Find("CharacterTab").transform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("CharacterTab").transform.GetChild(5).gameObject.SetActive(false);
        }
    }

    void Admin()
    {
        if (adminCheck >= 10)
        {
            adminCheck = 0;

            if (!canvasAdmin.activeInHierarchy)
            {
                canvasAdmin.SetActive(true);
            }
            else
            {
                canvasAdmin.SetActive(false);
            }
        }
    }

    public void ShowAdminTab()
    {
        Debug.Log(adminCheck);
        adminCheck += 1;
    }

    public void GetScore()
    {
        GameManager.instance.score += 1;
    }

    public void LevelClear()
    {
        i++;

        if (i == 1)
        {
            LevelSelectManager.instance.level1IsClear = true;
        }

        if (i == 2)
        {
            LevelSelectManager.instance.level2IsClear = true;
        }

        if (i == 3)
        {
            LevelSelectManager.instance.level3IsClear = true;
        }

        if (i == 4)
        {
            LevelSelectManager.instance.level4IsClear = true;
        }

        if (i == 5)
        {
            LevelSelectManager.instance.level5IsClear = true;
        }

        if (i == 6)
        {
            LevelSelectManager.instance.level6IsClear = true;
        }

        if (i == 7)
        {
            LevelSelectManager.instance.level7IsClear = true;
        }

        if (i == 8)
        {
            LevelSelectManager.instance.level8IsClear = true;
        }

        if (i == 9)
        {
            LevelSelectManager.instance.level9IsClear = true;
        }

        if (i == 10)
        {
            LevelSelectManager.instance.level10IsClear = true;
        }
    }
}
