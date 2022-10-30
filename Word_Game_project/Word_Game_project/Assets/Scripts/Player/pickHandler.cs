using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickHandler : MonoBehaviour
{
    private float tempTime;
    public Image fillImage;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Pickable")
        {
            tempTime += Time.deltaTime;
            fillImage.transform.parent.gameObject.SetActive(true);
            fillImage.fillAmount = tempTime / 2;
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

}
