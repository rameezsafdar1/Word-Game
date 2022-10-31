using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickHandler : MonoBehaviour
{
    public int actor;
    private float tempTime;
    public Image fillImage;
    public Transform pickpoint;
    [SerializeField] private bool hasAlphabet;
    public movementAnimationController animHandler;
    public thirdPersonMovement movementHandler;
    public Color pickColor;
    private GameObject pickedAlphabet;
    public GameObject pickButton;
    private Transform PlacementPos;

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Pickable" && !hasAlphabet)
        {
            tempTime = 0;
        }

        if (other.tag == "Holder" && hasAlphabet && other.GetComponent<alphabetHolder>().Alphabet == pickedAlphabet.GetComponent<alphabet>().letter)
        {
            tempTime = 2;
            PlacementPos = other.GetComponent<alphabetHolder>().placementPoint;
            pickedAlphabet.GetComponent<curveFollower>().finalRot = Quaternion.Euler(90, 0, 0);
            pickedAlphabet.GetComponent<curveFollower>().targetNull();
            pickedAlphabet.GetComponent<curveFollower>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hasAlphabet)
        {
            if (other.tag == "Pickable")
            {
                tempTime += Time.deltaTime;
                fillImage.transform.parent.gameObject.SetActive(true);
                fillImage.fillAmount = tempTime / 2;

                if (tempTime >= 2)
                {
                    pickedAlphabet = other.gameObject;
                    other.GetComponent<curveFollower>().setMyTarget(pickpoint, pickpoint.localPosition);
                    other.GetComponent<materialChanger>().changeColor(pickColor);
                    other.tag = "Untagged";
                    other.transform.localScale = new Vector3(80, 80, 80);                    
                    hasAlphabet = true;
                    StartCoroutine(wait());
                    tempTime = 0;
                    fillImage.transform.parent.gameObject.SetActive(false);
                    fillImage.fillAmount = 0;
                }
            }
        }

        else
        {
            if (other.tag == "Holder" && PlacementPos != null)
            {
                tempTime -= Time.deltaTime;
                fillImage.transform.parent.gameObject.SetActive(true);
                fillImage.fillAmount = tempTime / 2;

                if (tempTime <= 0)
                {
                    hasAlphabet = false;
                    pickedAlphabet.GetComponent<curveFollower>().setMyTarget(EffectsManager.Instance.instParent, PlacementPos.localPosition);                    
                    pickedAlphabet.GetComponent<materialChanger>().changeColor(Color.yellow);
                    pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
                    hasAlphabet = false;
                    StartCoroutine(wait());
                    StartCoroutine(placementWait());
                    tempTime = 2;
                    fillImage.transform.parent.gameObject.SetActive(false);
                    fillImage.fillAmount = 0;
                    PlacementPos = null;
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pickable")
        {
            tempTime = 0;
            fillImage.transform.parent.gameObject.SetActive(false);
            fillImage.fillAmount = 0;
        }

        if (other.tag == "Holder" && hasAlphabet)
        {
            PlacementPos = null;
            tempTime = 2;
            fillImage.transform.parent.gameObject.SetActive(false);
            fillImage.fillAmount = 1;
        }

    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        animHandler.anim.SetBool("Picked", true);
        movementHandler.speed = 7;
        pickButton.SetActive(true);
    }

    private IEnumerator placementWait()
    {
        animHandler.anim.SetBool("Picked", false);
        yield return new WaitForSeconds(0.5f);
        //pickedAlphabet.tag = "Pickable"; 
        movementHandler.speed = 10;
        pickButton.SetActive(false);
        animHandler.anim.SetBool("Picked", false);
    }


    public void dropAlphabet()
    {
        if (pickedAlphabet != null)
        {
            pickedAlphabet.transform.parent = EffectsManager.Instance.instParent;
            pickedAlphabet.GetComponent<curveFollower>().enabled = true;
            pickedAlphabet.GetComponent<curveFollower>().targetNull();
            pickedAlphabet.GetComponent<materialChanger>().changeColor(Color.white);
            animHandler.anim.SetBool("Picked", false);
            movementHandler.speed = 10;
            pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
            pickedAlphabet.transform.tag = "Pickable";
            pickedAlphabet = null;
            pickButton.SetActive(false);
            hasAlphabet = false;
        }
    }

}
