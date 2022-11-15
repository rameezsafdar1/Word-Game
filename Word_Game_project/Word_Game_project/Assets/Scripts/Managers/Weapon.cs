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
                Vector3 direction = other.transform.position - transform.position;
                other.GetComponent<iDamagable>().takeDamage(direction);
                gameObject.SetActive(false);
                Vibration.Vibrate(200);
                StartCoroutine(wait());
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
                StartCoroutine(wait());
            }
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.2f);
        Vibration.Vibrate(200);
    }
}
