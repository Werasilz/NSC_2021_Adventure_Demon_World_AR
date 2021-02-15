using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerDirection : MonoBehaviour
{
    public bool[] direction;                            // Direction Array 0 1 || Left Right 
    public bool playerOnLeft;
    public bool playerOnRight;

    private void Update()
    {
        // Boss
        if (transform.GetComponentInParent<EnemyController>().isBossType)
        {
            if (!transform.GetComponentInParent<EnemyController>().isBossDanger && !transform.GetComponentInParent<EnemyController>().isBossNormalAttack)
            {
                if (playerOnLeft && !playerOnRight)
                {
                    transform.GetComponentInParent<EnemyController>().isFlip = false;
                }

                if (playerOnRight && !playerOnLeft)
                {
                    transform.GetComponentInParent<EnemyController>().isFlip = true;
                }
            }
        }

        // Normal Enemy
        if (!transform.GetComponentInParent<EnemyController>().isBossType)
        {
            if (playerOnLeft && !playerOnRight)
            {
                transform.GetComponentInParent<EnemyController>().isFlip = false;
            }

            if (playerOnRight && !playerOnLeft)
            {
                transform.GetComponentInParent<EnemyController>().isFlip = true;
            }
        }
    }
}
