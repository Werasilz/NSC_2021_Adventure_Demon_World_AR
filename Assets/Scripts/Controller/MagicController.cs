using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    private float disappearTime;

    void Start()
    {
        disappearTime = 2f;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    void Update()
    {
        disappearTime -= Time.deltaTime;
        transform.localScale += new Vector3(0.3f, 0.3f, 0.3f) * Time.deltaTime;
        transform.Rotate(0, 5, 0);

        if (disappearTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().canShoot = true;
    }
}
