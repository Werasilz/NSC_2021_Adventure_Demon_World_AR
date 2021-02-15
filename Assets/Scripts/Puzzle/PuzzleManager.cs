using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject blocker;
    public int puzzleStack1;
    public int puzzleStack2;
    public int puzzleStack3;

    private void Start()
    {
        puzzleStack1 = 0;
        puzzleStack2 = 0;
        puzzleStack3 = 0;

        if (GameManager.instance.level == 6 || GameManager.instance.level == 7 || GameManager.instance.level == 8 || GameManager.instance.level == 9 || GameManager.instance.level == 10)
        {
            button1.SetActive(false);
            button2.SetActive(false);
            button3.SetActive(false);
        }

    }

    void Update()
    {
        // puzzle number 1 is Button UP || 2 is Button Down
        #region Check Button Up Down and Puzzle Stack
        if (GameManager.instance.level == 6)
        {
            // Check Puzzle Number for destroy blocker
            if (button1.GetComponent<PuzzleController>().puzzleNumber == 2 &&
                button2.GetComponent<PuzzleController>().puzzleNumber == 2 &&
                button3.GetComponent<PuzzleController>().puzzleNumber == 2)
            {
                Destroy(blocker);
            }

            // Check Puzzle Stack for show button
            if (puzzleStack1 >= 3)
            {
                button1.SetActive(true);
            }

            if (puzzleStack2 >= 3)
            {
                button2.SetActive(true);
            }

            if (puzzleStack3 >= 3)
            {
                button3.SetActive(true);
            }
        }

        if (GameManager.instance.level == 7)
        {
            if (button1.GetComponent<PuzzleController>().puzzleNumber == 2 &&
                button2.GetComponent<PuzzleController>().puzzleNumber == 1 &&
                button3.GetComponent<PuzzleController>().puzzleNumber == 1)
            {
                Destroy(blocker);
            }

            // Check Puzzle Stack for show button
            if (puzzleStack1 >= 4)
            {
                button1.SetActive(true);
            }

            if (puzzleStack2 >= 4)
            {
                button2.SetActive(true);
            }

            if (puzzleStack3 >= 4)
            {
                button3.SetActive(true);
            }
        }

        if (GameManager.instance.level == 8)
        {
            // Check Puzzle Number for destroy blocker
            if (button1.GetComponent<PuzzleController>().puzzleNumber == 2 &&
                button2.GetComponent<PuzzleController>().puzzleNumber == 2 &&
                button3.GetComponent<PuzzleController>().puzzleNumber == 1)
            {
                Destroy(blocker);
            }


            // Check Puzzle Stack for show button
            if (puzzleStack1 >= 5)
            {
                button1.SetActive(true);
            }

            if (puzzleStack2 >= 5)
            {
                button2.SetActive(true);
            }

            if (puzzleStack3 >= 5)
            {
                button3.SetActive(true);
            }
        }

        if (GameManager.instance.level == 9)
        {
            // Check Puzzle Number for destroy blocker
            if (button1.GetComponent<PuzzleController>().puzzleNumber == 1 &&
                button2.GetComponent<PuzzleController>().puzzleNumber == 2 &&
                button3.GetComponent<PuzzleController>().puzzleNumber == 2)
            {
                Destroy(blocker);
            }

            // Check Puzzle Stack for show button
            if (puzzleStack1 >= 6)
            {
                button1.SetActive(true);
            }

            if (puzzleStack2 >= 6)
            {
                button2.SetActive(true);
            }

            if (puzzleStack3 >= 6)
            {
                button3.SetActive(true);
            }
        }

        if (GameManager.instance.level == 10)
        {
            // Check Puzzle Number for destroy blocker
            if (button1.GetComponent<PuzzleController>().puzzleNumber == 1 &&
                button2.GetComponent<PuzzleController>().puzzleNumber == 2 &&
                button3.GetComponent<PuzzleController>().puzzleNumber == 2)
            {
                Destroy(blocker);
            }

            // Check Puzzle Stack for show button
            if (puzzleStack1 >= 2)
            {
                button1.SetActive(true);
            }

            if (puzzleStack2 >= 2)
            {
                button2.SetActive(true);
            }

            if (puzzleStack3 >= 2)
            {
                button3.SetActive(true);
            }
        }
        #endregion
    }
}
