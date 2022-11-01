using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIMover : MonoBehaviour
{
    public int actor;
    private NavMeshAgent Agent;
    public List<Transform> pickDestinations = new List<Transform>();
    public List<Transform> dropDestinations = new List<Transform>();
    [SerializeField] private Transform currentDestination;
    [SerializeField] private bool isFollowing;
    [SerializeField] private bool hasAlphabet;
    public Animator anim;
    private float tempTime;
    public Image fillImage;
    private GameObject pickedAlphabet;
    public Transform pickpoint;
    public Color pickColor;
    private int currentDrop;
    private Transform PlacementPos;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        anim.SetFloat("Velocity", Agent.velocity.magnitude);

        if (currentDrop < dropDestinations.Count)
        {
            if (!hasAlphabet)
            {
                if (!isFollowing)
                {
                    int x = Random.Range(0, pickDestinations.Count);
                    if (!pickDestinations[x].GetComponent<alphabet>().picked && !pickDestinations[x].GetComponent<alphabet>().placed)
                    {
                        Agent.isStopped = false;
                        isFollowing = true;
                        currentDestination = pickDestinations[x];
                        Agent.SetDestination(currentDestination.position);
                    }
                }
                else
                {
                    if (currentDestination.GetComponent<alphabet>().picked || currentDestination.GetComponent<alphabet>().placed)
                    {
                        Agent.ResetPath();
                        Agent.isStopped = true;
                        Agent.velocity = Vector3.zero;
                        isFollowing = false;
                    }
                }
            }

            else
            {
                if (!isFollowing)
                {
                    Agent.isStopped = false;
                    currentDestination = dropDestinations[currentDrop];
                    Agent.SetDestination(currentDestination.position);
                    isFollowing = true;
                }
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
        }

        if (other.tag == "Holder" && hasAlphabet && other.GetComponent<alphabetHolder>().Alphabet == pickedAlphabet.GetComponent<alphabet>().letter && other.GetComponent<alphabetHolder>().actor == actor)
        {
            tempTime = 4;
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
                    other.tag = "Untagged";
                    other.transform.localScale = new Vector3(80, 80, 80);
                    hasAlphabet = true;
                    isFollowing = false;
                    other.GetComponent<alphabet>().picked = true;
                    StartCoroutine(wait());
                    tempTime = 0;
                    fillImage.transform.parent.gameObject.SetActive(false);
                    fillImage.fillAmount = 0;
                }
            }
        }

        else
        {
            if (other.tag == "Holder" && PlacementPos != null && other.GetComponent<alphabetHolder>().Alphabet == pickedAlphabet.GetComponent<alphabet>().letter && other.GetComponent<alphabetHolder>().actor == actor)
            {
                tempTime -= Time.deltaTime;
                fillImage.transform.parent.gameObject.SetActive(true);
                fillImage.fillAmount = tempTime / 4;

                if (tempTime <= 0)
                {
                    hasAlphabet = false;
                    pickedAlphabet.GetComponent<curveFollower>().setMyTarget(EffectsManager.Instance.instParent, PlacementPos.localPosition);

                    Agent.ResetPath();
                    Agent.isStopped = true;
                    Agent.velocity = Vector3.zero;

                    pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
                    currentDestination = null;
                    hasAlphabet = false;
                    isFollowing = false;
                    StartCoroutine(wait());
                    StartCoroutine(placementWait());
                    tempTime = 0;
                    fillImage.transform.parent.gameObject.SetActive(false);
                    fillImage.fillAmount = 0;

                    currentDrop++;
                }
            }

            if (other.tag == "Holder" && currentDestination == other.transform && other.GetComponent<alphabetHolder>().Alphabet != pickedAlphabet.GetComponent<alphabet>().letter)
            {
                dropAlphabet();
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

        if (other.tag == "Holder" && hasAlphabet && other.GetComponent<alphabetHolder>().Alphabet == pickedAlphabet.GetComponent<alphabet>().letter && other.GetComponent<alphabetHolder>().actor == actor)
        {
            PlacementPos = null;
            tempTime = 0;
            fillImage.transform.parent.gameObject.SetActive(false);
            fillImage.fillAmount = 1;
        }

    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Picked", true);
        Agent.speed = 7;
    }

    private IEnumerator placementWait()
    {
        pickedAlphabet.GetComponent<alphabet>().picked = false;
        anim.SetBool("Picked", false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(waitRepick());
        Agent.speed = 10;
        anim.SetBool("Picked", false);
    }

    private IEnumerator waitRepick()
    {
        yield return new WaitForSeconds(1f);
        pickedAlphabet.tag = "EnemyDrop";
    }

    public void dropAlphabet()
    {
        if (pickedAlphabet != null)
        {
            isFollowing = false;
            pickedAlphabet.transform.parent = EffectsManager.Instance.instParent;
            pickedAlphabet.GetComponent<curveFollower>().enabled = true;
            pickedAlphabet.GetComponent<curveFollower>().targetNull();
            pickedAlphabet.GetComponent<materialChanger>().changeColor(Color.white);
            anim.SetBool("Picked", false);
            Agent.speed = 8;
            pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
            pickedAlphabet.transform.tag = "Pickable";
            pickedAlphabet.GetComponent<alphabet>().picked = false;
            pickedAlphabet = null;
            hasAlphabet = false;
            fillImage.transform.parent.gameObject.SetActive(false);
        }
    }
}
