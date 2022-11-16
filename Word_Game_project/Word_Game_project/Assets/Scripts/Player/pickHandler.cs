using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickHandler : MonoBehaviour
{
    [HideInInspector] public LevelManager lManager;
    public thirdPersonMovement Player;
    public int actor;
    [HideInInspector] public float tempTime;
    public Image fillImage;
    public Transform pickpoint;
    public bool hasAlphabet;
    public movementAnimationController animHandler;
    public thirdPersonMovement movementHandler;
    public Color pickColor;
    public Color darkerPickCol;
    [SerializeField] private GameObject pickedAlphabet;
    public GameObject pickButton;
    private Transform PlacementPos;
    public Transform weaponParent, weaponBackParent;
    public GameObject weaponObtained;
    private float tempVibrate;

    private void Update()
    {
        if (Player.controller.velocity.magnitude >= 0.1 && hasAlphabet)
        {
            tempVibrate += Time.deltaTime;
            if (tempVibrate >= 0.2f)
            {
                Vibration.Vibrate(100);
                tempVibrate = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickable" && !hasAlphabet)
        {
            tempTime = 0;
            other.GetComponent<curveFollower>().targetNull();
            other.GetComponent<curveFollower>().enabled = true;
            fillImage.color = pickColor;
        }

        if (other.tag == "Holder" && hasAlphabet && other.GetComponent<alphabetHolder>().Alphabet == pickedAlphabet.GetComponent<alphabet>().letter && other.GetComponent<alphabetHolder>().actor == actor)
        {
            fillImage.color = pickColor;
            tempTime = 4;
            PlacementPos = other.GetComponent<alphabetHolder>().placementPoint;
            pickedAlphabet.GetComponent<curveFollower>().finalRot = Quaternion.Euler(-90, -180, 0);
            pickedAlphabet.GetComponent<curveFollower>().targetNull();
            pickedAlphabet.GetComponent<curveFollower>().enabled = true;
        }

        if (other.tag == "Weapon" && weaponObtained == null && !hasAlphabet)
        {
            weaponObtained = other.gameObject;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.parent = weaponParent;
            weaponObtained.transform.localRotation = Quaternion.identity;
            weaponObtained.transform.localPosition = Vector3.zero;
            weaponObtained.transform.localScale = new Vector3(5, 5, 5);
            weaponObtained.tag = "Untagged";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Player.isStunned)
        {
            if (!hasAlphabet)
            {
                if (other.tag == "Pickable" && other.GetComponent<alphabet>().actor == actor)
                {
                    if (!other.GetComponent<curveFollower>().enabled)
                    {
                        other.GetComponent<curveFollower>().enabled = true;
                    }

                    tempTime += Time.deltaTime;
                    fillImage.transform.parent.gameObject.SetActive(true);
                    fillImage.fillAmount = tempTime / 2;

                    if (tempTime >= 2)
                    {
                        pickedAlphabet = other.gameObject;
                        pickedAlphabet.GetComponent<curveFollower>().finalRot = Quaternion.Euler(0, 0, 0);
                        pickedAlphabet.transform.rotation = pickedAlphabet.GetComponent<curveFollower>().finalRot;
                        other.GetComponent<curveFollower>().setMyTarget(pickpoint, pickpoint.localPosition);

                        other.tag = "Picked";
                        other.transform.localScale = new Vector3(80, 80, 80);
                        hasAlphabet = true;
                        Vibration.Vibrate(200);
                        other.GetComponent<alphabet>().LetterPicked();
                        StartCoroutine(wait());
                        tempTime = 0;
                        fillImage.transform.parent.gameObject.SetActive(false);
                        fillImage.fillAmount = 0;

                        if (weaponObtained != null)
                        {
                            weaponObtained.transform.parent = weaponBackParent;
                            weaponObtained.transform.localRotation = Quaternion.identity;
                            weaponObtained.transform.localPosition = Vector3.zero;
                            weaponObtained.transform.localScale = new Vector3(5, 5, 5);
                        }

                    }
                }                
            }

            else
            {
                if (other.tag == "Holder" && PlacementPos != null && !other.GetComponent<alphabetHolder>().takenOver)
                {
                    tempTime -= Time.deltaTime;
                    fillImage.transform.parent.gameObject.SetActive(true);
                    fillImage.fillAmount = tempTime / 4;

                    if (tempTime <= 0)
                    {
                        other.GetComponent<alphabetHolder>().takenOver = true;
                        hasAlphabet = false;
                        lManager.playerplacement();
                        if (weaponObtained != null)
                        {
                            weaponObtained.transform.parent = weaponParent;
                            weaponObtained.transform.localRotation = Quaternion.identity;
                            weaponObtained.transform.localPosition = Vector3.zero;
                            weaponObtained.transform.localScale = new Vector3(5, 5, 5);
                            weaponObtained.tag = "Untagged";
                        }
                        pickedAlphabet.layer = 8;
                        pickedAlphabet.GetComponent<alphabet>().Holder = other.transform;
                        pickedAlphabet.GetComponent<alphabet>().pickColor = pickColor;
                        pickedAlphabet.GetComponent<alphabet>().finalPlacement();
                        pickedAlphabet.GetComponent<curveFollower>().setMyTarget(EffectsManager.Instance.instParent, PlacementPos.position);
                        pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
                        pickedAlphabet.transform.tag = "PlayerDrop";
                        hasAlphabet = false;
                        StartCoroutine(wait());
                        StartCoroutine(placementWait(pickedAlphabet));
                        pickedAlphabet = null;
                        tempTime = 0;
                        fillImage.transform.parent.gameObject.SetActive(false);
                        fillImage.fillAmount = 0;
                        PlacementPos = null;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Player.isStunned)
        {
            if (other.tag == "Pickable" || other.tag == "Picked" || other.tag == "EnemyDrop" || other.tag == "PlayerDrop")
            {
                tempTime = 0;
                fillImage.transform.parent.gameObject.SetActive(false);
                fillImage.fillAmount = 0;
            }

            if (other.tag == "Holder" && hasAlphabet && other.GetComponent<alphabetHolder>().Alphabet == pickedAlphabet.GetComponent<alphabet>().letter && other.GetComponent<alphabetHolder>().actor == actor)
            {
                PlacementPos = null;
                tempTime = 0;
                fillImage.transform.parent.gameObject.SetActive(false);
                fillImage.fillAmount = 1;
            }
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        animHandler.anim.SetBool("Picked", true);
        movementHandler.speed = 7;
        pickButton.SetActive(true);
    }

    private IEnumerator placementWait(GameObject go)
    {
        go.GetComponent<alphabet>().LetterDropped();
        go.GetComponent<alphabet>().callFinalDrop();
        go.GetComponent<alphabet>().placed = true;
        animHandler.anim.SetBool("Picked", false);
        yield return new WaitForSeconds(0.5f);
        go.GetComponent<dropEffect>().dropped();
        CinemachineShake.Instance.ShakeCamera(2, 0.3f);
        Vibration.Vibrate(200);
        StartCoroutine(waitVIb());
        movementHandler.speed = 10;
        pickButton.SetActive(false);
        animHandler.anim.SetBool("Picked", false);
    }    

    private IEnumerator waitVIb()
    {
        yield return new WaitForSeconds(0.2f);
        Vibration.Vibrate(200);
    }

    public void dropAlphabet()
    {
        if (pickedAlphabet != null)
        {
            if (weaponObtained != null)
            {
                weaponObtained.transform.parent = weaponParent;
                weaponObtained.transform.localRotation = Quaternion.identity;
                weaponObtained.transform.localPosition = Vector3.zero;
                weaponObtained.transform.localScale = new Vector3(5, 5, 5);
                weaponObtained.tag = "Untagged";
            }
            pickedAlphabet.transform.parent = EffectsManager.Instance.instParent;
            pickedAlphabet.GetComponent<curveFollower>().enabled = true;
            pickedAlphabet.GetComponent<curveFollower>().targetNull();

            animHandler.anim.SetBool("Picked", false);
            movementHandler.speed = 10;
            pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
            pickedAlphabet.transform.tag = "Pickable";
            pickedAlphabet.GetComponent<alphabet>().LetterDropped();
            pickedAlphabet = null;
            pickButton.SetActive(false);
            hasAlphabet = false;
            fillImage.transform.parent.gameObject.SetActive(false);
        }
    }

    public void hideweapon()
    {
        if (weaponObtained != null)
        {
            weaponObtained.SetActive(false);
        }
    }
}
