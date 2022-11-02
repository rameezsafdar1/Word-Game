using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorLerp : MonoBehaviour
{
    public Material mat;
    public Color col, darkcol;

    private void Update()
    {
        mat.color = Color.Lerp(col, darkcol, Mathf.PingPong(Time.time, 1));
    }


}
