using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStack : MonoBehaviour
{
    private GameObject puzzleSystem;
    public bool[] puzzleCheck;

    private void Awake()
    {
        puzzleSystem = GameObject.Find("PuzzleSystem");
    }

    private void OnDestroy()
    {
        if (puzzleCheck[0])
        {
            puzzleSystem.GetComponent<PuzzleManager>().puzzleStack1 += 1;
        }

        if (puzzleCheck[1])
        {
            puzzleSystem.GetComponent<PuzzleManager>().puzzleStack2 += 1;
        }

        if (puzzleCheck[2])
        {
            puzzleSystem.GetComponent<PuzzleManager>().puzzleStack3 += 1;
        }
    }
}