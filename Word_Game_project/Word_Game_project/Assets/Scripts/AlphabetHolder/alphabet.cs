using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class alphabet : MonoBehaviour
{
    public int actor;
    public string letter;
    private bool picked;
    private float tempTime, tempTime2;
    private Vector3 startPos;
    private Quaternion startrot;
    [HideInInspector]
    public bool placed;
    public Transform Holder;
    public Color pickColor;
    public Collider col;

    public UnityEvent onPicked;
    public UnityEvent onDropped;
    public UnityEvent onFinalDrop;


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
            tempTime2 = 0;
        }

        if (placed)
        {
            tempTime2 += Time.deltaTime;

            if (tempTime2 >= 0.5f)
            {
                transform.rotation = Quaternion.Euler(-90, -180, 0);
            }

        }



        if (!picked && !placed && tag == "Pickable")
        {
            tempTime += Time.deltaTime;

            if (tempTime >= 15)
            {
                if (transform.position != startPos)
                {
                    GetComponent<curveFollower>().visual.transform.localPosition = Vector3.zero;
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

    public void finalPlacement()
    {
        col.enabled = false;
    }

    public void LetterPicked()
    {
        picked = true;
        if (onPicked != null)
        {
            onPicked.Invoke();
        }
    }

    public void LetterDropped()
    {
        picked = false;
        if (onDropped != null)
        {
            onDropped.Invoke();
        }
    }

    public void callFinalDrop()
    {
        if (onFinalDrop != null)
        {
            onFinalDrop.Invoke();
        }
    }

}
