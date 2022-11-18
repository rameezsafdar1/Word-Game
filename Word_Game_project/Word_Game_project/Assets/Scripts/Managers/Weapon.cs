using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool player;
    public AIMover ai;
    public AudioSource hitAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (player)
        {
            if (other.tag == "AI")
            {
                Vector3 direction = other.transform.position - transform.position;
                other.GetComponent<iDamagable>().takeDamage(direction);
                gameObject.SetActive(false);
                Vibration.Vibrate(200);

                if (hitAudio != null)
                {
                    hitAudio.Play();
                }
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                Vector3 direction = new Vector3(0, 0, transform.position.z * -1);
                other.GetComponent<iDamagable>().takeDamage(direction);
                gameObject.SetActive(false);
                Vibration.Vibrate(200);
            }
        }
    }
}
