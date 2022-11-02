using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatController : MonoBehaviour
{
    public fieldOfView fov;
    public Animator anim;
    public pickHandler playerHandler;
    public GameObject baseWeapon;

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

    public void makeAttackable()
    {
        baseWeapon.SetActive(true);
    }

    public void makeNeutral()
    {
        baseWeapon.SetActive(false);
    }
}
