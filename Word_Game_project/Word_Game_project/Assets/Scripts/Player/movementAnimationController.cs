using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementAnimationController : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    private thirdPersonMovement main;

    private void Start()
    {
        main = GetComponent<thirdPersonMovement>();
    }

    private void Update()
    {
        anim.SetFloat("Velocity", main.direction.magnitude);
    }

}
