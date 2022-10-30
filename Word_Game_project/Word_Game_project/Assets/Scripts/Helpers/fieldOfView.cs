using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldOfView : MonoBehaviour
{
    [Range(0.1f, 2f)]
    public float detectionFrequency;
    public float viewAngle, radius;
    public LayerMask detectionMask, obstacleMask;
    public List<Transform> detectedObjects = new List<Transform>();
    public Transform rayPoint;
    public string detectionTag;

    private void Start()
    {
        StartCoroutine(detect());
    }

    private IEnumerator detect()
    {
        while (true)
        {
            while (true)
            {
                yield return new WaitForSeconds(detectionFrequency);

                detectedObjects.Clear();

                Collider[] cols = Physics.OverlapSphere(transform.position, radius, detectionMask);

                for (int i = 0; i < cols.Length; i++)
                {
                    Vector3 dir = (cols[i].transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(dir, transform.forward);

                    if (angle <= viewAngle / 2)
                    {
                        RaycastHit hit;

                        Vector3 direction = cols[i].transform.position - transform.position;

                        if (Physics.Raycast(rayPoint.position, direction, out hit, radius, obstacleMask))
                        {
                            Debug.Log(hit.transform.name + " is in the way");
                        }
                        else
                        {
                            if (!detectedObjects.Contains(cols[i].transform))
                            {
                                detectedObjects.Add(cols[i].transform);
                            }
                        }
                    }
                }

            }
        }
    }

}
