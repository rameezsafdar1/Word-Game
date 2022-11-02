using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorLerp : MonoBehaviour
{
    public Material mat;
    public Color col;

    private void Update()
    {
        mat.color = Color.Lerp(Color.white, col, Mathf.PingPong(Time.time, 1));
    }


}
