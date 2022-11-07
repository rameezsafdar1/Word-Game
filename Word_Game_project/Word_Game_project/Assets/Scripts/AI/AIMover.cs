using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIMover : MonoBehaviour, iDamagable
{
    public Transform Player;
    public int actor;
    private NavMeshAgent Agent;
    public List<string> word = new List<string>();
    public List<Transform> pickDestinations = new List<Transform>();
    public List<Transform> dropDestinations = new List<Transform>();
    [SerializeField] private Transform currentDestination;
    [SerializeField] private bool isFollowing;
    public bool hasAlphabet;
    public Animator anim;
    private float tempTime;
    public Image fillImage;
    public GameObject pickedAlphabet;
    public Transform pickpoint;
    public Color pickColor, darkerPickColor;
    private Transform PlacementPos;
    public bool isStunned;
    private Quaternion lastrot;
    [HideInInspector] public bool isAggressive;
    public colorLerp pulse;
    private int currentDrop;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        for (int i = 0; i < dropDestinations.Count; i++)
        {
            word.Add(dropDestinations[i].GetComponent<alphabetHolder>().Alphabet);
        }
    }

    private void Update()
    {
        anim.SetFloat("Velocity", Agent.velocity.magnitude);

        checkDestionations();


        if (dropDestinations.Count > 0 && !isStunned && !isAggressive)
        {
            if (!hasAlphabet)
            {
                anim.SetBool("Picked", false);
                if (!isFollowing)
                {
                    int x = Random.Range(0, pickDestinations.Count);

                    for (int h = 0; h < word.Count; h++)
                    {
                        if (pickDestinations[x].GetComponent<alphabet>().letter == word[h])
                        {
                            for (int i = 0; i < dropDestinations.Count; i++)
                            {
                                if (pickDestinations[x].GetComponent<alphabet>().letter == dropDestinations[i].GetComponent<alphabetHolder>().Alphabet)
                                {
                                    if (!pickDestinations[x].GetComponent<alphabet>().picked && pickDestinations[x] != currentDestination)
                                    {
                                        currentDrop = i;
                                        Agent.isStopped = false;
                                        isFollowing = true;
                                        currentDestination = pickDestinations[x];
                                        Agent.SetDestination(currentDestination.position);
                                        word.RemoveAt(h);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                else
                {
                    alphabet a = currentDestination.GetComponent<alphabet>();
                    if (a != null && a.picked)
                    {
                        isFollowing = false;
                        currentDestination = null;
                        return;
                    }


                    if (currentDestination != null && currentDestination.GetComponent<alphabet>() != null)
                    {
                        if (currentDestination.GetComponent<alphabet>().picked && currentDestination.GetComponent<alphabet>().placed)
                        {
                            Agent.ResetPath();
                            Agent.isStopped = true;
                            Agent.velocity = Vector3.zero;
                            isFollowing = false;
                        }
                    }
                    else
                    {
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

        if (isStunned)
        {
            transform.rotation = lastrot;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isStunned)
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
                pickedAlphabet.GetComponent<curveFollower>().finalRot = Quaternion.Euler(-90, -180, 0);
                pickedAlphabet.GetComponent<curveFollower>().targetNull();
                pickedAlphabet.GetComponent<curveFollower>().enabled = true;
            }            

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isStunned)
        {
            if (!hasAlphabet)
            {
                if (currentDestination == other.transform)
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
                            isFollowing = false;
                            other.GetComponent<alphabet>().picked = true;
                            StartCoroutine(wait());
                            tempTime = 0;
                            fillImage.transform.parent.gameObject.SetActive(false);
                            fillImage.fillAmount = 0;
                        }
                    }

                    if (other.tag == "PlayerDrop" || other.tag == "EnemyDrop")
                    {
                        if (!other.GetComponent<curveFollower>().enabled)
                        {
                            other.GetComponent<curveFollower>().enabled = true;
                        }

                        tempTime += Time.deltaTime;
                        fillImage.transform.parent.gameObject.SetActive(true);
                        fillImage.fillAmount = tempTime / 8;


                        if (tempTime >= 8)
                        {
                            pickedAlphabet = other.gameObject;
                            pickedAlphabet.layer = 0;
                            pickedAlphabet.GetComponent<curveFollower>().finalRot = Quaternion.Euler(0, 0, 0);
                            pickedAlphabet.transform.rotation = pickedAlphabet.GetComponent<curveFollower>().finalRot;
                            other.GetComponent<curveFollower>().setMyTarget(pickpoint, pickpoint.localPosition);
                            other.GetComponent<materialChanger>().changeColor(pickColor);
                            other.tag = "Picked";
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

                    if (other.tag == "PlayerDrop" && other.transform == currentDestination)
                    {
                        pulse.col = pickColor;
                        pulse.darkcol = darkerPickColor;
                        pulse.enabled = true;
                    }

                }
            }

            else
            {
                if (currentDestination == other.transform)
                {
                    if (other.tag == "Holder" && PlacementPos != null && other.GetComponent<alphabetHolder>().Alphabet == pickedAlphabet.GetComponent<alphabet>().letter && other.GetComponent<alphabetHolder>().actor == actor)
                    {
                        tempTime -= Time.deltaTime;
                        fillImage.transform.parent.gameObject.SetActive(true);
                        fillImage.fillAmount = tempTime / 4;

                        if (tempTime <= 0)
                        {
                            word.Clear();
                            pulse.mat.color = Color.white;
                            pulse.enabled = false;

                            hasAlphabet = false;
                            pickedAlphabet.GetComponent<curveFollower>().setMyTarget(EffectsManager.Instance.instParent, PlacementPos.position);
                            pickedAlphabet.GetComponent<alphabet>().Holder = other.transform;

                            Agent.ResetPath();
                            Agent.isStopped = true;
                            Agent.velocity = Vector3.zero;

                            pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
                            currentDestination = null;
                            hasAlphabet = false;
                            StartCoroutine(wait());
                            StartCoroutine(placementWait(pickedAlphabet));
                            pickedAlphabet = null;
                            tempTime = 0;
                            fillImage.transform.parent.gameObject.SetActive(false);
                            fillImage.fillAmount = 0;

                            dropDestinations.RemoveAt(currentDrop);
                            isFollowing = false;
                        }
                    }

                    if (pickedAlphabet != null && other.tag == "Holder" && other.GetComponent<alphabetHolder>().Alphabet != pickedAlphabet.GetComponent<alphabet>().letter)
                    {
                        dropAlphabet();
                    }
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!isStunned)
        {
            if (other.tag == "Pickable" || other.tag == "Picked")
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

        if (other.tag == "Picked")
        {
            pulse.mat.color = Color.white;
            pulse.enabled = false;
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Picked", true);
        Agent.speed = 9;
    }

    private IEnumerator placementWait(GameObject go)
    {
        go.GetComponent<alphabet>().picked = false;
        go.GetComponent<alphabet>().placed = true;
        anim.SetBool("Picked", false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(waitRepick(go));
        Agent.speed = 15;
        anim.SetBool("Picked", false);
    }

    private IEnumerator waitRepick(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        go.tag = "EnemyDrop";
        go.GetComponent<alphabet>().ai = this;
    }

    public void dropAlphabet()
    {
        if (pickedAlphabet != null)
        {
            Debug.Log("had something");
            isFollowing = false;            
            pickedAlphabet.transform.parent = EffectsManager.Instance.instParent;
            pickedAlphabet.GetComponent<curveFollower>().enabled = true;
            pickedAlphabet.GetComponent<curveFollower>().targetNull();
            pickedAlphabet.GetComponent<materialChanger>().changeColor(Color.white);
            anim.SetBool("Picked", false);
            Agent.speed = 15;
            pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
            StartCoroutine(waitRetag(pickedAlphabet));
            pickedAlphabet.GetComponent<alphabet>().picked = false;
            pickedAlphabet = null;
            hasAlphabet = false;
            fillImage.transform.parent.gameObject.SetActive(false);
        }
    }

    private IEnumerator waitRetag(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        obj.transform.tag = "Pickable";
    }

    public void takeDamage()
    {
        if (pickedAlphabet != null)
        {
            pickedAlphabet.GetComponent<curveFollower>().visual.localPosition = Vector3.zero;
        }

        gameObject.layer = 0;
        isStunned = true;
        dropAlphabet();
        
        StartCoroutine(stunMe());

        Agent.ResetPath();
        Agent.velocity = Vector3.zero;
        isFollowing = false;
        Agent.speed = 20;
        lastrot = Agent.transform.rotation;
        fillImage.transform.parent.gameObject.SetActive(false);

    }

    private IEnumerator stunMe()
    {
        anim.applyRootMotion = true;
        anim.SetBool("Picked", false);
        anim.SetBool("Stun", true);
        yield return new WaitForSeconds(5f);
        isStunned = false;
        anim.SetBool("Stun", false);
        anim.applyRootMotion = false;
        Agent.speed = 15;
        gameObject.layer = 6;
    }

    public void makeMeAggressive()
    {
        if (!isStunned)
        {
            anim.SetBool("Picked", false);
            dropAlphabet();
            isAggressive = true;
            Agent.SetDestination(Player.position);
        }
    }

    public void calmMeDown()
    {
        Agent.isStopped = false;
        isAggressive = false;
        isFollowing = true;
        StartCoroutine(waitRetarget());
    }

    private IEnumerator waitRetarget()
    {
        yield return new WaitForSeconds(0.1f);
        isFollowing = false;
        yield return new WaitForSeconds(0.2f);
        isFollowing = true;
    }

    private void checkDestionations()
    {
        for (int i = 0; i < dropDestinations.Count; i++)
        {
            if (!word.Contains(dropDestinations[i].GetComponent<alphabetHolder>().Alphabet))
            {
                word.Add(dropDestinations[i].GetComponent<alphabetHolder>().Alphabet);
            }
        }
    }

}
