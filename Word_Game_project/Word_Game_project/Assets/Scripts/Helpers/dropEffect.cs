using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropEffect : MonoBehaviour
{
    public GameObject dropPar;

    public void dropped()
    {
        dropPar.SetActive(true);
    }
}
