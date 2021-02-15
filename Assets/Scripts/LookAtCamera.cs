using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private GameObject arCamera;

    private void Awake()
    {
        arCamera = GameObject.Find("AR Camera");
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(45, arCamera.transform.eulerAngles.y, 0);
    }
}
