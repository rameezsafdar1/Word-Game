using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool player;
    public AIMover ai;

    private void OnTriggerEnter(Collider other)
    {
        if (player)
        {
            if (other.tag == "AI")
            {
                other.GetComponent<iDamagable>().takeDamage();
                gameObject.SetActive(false);
                Vibration.Vibrate(6);
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                ai.calmMeDown();
                other.GetComponent<iDamagable>().takeDamage();
                gameObject.SetActive(false);
                Vibration.Vibrate(6);
            }
        }
    }
}
