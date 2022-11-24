using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cones : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 direction = other.transform.position - transform.position;
            other.GetComponent<iDamagable>().takeDamage(direction);
            other.GetComponent<thirdPersonMovement>().playfallaudio();
        }
    }
}
