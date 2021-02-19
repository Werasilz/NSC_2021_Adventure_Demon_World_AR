using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyArea"))
        {
            other.gameObject.GetComponent<EnemyController>().enemyCollider.enabled = true;
        }

        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().stackHit = 1;
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyArea"))
        {
            other.GetComponentInParent<EnemyController>().isIntoArea = true;
        }
    }
}
