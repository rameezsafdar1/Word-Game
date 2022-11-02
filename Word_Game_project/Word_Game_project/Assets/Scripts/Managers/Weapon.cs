using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI")
        {
            other.GetComponent<iDamagable>().takeDamage();
            gameObject.SetActive(false);
            Vibration.Vibrate(6);
        }
        
    }
}
