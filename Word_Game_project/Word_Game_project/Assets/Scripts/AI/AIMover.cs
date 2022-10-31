using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMover : MonoBehaviour
{
    private NavMeshAgent Agent;
    public List<Transform> pickDestinations = new List<Transform>();
    public List<Transform> dropDestinations = new List<Transform>();
    [SerializeField] private Transform currentDestination;
    public bool isFollowing;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isFollowing)
        {
            int x = Random.Range(0, pickDestinations.Count);
            if (!pickDestinations[x].GetComponent<alphabet>().picked)
            {
                Agent.isStopped = false;
                isFollowing = true;
                currentDestination = pickDestinations[x];
                Agent.SetDestination(currentDestination.position);
            }

        }
        else
        {
            if (currentDestination.GetComponent<alphabet>().picked)
            {
                Agent.ResetPath();
                Agent.isStopped = true;
                Agent.velocity = Vector3.zero;
                isFollowing = false;
            }
        }
    }

}
