using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour
{
    private Vector3 position;
    private Quaternion rotation;

    private void Start()
    {
        transform.parent = null;
        position = transform.position;
        rotation = transform.rotation;
    }

    public void resetMe()
    {
        transform.parent = null;
        transform.position = position;
        transform.rotation = rotation;
        transform.tag = "Weapon";
    }

}
