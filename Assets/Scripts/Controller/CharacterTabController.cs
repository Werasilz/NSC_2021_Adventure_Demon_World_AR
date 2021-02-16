using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterTabController : MonoBehaviour
{
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

    void Update()
    {
        levelText.text = "เลเวล : " + ExperieneManager.instance.levelPlayer;
        scoreText.text = "คะแนน : " + GameManager.instance.score;
        hpText.text = "พลังชีวิต : " + GameManager.instance.maxHpPlayer;
        damageText.text = "พลังโจมตี : " + (minDamage * ExperieneManager.instance.increaseDamagePercent).ToString("0") + " - " + (maxDamage * ExperieneManager.instance.increaseDamagePercent).ToString("0");
        defenceText.text = "พลังป้องกัน : " + (GameManager.instance.defence * 10).ToString() + " %";
        criticalText.text = "อัตราคริติคอล : " + (100 / GameManager.instance.critical).ToString("0") + " %";

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
}
