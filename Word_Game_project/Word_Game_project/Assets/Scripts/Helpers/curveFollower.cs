using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class curveFollower : MonoBehaviour
{
    private Vector3 target;
    public AnimationCurve Curve;
    [HideInInspector]
    public float speed;
    public float timeToReach;
    [HideInInspector]
    public float timer;
    private Vector3 startPos;
    public Transform visual;
    public UnityEvent onReached;
    [HideInInspector]
    public Quaternion finalRot;
    private void Update()
    {
        if (target.magnitude > 0.1f)
        {
            timer += Time.deltaTime;
            speed += Time.deltaTime / timeToReach;
            Vector3 distance = new Vector3(visual.localPosition.x, Curve.Evaluate(timer), visual.localPosition.z);
            visual.localPosition = distance;
            transform.localPosition = Vector3.Lerp(startPos, target, speed);

            if (timer >= timeToReach)
            {
                transform.localRotation = finalRot;
                transform.localPosition = target;
                if (onReached != null)
                {
                    onReached.Invoke();
                }
                this.enabled = false;
            }


        }
    }

    public void setMyTarget(Transform parent, Vector3 t)
    {
        timer = 0;
        speed = 0;
        transform.parent = parent;
        startPos = transform.localPosition;
        target = t;
    }

    public void targetNull()
    {
        target = Vector3.zero;
    }
}
