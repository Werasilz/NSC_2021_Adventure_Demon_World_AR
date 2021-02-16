using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    #region Player Status
    public int level = 0;                                                               // Level Game Play
    public int score = 0;                                                               // Score by Kill Enemy
    public float hpPlayer;                                                              // Player Health
    public float maxHpPlayer = 100;                                                     // Max HP
    public int defence = 0;
    public int critical = 10;
    #endregion

    #region Text User Interface
    private TextMeshProUGUI hpText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI questText1;
    private TextMeshProUGUI questText2;
    #endregion

    #region Game Value
    public int dropRate;                                                                // Drop Item Rate Default = 5
    [HideInInspector] public bool isPause;                                              // Check Game Pause
    [HideInInspector] public int damageDouble = 1;                                      // For Boss Double Damage
    #endregion

    #region Quest Value
    public int shardCollect;
    public int shardAll;
    public int switchCollect;
    public int switchAll;
    private bool questFinish1;
    private bool questFinish2;
    #endregion

    #region GameObject
    [HideInInspector] public GameObject canvas;
    private GameObject completeTab;
    private GameObject pauseTab;
    private GameObject gameoverTab;
    private GameObject gamePlay;
    private GameObject questBar1;
    private GameObject questBar2;
    private GameObject clearText;
    #endregion

    #region GameObject for Rotate to Camera
    private GameObject arCamera;
    private GameObject player;
    #endregion

    [HideInInspector] public bool isReadComic;

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

    public void GetComponent()
    {
        arCamera = GameObject.Find("AR Camera");
        player = GameObject.Find("Player");
        hpText = GameObject.Find("HpText").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        gamePlay = GameObject.Find("GamePlay");
        gamePlay.transform.GetChild(1).gameObject.SetActive(true);                      // Enemy Group
        completeTab = GameObject.Find("CompleteTab");
        pauseTab = GameObject.Find("PauseTab");
        gameoverTab = GameObject.Find("GameoverTab");
        completeTab.SetActive(false);                                                   // Set Complete Disable on Start
        gameoverTab.SetActive(false);                                                   // Set Game Over Disabel on Start

        if (level == 1 || level == 2 || level == 3 || level == 4 || level == 5)         // Get Quest Text only normal level
        {
            questBar1 = GameObject.Find("UserInterface").transform.GetChild(2).gameObject;
            questBar2 = GameObject.Find("UserInterface").transform.GetChild(3).gameObject;
            clearText = GameObject.Find("UserInterface").transform.GetChild(4).gameObject;
            questText1 = GameObject.Find("QuestText").GetComponent<TextMeshProUGUI>();
            questText2 = GameObject.Find("QuestText2").GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        #region Set Variable to Text UI
        if (level != 0)
        {
            if (PlaceMapScript.instance.isSetPosition)                                  // Set Text when press Set Position Button
            {
                hpText.text = hpPlayer.ToString();
                scoreText.text = score.ToString();

                if (level == 1 || level == 2 || level == 3 || level == 4 || level == 5) // Set Text Quest only normal level
                {
                    questText1.text = "เศษเสี้ยว : " + shardCollect + " / " + shardAll + " ชิ้น";
                    questText2.text = "ปลดล็อค : " + switchCollect + " / " + switchAll + " จุด";
                }
            }
        }

        #endregion

        LookAtCamera();
        CheckFinish();
        PotionHelper();
        LimitHpPlayer();
        GodMode();
        ShowPauseTab();
        ShowGameOverTab();
    }

    void LookAtCamera()
    {
        if (level != 0)
        {
            if (PlaceMapScript.instance.isSetPosition)                                  // Player Look at Camera
            {

                player.transform.rotation = Quaternion.Euler(0, arCamera.transform.eulerAngles.y, 0);
            }
        }
    }

    void CheckFinish()
    {
        if (level == 1 || level == 2 || level == 3 || level == 4 || level == 5)
        {
            if (shardCollect > 0 && shardCollect >= shardAll)                           // Check Get All Shard
            {
                shardCollect = 0;                                                       // Clear Shard
                questBar1.SetActive(false);                                             // Hide Quest Bar
                questFinish1 = true;                                                    // CheckFinish
            }

            if (switchCollect > 0 && switchCollect >= switchAll)                        // Check Get All Switch
            {
                switchCollect = 0;                                                      // Clear Shard
                questBar2.SetActive(false);                                             // Hide Quest Bar
                questFinish2 = true;                                                    // Check Finish
            }

            if (questFinish1 && questFinish2)                                           // Check Finish All Quest
            {
                questFinish1 = false;                                                   // Reset Value
                questFinish2 = false;                                                   // Reset Value
                clearText.SetActive(true);                                              // Show Clear Text
                Invoke("ShowCompleteTab", 5);                                           // Call Level Clear
            }

        }
    }

    #region Hp Player Section
    void PotionHelper()                                                                 // Helper for Player when Low HP
    {
        if (hpPlayer <= 50)                                                             // Change Drop Rate to 3 when HP < 50
        {
            dropRate = 2;
        }
        else if (hpPlayer <= 30)                                                        // Change Drop Rate to 2 when HP < 30
        {
            dropRate = 1;
        }
        else                                                                            // Reset Drop Rate
        {
            dropRate = 3;
        }
    }

    void LimitHpPlayer()
    {
        if (hpPlayer > maxHpPlayer)                                                     // Set HpPlayer Limit By Max HP Player
        {
            hpPlayer = maxHpPlayer;
        }
    }
    #endregion

    #region Game Menu Tab
    public void ShowCompleteTab()
    {
        completeTab.SetActive(true);
        gamePlay.transform.GetChild(1).gameObject.SetActive(false);

        if (level == 1)
        {
            LevelSelectManager.instance.level1IsClear = true;
        }

        if (level == 2)
        {
            LevelSelectManager.instance.level2IsClear = true;
        }

        if (level == 3)
        {
            LevelSelectManager.instance.level3IsClear = true;
        }

        if (level == 4)
        {
            LevelSelectManager.instance.level4IsClear = true;
        }

        if (level == 5)
        {
            LevelSelectManager.instance.level5IsClear = true;
        }

        if (level == 6)
        {
            LevelSelectManager.instance.level6IsClear = true;
        }

        if (level == 7)
        {
            LevelSelectManager.instance.level7IsClear = true;
        }

        if (level == 8)
        {
            LevelSelectManager.instance.level8IsClear = true;
        }

        if (level == 9)
        {
            LevelSelectManager.instance.level9IsClear = true;
        }

        if (level == 10)
        {
            LevelSelectManager.instance.level10IsClear = true;
        }
    }

    void ShowGameOverTab()
    {
        if (hpPlayer <= 0)
        {
            gameoverTab.SetActive(true);
            gamePlay.transform.GetChild(1).gameObject.SetActive(false);
            player.SetActive(false);
        }
    }

    void ShowPauseTab()
    {
        if (level != 0)
        {
            if (PlaceMapScript.instance.isSetPosition)
            {
                if (isPause)
                {
                    if (!pauseTab.activeInHierarchy)
                    {
                        pauseTab.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
                else if (!isPause)
                {
                    pauseTab.SetActive(false);
                    Time.timeScale = 1;
                }
            }

            if (!PlaceMapScript.instance.isSetPosition)
            {
                if (!isPause)
                {
                    Time.timeScale = 1;
                }
            }
        }
    }
    #endregion

    public void SetLevel()                                                              // Check Scene Level and Set to variable
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            level = 1;
            shardAll = 4;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            level = 2;
            shardAll = 4;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            level = 3;
            shardAll = 6;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            level = 4;
            shardAll = 5;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            level = 5;
            shardAll = 5;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            level = 6;
            shardAll = 10;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            level = 7;
            shardAll = 10;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            level = 8;
            shardAll = 10;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 9)
        {
            level = 9;
            shardAll = 10;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 10)
        {
            level = 10;
            shardAll = 10;
            shardCollect = 0;
            switchAll = 3;
            switchCollect = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 11)
        {
            level = 0;
        }

        hpPlayer = maxHpPlayer;
    }

    void GodMode()
    {
        if (Input.GetKeyDown(KeyCode.F1))                                               // Full HP
        {
            hpPlayer = maxHpPlayer;
        }

        if (Input.GetKeyDown(KeyCode.F2))                                               // Level Up
        {
            ExperieneManager.instance.expPlayer += 5000;
        }

        if (Input.GetKeyDown(KeyCode.F3) && gamePlay.transform.GetChild(1).gameObject.activeInHierarchy)        // Hide Enemy
        {
            gamePlay.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.F3) && !gamePlay.transform.GetChild(1).gameObject.activeInHierarchy)  // Show Enemy
        {
            gamePlay.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void AdminSetDemonEnemy()
    {
        for (int i = 0; i < gamePlay.transform.GetChild(1).gameObject.transform.childCount; i++)                // Find Enemy and SetActive all in EnemyGroup
        {
            gamePlay.transform.GetChild(1).gameObject.transform.GetChild(i).GetComponent<EnemyController>().demonType = 1;
        }
    }
}
