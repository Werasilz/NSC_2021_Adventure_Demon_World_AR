using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private Color textColor;
    private TextMeshPro textmeshPro;
    private float disappearTime;
    public bool[] typeOfText;

    private void Awake()
    {
        textmeshPro = GetComponent<TextMeshPro>();

        // typeOfText 0 = Damage and Hp popup
        // typeOfText 1 = Skill popup
        // typeOfText 2 = Shard popup

        if (typeOfText[0])
        {
            disappearTime = 0.5f;                                                    // Time To Disappear
        }

        if (typeOfText[1])
        {
            disappearTime = 1.5f;                                                     // Time To Disappear
        }

        if (typeOfText[2])
        {
            disappearTime = 2f;                                                     // Time To Disappear
        }

        textColor = textmeshPro.color;                                              // Save Color To textColor
    }

    private void Update()
    {
        float moveY = 0.003f;
        transform.position += new Vector3(0, moveY) * Time.deltaTime;               // Text Move Up
        disappearTime -= Time.deltaTime;                                            // Decrease DisappearTime
        transform.localScale += new Vector3(0.002f, 0.002f, 0.002f) * Time.deltaTime;  // Increase Text Scale

        if (disappearTime < 0)                                                      // FadeAlpha when Disappear < 0
        {
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            textmeshPro.color = textColor;
        }

        if (textColor.a < 0)                                                        // Destroy When Alpha < 0
        {
            Destroy(gameObject);
        }
    }
}

