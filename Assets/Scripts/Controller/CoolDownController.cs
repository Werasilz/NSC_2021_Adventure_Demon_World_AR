using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownController : MonoBehaviour
{
    public float coolDown;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        coolDown -= Time.deltaTime;

        if (coolDown <= 0)
        {
            button.interactable = true;
        }
    }
    public void StartCoolDown()
    {
        button.interactable = false;
        coolDown = 3;
    }
}
