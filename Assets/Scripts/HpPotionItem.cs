using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpPotionItem : MonoBehaviour
{
    public AudioClip potionSound;
    public GameObject hpEffect;                                                                         // Hp Effect when Collcet Potion
    public GameObject healthPopup;                                                                      // Hp popup when Collect Potion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerArea"))
        {
            SoundManager.instance.PlaySingle(potionSound);
            GameObject effectClone = Instantiate(hpEffect, other.transform.position, transform.rotation);
            effectClone.transform.SetParent(other.gameObject.transform.parent);
            Destroy(effectClone, 1);
            int hpValue = Random.Range(10, 16);                                                         // Random Hp Value
            GameManager.instance.hpPlayer += hpValue;                                                   // Hp to Player Hp
            GameObject healthPopupClone = Instantiate(healthPopup, transform.position, transform.rotation);
            healthPopupClone.GetComponent<TextMeshPro>().SetText("+" + hpValue.ToString());
            Destroy(gameObject);
        }
    }
}
