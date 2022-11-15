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
    private bool hitDone;

    private void Update()
    {
        if (playerHandler != null)
        {
            if (fov.detectedObjects.Count > 0 && !playerHandler.hasAlphabet && !Player.isStunned && !hitDone)
            {
                anim.SetTrigger("Attack");
                Vibration.Vibrate(200);
                StartCoroutine(waitHitComplete());
                StartCoroutine(wait());
            }
        }

        if (aiMover != null)
        {
            if (fov.detectedObjects.Count > 0 && !aiMover.hasAlphabet && !aiMover.isStunned && !hitDone)
            {
                anim.SetTrigger("Attack");
                StartCoroutine(waitHitComplete());
                StartCoroutine(wait());
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

    private IEnumerator waitHitComplete()
    {
        hitDone = true;
        yield return new WaitForSeconds(0.5f);
        hitDone = false;        
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.2f);
        Vibration.Vibrate(200);
    }

}
