using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphabet : MonoBehaviour
{
    public string letter;
    public bool picked;
    private float tempTime;
    private Vector3 startPos;
    private Quaternion startrot;
    [HideInInspector]
    public bool placed;
    public AIMover ai;
    public Transform Holder;

    private void Start()
    {
        startPos = transform.position;
        startrot = transform.rotation;
    }

    private void Update()
    {

        if (picked) 
        {
            placed = false;
            tempTime = 0;
        }

        if (!picked && !placed && tag == "Pickable")
        {
            tempTime += Time.deltaTime;

            if (tempTime >= 15)
            {
                if (transform.position != startPos)
                {
                    transform.position = startPos;
                    transform.rotation = startrot;
                }
                tempTime = 0;
            }
        }
    }

    public void forceReset()
    {
        transform.position = startPos;
        transform.rotation = startrot;
    }

}
