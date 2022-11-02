using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatController : MonoBehaviour
{
    public fieldOfView fov;
    public Animator anim;
    public pickHandler playerHandler;

    private void Update()
    {
        if (playerHandler != null)
        {
            if (fov.detectedObjects.Count > 0 && !playerHandler.hasAlphabet)
            {
                anim.SetTrigger("Attack");
            }
        }
    }


}
