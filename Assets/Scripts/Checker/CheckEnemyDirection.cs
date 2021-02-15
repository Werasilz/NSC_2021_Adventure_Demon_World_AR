using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyDirection : MonoBehaviour
{
    public static CheckEnemyDirection instance;
    public bool[] direction;                            // Direction Array 0 1 || Left Right 
    public bool enemyOnLeft;
    public bool enemyOnRight;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyArea") || other.CompareTag("Boss"))
        {
            enemyOnLeft = false;
            enemyOnRight = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (direction[0])
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
            {
                enemyOnLeft = true;
            }
        }

        if (direction[1])
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
            {
                enemyOnRight = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyArea") || other.CompareTag("Boss"))
        {
            enemyOnLeft = false;
            enemyOnRight = false;
        }
    }
}
