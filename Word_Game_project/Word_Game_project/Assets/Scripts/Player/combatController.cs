using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatController : MonoBehaviour
{
    public fieldOfView fov;
    public Animator anim;

    private void Update()
    {
        if (fov.detectedObjects.Count > 0)
        {
            anim.SetTrigger("Attack");
        }
    }


}
