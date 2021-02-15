using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyIntoWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().isIntoArea = false;
        }
    }
}
