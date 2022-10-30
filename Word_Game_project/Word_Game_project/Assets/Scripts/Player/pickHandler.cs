using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickHandler : MonoBehaviour
{
    private float tempTime;
    public Image fillImage;
    public Transform pickpoint;
    private bool hasAlphabet;
    public movementAnimationController animHandler;
    public thirdPersonMovement movementHandler;
    public Color pickColor;

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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pickable")
        {
            tempTime = 0;
            fillImage.transform.parent.gameObject.SetActive(false);
            fillImage.fillAmount = 0;
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        animHandler.anim.SetBool("Picked", true);
        movementHandler.speed = 7;
    }

}
