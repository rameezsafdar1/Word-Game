using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatController : MonoBehaviour
{
    public fieldOfView fov;
    public Animator anim;
    public pickHandler playerHandler;
    public AIMover aiMover;
    public thirdPersonMovement Player;
    public GameObject baseWeapon;

    private void Update()
    {
        if (playerHandler != null)
        {
            if (fov.detectedObjects.Count > 0 && !playerHandler.hasAlphabet && !Player.isStunned)
            {
                anim.SetTrigger("Attack");
            }
        }

        if (aiMover != null)
        {
            if (fov.detectedObjects.Count > 0 && !aiMover.hasAlphabet && aiMover.isAggressive && !aiMover.isStunned)
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
