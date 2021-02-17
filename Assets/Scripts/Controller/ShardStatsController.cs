using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShardStatsController : MonoBehaviour
{
    public GameObject useButton1;
    public GameObject useButton2;
    public GameObject useButton3;
    public GameObject useButton4;
    public GameObject useButton5;

    public GameObject star;
    public Button shard1;
    public Button shard2;
    public Button shard3;
    public Button shard4;
    public Button shard5;

    private void Update()
    {
        if (LevelSelectManager.instance.statsSet1)
        {
            star.transform.position = shard1.transform.position;
            useButton1.SetActive(false);
        }

        if (LevelSelectManager.instance.statsSet2)
        {
            star.transform.position = shard2.transform.position;
            useButton2.SetActive(false);
        }

        if (LevelSelectManager.instance.statsSet3)
        {
            star.transform.position = shard3.transform.position;
            useButton3.SetActive(false);
        }

        if (LevelSelectManager.instance.statsSet4)
        {
            star.transform.position = shard4.transform.position;
            useButton4.SetActive(false);
        }

        if (LevelSelectManager.instance.statsSet5)
        {
            star.transform.position = shard5.transform.position;
            useButton5.SetActive(false);
        }
    }

    private void CheckEquipStats()
    {
        if (LevelSelectManager.instance.statsSet1)
        {
            RemoveStats1();
        }

        if (LevelSelectManager.instance.statsSet2)
        {
            RemoveStats2();
        }

        if (LevelSelectManager.instance.statsSet3)
        {
            RemoveStats3();
        }

        if (LevelSelectManager.instance.statsSet4)
        {
            RemoveStats4();
        }

        if (LevelSelectManager.instance.statsSet5)
        {
            RemoveStats5();
        }
    }

    public void SetStats1()
    {
        CheckEquipStats();
        LevelSelectManager.instance.statsSet1 = true;

        if (LevelSelectManager.instance.statsSet1)
        {
            GameManager.instance.maxHpPlayer += 25;                         // HP
            GameManager.instance.defence += 1;                              // DEF
            ExperieneManager.instance.increaseDamagePercent += 0.2f;        // STR
        }
    }

    public void SetStats2()
    {
        CheckEquipStats();
        LevelSelectManager.instance.statsSet2 = true;

        if (LevelSelectManager.instance.statsSet2)
        {
            GameManager.instance.critical -= 3;                             // CRI
            GameManager.instance.defence += 1;                              // DEF
            ExperieneManager.instance.increaseDamagePercent += 0.1f;        // STR
        }
    }

    public void SetStats3()
    {
        CheckEquipStats();
        LevelSelectManager.instance.statsSet3 = true;

        if (LevelSelectManager.instance.statsSet3)
        {
            GameManager.instance.maxHpPlayer += 20;                         // HP
            GameManager.instance.critical -= 4;                             // CRI
            GameManager.instance.defence += 2;                              // DEF
        }
    }

    public void SetStats4()
    {
        CheckEquipStats();
        LevelSelectManager.instance.statsSet4 = true;

        if (LevelSelectManager.instance.statsSet4)
        {
            GameManager.instance.maxHpPlayer += 35;                         // HP
            GameManager.instance.critical -= 3;                             // CRI
            ExperieneManager.instance.increaseDamagePercent += 0.3f;        // STR
        }
    }

    public void SetStats5()
    {
        CheckEquipStats();
        LevelSelectManager.instance.statsSet5 = true;

        if (LevelSelectManager.instance.statsSet5)
        {
            GameManager.instance.maxHpPlayer += 50;                         // HP
            GameManager.instance.defence += 3;                              // DEF
            GameManager.instance.critical -= 5;                             // CRI
            ExperieneManager.instance.increaseDamagePercent += 0.5f;        // STR
        }
    }

    private void RemoveStats1()
    {
        useButton1.SetActive(true);
        LevelSelectManager.instance.statsSet1 = false;
        GameManager.instance.maxHpPlayer -= 25;                         // HP
        GameManager.instance.defence -= 1;                              // DEF
        ExperieneManager.instance.increaseDamagePercent -= 0.2f;        // STR
    }

    private void RemoveStats2()
    {
        useButton2.SetActive(true);
        LevelSelectManager.instance.statsSet2 = false;
        GameManager.instance.critical += 3;                             // CRI
        GameManager.instance.defence -= 1;                              // DEF
        ExperieneManager.instance.increaseDamagePercent -= 0.1f;        // STR
    }

    private void RemoveStats3()
    {
        useButton3.SetActive(true);
        LevelSelectManager.instance.statsSet3 = false;
        GameManager.instance.maxHpPlayer -= 20;                         // HP
        GameManager.instance.critical += 4;                             // CRI
        GameManager.instance.defence -= 2;                              // DEF
    }

    private void RemoveStats4()
    {
        useButton4.SetActive(true);
        LevelSelectManager.instance.statsSet4 = false;
        GameManager.instance.maxHpPlayer -= 35;                         // HP
        GameManager.instance.critical += 3;                             // CRI
        ExperieneManager.instance.increaseDamagePercent -= 0.3f;        // STR
    }

    private void RemoveStats5()
    {
        useButton5.SetActive(true);
        LevelSelectManager.instance.statsSet5 = false;
        GameManager.instance.maxHpPlayer -= 50;                         // HP
        GameManager.instance.defence -= 3;                              // DEF
        GameManager.instance.critical += 5;                             // CRI
        ExperieneManager.instance.increaseDamagePercent -= 0.5f;        // STR
    }
}
