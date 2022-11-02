using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickHandler : MonoBehaviour
{
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
    public colorLerp pulse;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickable" && !hasAlphabet)
        {
            tempTime = 0;
            other.GetComponent<curveFollower>().targetNull();
            other.GetComponent<curveFollower>().enabled = true;
            fillImage.color = pickColor;
        }

        if (other.tag == "EnemyDrop" && !hasAlphabet)
        {
            tempTime = 0;
            other.GetComponent<curveFollower>().targetNull();
            other.GetComponent<curveFollower>().enabled = true;

            fillImage.color = other.GetComponent<alphabet>().ai.pickColor;

            if (!Player.isStunned)
            {
                pulse.col = pickColor;
                pulse.darkcol = darkerPickCol;
                pulse.enabled = true;
                other.GetComponent<alphabet>().ai.makeMeAggressive();
            }
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

        if (other.tag == "Weapon" && weaponObtained == null)
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
                if (other.tag == "Pickable")
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
                        other.GetComponent<materialChanger>().changeColor(pickColor);
                        other.tag = "Picked";
                        other.transform.localScale = new Vector3(80, 80, 80);
                        hasAlphabet = true;
                        other.GetComponent<alphabet>().picked = true;
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

                else if (other.tag == "EnemyDrop")
                {
                    other.GetComponent<curveFollower>().enabled = true;
                    tempTime += Time.deltaTime;
                    fillImage.transform.parent.gameObject.SetActive(true);
                    fillImage.fillAmount = tempTime / 8;
                    other.GetComponent<alphabet>().ai.makeMeAggressive();
                    if (tempTime >= 8)
                    {
                        pulse.mat.color = Color.white;
                        pulse.enabled = false;
                        pickedAlphabet = other.gameObject;

                        pickedAlphabet.GetComponent<alphabet>().ai.calmMeDown();
                        pickedAlphabet.GetComponent<alphabet>().ai.currentDrop--;
                        pickedAlphabet.GetComponent<curveFollower>().finalRot = Quaternion.Euler(0, 0, 0);
                        pickedAlphabet.transform.rotation = pickedAlphabet.GetComponent<curveFollower>().finalRot;
                        pickedAlphabet.GetComponent<curveFollower>().setMyTarget(pickpoint, pickpoint.localPosition);
                        pickedAlphabet.GetComponent<materialChanger>().changeColor(pickColor);
                        pickedAlphabet.tag = "Picked";
                        pickedAlphabet.transform.localScale = new Vector3(80, 80, 80);
                        hasAlphabet = true;
                        pickedAlphabet.GetComponent<alphabet>().picked = true;
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
                if (other.tag == "Holder" && PlacementPos != null)
                {
                    tempTime -= Time.deltaTime;
                    fillImage.transform.parent.gameObject.SetActive(true);
                    fillImage.fillAmount = tempTime / 4;

                    if (tempTime <= 0)
                    {
                        hasAlphabet = false;

                        if (weaponObtained != null)
                        {
                            weaponObtained.transform.parent = weaponParent;
                            weaponObtained.transform.localRotation = Quaternion.identity;
                            weaponObtained.transform.localPosition = Vector3.zero;
                            weaponObtained.transform.localScale = new Vector3(5, 5, 5);
                            weaponObtained.tag = "Untagged";
                        }

                        pickedAlphabet.GetComponent<alphabet>().ai = null;
                        pickedAlphabet.GetComponent<curveFollower>().setMyTarget(EffectsManager.Instance.instParent, PlacementPos.position);
                        pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);                        
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
            if (other.tag == "Pickable" || other.tag == "Picked" || other.tag == "EnemyDrop")
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
        if (other.tag == "EnemyDrop")
        {
            tempTime = 0;
            other.GetComponent<alphabet>().ai.calmMeDown();
            pulse.enabled = false;
            pulse.mat.color = Color.white;
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
        go.GetComponent<alphabet>().picked = false;
        go.GetComponent<alphabet>().placed = true;
        animHandler.anim.SetBool("Picked", false);
        yield return new WaitForSeconds(0.5f);
        go.GetComponent<dropEffect>().dropped();
        CinemachineShake.Instance.ShakeCamera(2, 0.3f);
        Vibration.Vibrate(8);
        //StartCoroutine(waitRepick(go));
        movementHandler.speed = 10;
        pickButton.SetActive(false);
        animHandler.anim.SetBool("Picked", false);
    }

    private IEnumerator waitRepick(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        go.tag = "Pickable"; 
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
            pickedAlphabet.GetComponent<materialChanger>().changeColor(Color.white);
            animHandler.anim.SetBool("Picked", false);
            movementHandler.speed = 10;
            pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
            pickedAlphabet.transform.tag = "Pickable";
            pickedAlphabet.GetComponent<alphabet>().picked = false;
            pickedAlphabet = null;
            pickButton.SetActive(false);
            hasAlphabet = false;
            fillImage.transform.parent.gameObject.SetActive(false);
        }
    }

}
