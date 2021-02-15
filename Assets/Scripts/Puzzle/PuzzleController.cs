using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PuzzleController : MonoBehaviour
{
    public bool Button;
    public bool Switch;
    public float countDown;
    public float timeleft;
    public bool isTab;
    public int puzzleNumber;
    private Animator anim;
    private int maxType;
    private int minType;
    public bool isPlayerTrigger;
    public GameObject[] enemy;

    private void Awake()
    {
        puzzleNumber = 1;
        anim = GetComponentInChildren<Animator>();
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);              // Hide Loading
    }

    private void Update()
    {
        if (countDown > 0)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);           // Show Loading when get countDown from raycast

            timeleft -= Time.deltaTime;                                             // When not ray cast time left = 0.1

            if (timeleft <= 0)                                                      // time left for hide Loading
            {
                countDown = 0;
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }

        if (countDown > 3)                                                          // Reset countDown to 0 when limit 3 second
        {
            countDown = 0;

            if (!isTab)
            {
                isTab = true;                                                       // Set to true for cannot raycast send countDown point

                if (puzzleNumber == 1)
                {
                    anim.SetBool("isTab", true);
                    puzzleNumber = 2;

                    if (Switch)
                    {
                        Destroy(gameObject);
                        // add quest
                    }
                    else
                    {
                        SpawnEnemy();
                    }
                }
                else if (puzzleNumber == 2)
                {
                    anim.SetBool("isTab", false);
                    puzzleNumber = 1;

                    if (Switch)
                    {
                        Destroy(gameObject);
                        // add quest 
                    }
                    else
                    {
                        SpawnEnemy();
                    }
                }

                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);      // Hide Loading When loading finish
                Invoke("ResetIsTab", 1);                                            // Wait 1 second and Call Reset isTab to false
            }
        }
    }

    void ResetIsTab()
    {
        isTab = false;
    }

    void SpawnEnemy()
    {
        if (GameManager.instance.level == 6 || GameManager.instance.level == 7)
        {
            minType = 0;
            maxType = 2;
        }
        else if (GameManager.instance.level == 8)
        {
            minType = 1;
            maxType = 3;
        }
        else if (GameManager.instance.level == 9 || GameManager.instance.level == 10)
        {
            minType = 0;
            maxType = 3;
        }

        GameObject enemyClone = Instantiate(enemy[Random.Range(minType, maxType)], transform.position, transform.rotation);
        enemyClone.GetComponent<EnemyController>().Awake();
        enemyClone.GetComponent<EnemyController>().Start();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerTrigger = false;
        }
    }
}
