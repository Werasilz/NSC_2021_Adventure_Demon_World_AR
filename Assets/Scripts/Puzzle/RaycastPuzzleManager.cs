using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPuzzleManager : MonoBehaviour
{
    private Camera arCamera;
    RaycastHit hitObject;

    private void Awake()
    {
        arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    void Update()
    {
        Debug.DrawRay(arCamera.transform.position, transform.forward * 1000, Color.red);

        if (Physics.Raycast(arCamera.transform.position, transform.forward, out hitObject, 1000))
        {
            if (hitObject.collider.tag == "Puzzle")                                                                 // Check Tag Puzzle
            {
                if (hitObject.collider.GetComponent<PuzzleController>().isPlayerTrigger)
                {
                    if (hitObject.collider.GetComponent<PuzzleController>().Button)                                 // Puzzle is Button
                    {
                        if (!hitObject.collider.GetComponent<PuzzleController>().isTab)                             // isTab is false can loading
                        {
                            hitObject.collider.GetComponent<PuzzleController>().countDown += 1 * Time.deltaTime;    // Send 1 every 1 second
                            hitObject.collider.GetComponent<PuzzleController>().timeleft = 0.1f;                    // Send 1 every 1 second
                        }
                    }

                    if (hitObject.collider.GetComponent<PuzzleController>().Switch)                                 // Puzzle is Button
                    {
                        if (!hitObject.collider.GetComponent<PuzzleController>().isTab)                             // isTab is false can loading
                        {
                            hitObject.collider.GetComponent<PuzzleController>().countDown += 1 * Time.deltaTime;    // Send 1 every 1 second
                            hitObject.collider.GetComponent<PuzzleController>().timeleft = 0.1f;                    // Send 1 every 1 second
                        }
                    }
                }
            }
        }
    }
}
