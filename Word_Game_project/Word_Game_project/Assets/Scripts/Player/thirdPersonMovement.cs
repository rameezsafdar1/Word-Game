using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdPersonMovement : MonoBehaviour, iDamagable
{
    public GameManager manager;
    public CharacterController controller;
    public float speed;
    [SerializeField] private float rotationSpeed;
    private float gravity = -1f;
    [HideInInspector] public Vector3 direction;
    public bool isStunned;
    public pickHandler pick;
    [SerializeField] private float sweepTime;
    private Vector3 sweepDir;
    private visualGraph vg;
    [HideInInspector] public AudioSource auds;
    public AudioSource hitaudio, hitaudio2;

    private void Start()
    {
        vg = GetComponent<visualGraph>();
        auds = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!manager.gameCompleted)
        {
            if (!isStunned)
            {
                movement();
            }
            else
            {
                if (sweepTime < 1)
                {
                    makeGravity();
                    sweepTime += Time.deltaTime;
                    controller.Move(sweepDir.normalized * 5 * Time.deltaTime);
                    Vector3 dir = new Vector3(0, gravity, 0);
                    controller.Move(dir * speed * Time.deltaTime);
                }
            }
        }
        else
        {
            auds.Stop();
            direction = Vector3.zero;
            makeGravity();
        }
    }

    private void movement()
    {
        float horizontal = ControlFreak2.CF2Input.GetAxisRaw("Horizontal");
        float vertical = ControlFreak2.CF2Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            if (!auds.isPlaying)
            {
                auds.Play();
            }
        }
        else
        {
            if (auds.isPlaying)
            {
                auds.Pause();
            }
        }

        controller.Move(direction * speed * Time.deltaTime);
        makeGravity();
        rotation();

    }

    private void rotation()
    {
        if (direction.x != 0 || direction.z != 0)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion newRot = Quaternion.Euler(0, angle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, rotationSpeed * Time.deltaTime);
        }
    }

    private void makeGravity()
    {
        if (controller.isGrounded)
        {
            gravity = -1;
        }
        else
        {
            gravity -= speed * Time.deltaTime;
        }
        Vector3 newdir = new Vector3(0, gravity, 0);
        newdir = transform.TransformDirection(newdir);
        controller.Move(newdir * speed * Time.deltaTime);
    }

    public void takeDamage(Vector3 direction)
    {
        auds.Stop();
        vg.enabled = true;
        vg.startMoving();
        sweepTime = 0;
        sweepDir = transform.TransformDirection(direction);
        gameObject.layer = 8;
        isStunned = true;
        pick.animHandler.anim.SetBool("Picked", false);
        pick.dropAlphabet();
        pick.tempTime = 0;
        StartCoroutine(stunMe());        
        pick.fillImage.transform.parent.gameObject.SetActive(false);

        if (pick.weaponObtained != null)
        {
            pick.weaponObtained.GetComponent<Resetter>().resetMe();
            pick.weaponObtained = null;
        }
        Vibration.Vibrate(200);
    }

    public void playPunchAudio()
    {
        hitaudio.Play();
    }

    public void playfallaudio()
    {
        hitaudio2.Play();
    }

    private IEnumerator stunMe()
    {
        pick.animHandler.anim.SetBool("Picked", false);
        pick.animHandler.anim.SetBool("Stun", true);
        yield return new WaitForSeconds(2f);
        isStunned = false;
        pick.animHandler.anim.SetBool("Stun", false);
        gameObject.layer = 7;
    }

}
