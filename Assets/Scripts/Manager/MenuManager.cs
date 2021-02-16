using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public AudioClip clickSound;

    public void TabToStart()
    {
        Destroy(GameObject.Find("CanvasTitle"));
    }

    public void HideCharacterCanvas()
    {
        SoundManager.instance.PlaySingle(clickSound);
        GameObject.Find("CharacterTab").transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ShowCharacterCanvas()
    {
        SoundManager.instance.PlaySingle(clickSound);
        GameObject.Find("CharacterTab").transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ShowLicense()
    {
        SoundManager.instance.PlaySingle(clickSound);
        GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(true);
    }

    public void OkButton()
    {
        SoundManager.instance.PlaySingle(clickSound);
        GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(false);
    }


    public void LevelSelect()
    {
        SoundManager.instance.PlaySingle(clickSound);
        SceneManager.LoadScene("LevelSelect");
    }

    public void NextLevel()
    {
        SoundManager.instance.PlaySingle(clickSound);

        #region Level to Boss
        if (GameManager.instance.level == 1)
        {
            SceneManager.LoadScene("Level1Boss");
        }

        if (GameManager.instance.level == 2)
        {
            SceneManager.LoadScene("Level2Boss");
        }

        if (GameManager.instance.level == 3)
        {
            SceneManager.LoadScene("Level3Boss");
        }

        if (GameManager.instance.level == 4)
        {
            SceneManager.LoadScene("Level4Boss");
        }

        if (GameManager.instance.level == 5)
        {
            SceneManager.LoadScene("Level5Boss");
        }
        #endregion

        #region Boss to Level
        if (GameManager.instance.level == 6)
        {
            SceneManager.LoadScene("Level2");
        }

        if (GameManager.instance.level == 7)
        {
            SceneManager.LoadScene("Level3");
        }

        if (GameManager.instance.level == 8)
        {
            SceneManager.LoadScene("Level4");
        }

        if (GameManager.instance.level == 9)
        {
            SceneManager.LoadScene("Level5");
        }

        if (GameManager.instance.level == 10)
        {
            // End Game go to Main Menu
            // Not have Next Button
        }
        #endregion
    }

    public void MainMenu()
    {
        SoundManager.instance.StopMusic();
        SoundManager.instance.PlaySingle(clickSound);
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.isPause = false;
    }

    public void Back()
    {
        SoundManager.instance.PlaySingle(clickSound);
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowComic()
    {
        SoundManager.instance.PlaySingle(clickSound);
        GameObject.Find("Canvas").transform.GetChild(6).gameObject.SetActive(true);
        GameManager.instance.isReadComic = false;
    }

    public void RestartGame()
    {
        SoundManager.instance.PlaySingle(clickSound);

        #region Restat normal Level
        if (GameManager.instance.level == 1)
        {
            SceneManager.LoadScene("Level1");
        }

        if (GameManager.instance.level == 2)
        {
            SceneManager.LoadScene("Level2");
        }

        if (GameManager.instance.level == 3)
        {
            SceneManager.LoadScene("Level3");
        }

        if (GameManager.instance.level == 4)
        {
            SceneManager.LoadScene("Level4");
        }

        if (GameManager.instance.level == 5)
        {
            SceneManager.LoadScene("Level5");
        }
        #endregion

        #region Restart Boss Level
        if (GameManager.instance.level == 6)
        {
            SceneManager.LoadScene("Level1Boss");
        }
        if (GameManager.instance.level == 7)
        {
            SceneManager.LoadScene("Level2Boss");
        }
        if (GameManager.instance.level == 8)
        {
            SceneManager.LoadScene("Level3Boss");
        }
        if (GameManager.instance.level == 9)
        {
            SceneManager.LoadScene("Level4Boss");
        }
        if (GameManager.instance.level == 10)
        {
            SceneManager.LoadScene("Level5Boss");
        }
        #endregion
    }

    public void PauseButton()
    {
        SoundManager.instance.PlaySingle(clickSound);

        if (!GameManager.instance.isPause)
        {
            GameManager.instance.isPause = true;
        }
        else if (GameManager.instance.isPause)
        {
            GameManager.instance.isPause = false;
        }
    }

    public void SoundButton()
    {
        SoundManager.instance.PlaySingle(clickSound);

        if (SoundManager.instance.musicSource.volume > 0)
        {
            SoundManager.instance.musicSource.volume = 0;
            SoundManager.instance.efxSource.volume = 0;
        }
        else if (SoundManager.instance.musicSource.volume == 0)
        {
            SoundManager.instance.musicSource.volume = 0.5f;
            SoundManager.instance.efxSource.volume = 1;
        }
    }

    #region Level Select Button
    public void GotoLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GotoLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void GotoLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void GotoLevel4()
    {
        SceneManager.LoadScene("Level4");
    }

    public void GotoLevel5()
    {
        SceneManager.LoadScene("Level5");
    }

    public void GotoLevel6()
    {
        SceneManager.LoadScene("Level6");
    }

    public void GotoLevel7()
    {
        SceneManager.LoadScene("Level7");
    }

    public void GotoLevel8()
    {
        SceneManager.LoadScene("Level8");
    }

    public void GotoLevel9()
    {
        SceneManager.LoadScene("Level9");
    }

    public void GotoLevel10()
    {
        SceneManager.LoadScene("Level10");
    }
    #endregion

    #region Admin Button
    public void GetShard()
    {
        if (GameManager.instance.level == 1 || GameManager.instance.level == 2 || GameManager.instance.level == 3 || GameManager.instance.level == 4 || GameManager.instance.level == 5)
        {
            GameManager.instance.shardCollect += 1;
            GameManager.instance.switchCollect += 1;
        }
        else if (GameManager.instance.level == 6 || GameManager.instance.level == 7 || GameManager.instance.level == 8 || GameManager.instance.level == 9 || GameManager.instance.level == 10)
        {
            GameManager.instance.ShowCompleteTab();
        }
    }

    public void GetHP()
    {
        GameManager.instance.hpPlayer = GameManager.instance.maxHpPlayer;
    }

    public void GetEXP()
    {
        ExperieneManager.instance.expPlayer += 1000;
    }

    public void OneShotKill()
    {
        ExperieneManager.instance.increaseDamagePercent = 100;
        GameManager.instance.AdminSetDemonEnemy();
    }
    #endregion
}
