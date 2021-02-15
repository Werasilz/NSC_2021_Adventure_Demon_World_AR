using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color spriteColor;
    private float disappearTime;
    private float waitTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        disappearTime = 1f;                                                             // Time To Disappear
        waitTime = 1.5f;
        spriteColor = spriteRenderer.color;                                             // Save Color
    }

    void Update()
    {
        disappearTime -= Time.deltaTime;                                                // Decrease DisappearTime
        waitTime -= Time.deltaTime;

        if (disappearTime > 0)
        {
            transform.localScale += new Vector3(0.001f, 0.001f, 0.001f) * Time.deltaTime;
        }

        if (waitTime < 0)
        {
            float fadeSpeed = 3f;
            spriteColor.a -= fadeSpeed * Time.deltaTime;
            spriteRenderer.color = spriteColor;
        }

        if (spriteColor.a < 0)                                                          // Destroy When Alpha < 0
        {
            Destroy(gameObject);
        }
    }
}
