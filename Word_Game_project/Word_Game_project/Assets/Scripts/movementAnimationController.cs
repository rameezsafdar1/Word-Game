using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementAnimationController : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;   
    

    private void Update()
    {
        anim.SetFloat("Velocity", controller.velocity.magnitude);
    }

}
