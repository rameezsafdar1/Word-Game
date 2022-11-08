using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class visualGraph : MonoBehaviour
{
    public AnimationCurve Curve;
    public float timeToReach;
    [HideInInspector]
    public float timer;
    public Transform visual;
    public UnityEvent onReached;
    [HideInInspector]
    public Quaternion finalRot;
    private bool started;

    private void Update()
    {
        if (started)
        {
            timer += Time.deltaTime;

            Vector3 distance = new Vector3(visual.localPosition.x, Curve.Evaluate(timer), visual.localPosition.z);

            visual.localPosition = distance;           

            if (timer >= timeToReach)
            {
                visual.transform.localRotation = Quaternion.identity;
                visual.transform.localPosition = Vector3.zero;

                if (onReached != null)
                {
                    onReached.Invoke();
                }
                this.enabled = false;
            }


        }
    }

    public void startMoving()
    {
        timer = 0;
        started = true;
    }

}
