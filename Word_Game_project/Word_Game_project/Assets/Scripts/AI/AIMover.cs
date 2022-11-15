using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;

public class AIMover : MonoBehaviour, iDamagable
{
    public GameManager Manager;
    public int actor;
    public float runSpeed, pushSpeed;
    private NavMeshAgent Agent;
    public List<Transform> pickDestinations = new List<Transform>();
    public List<Transform> dropDestinations = new List<Transform>();
    [SerializeField] private Transform currentDestination;
    public bool hasAlphabet;
    public Animator anim;
    [SerializeField] private float tempTime;
    public Image fillImage;
    public GameObject pickedAlphabet;
    public Transform pickpoint;
    public Color pickColor;
    private Transform PlacementPos;
    public bool isStunned;
    private Quaternion lastrot;
    private GameObject weaponObtained;
    public Transform weaponParent, weaponBackParent;
    private curveFollower cf;
    public Transform DownRay;
    public LayerMask detectionLayer;
    public UnityEvent onPlacement;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        cf = GetComponent<curveFollower>();
    }

    private void Update()
    {
        if (Manager.gameStarted && !Manager.gameCompleted)
        {
            anim.SetFloat("Velocity", Agent.velocity.magnitude);


            if (!isStunned)
            {
                if (dropDestinations.Count > 0)
                {
                    if (!hasAlphabet)
                    {
                        anim.SetBool("Picked", false);
                        currentDestination = pickDestinations[0];
                        Agent.SetDestination(pickDestinations[0].position);
                    }

                    else
                    {
                        currentDestination = dropDestinations[0];
                        Agent.SetDestination(dropDestinations[0].position);
                    }
                }
            }

            else
            {
                transform.rotation = lastrot;

                RaycastHit hit;

                if (!Physics.Raycast(DownRay.position, DownRay.forward, out hit, 150, detectionLayer))
                {
                    transform.position += new Vector3(0, -1, 0) * 20 * Time.deltaTime;
                }
            }
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
                tempTime = 0;
                PlacementPos = other.GetComponent<alphabetHolder>().placementPoint;
                pickedAlphabet.GetComponent<curveFollower>().finalRot = Quaternion.Euler(-90, -180, 0);
                pickedAlphabet.GetComponent<curveFollower>().targetNull();
                pickedAlphabet.GetComponent<curveFollower>().enabled = true;
            }

            if (other.tag == "Weapon" && weaponObtained == null && !hasAlphabet)
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
        if (other.tag == "Finish")
        {
            gameObject.SetActive(false);
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
                            hasAlphabet = true;
                            pickedAlphabet = other.gameObject;
                            pickedAlphabet.GetComponent<curveFollower>().finalRot = Quaternion.Euler(0, 0, 0);
                            pickedAlphabet.transform.rotation = pickedAlphabet.GetComponent<curveFollower>().finalRot;
                            other.GetComponent<curveFollower>().setMyTarget(pickpoint, pickpoint.localPosition);

                            other.tag = "Picked";
                            other.transform.localScale = new Vector3(80, 80, 80);
                            
                            other.GetComponent<alphabet>().LetterPicked();
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
            }

            else
            {
                if (currentDestination == other.transform)
                {
                    PlacementPos = other.GetComponent<alphabetHolder>().placementPoint;

                    tempTime += Time.deltaTime;
                    fillImage.transform.parent.gameObject.SetActive(true);
                    fillImage.fillAmount = 1 - (tempTime / 4);
                    fillImage.color = pickColor;

                    if (tempTime >= 4)
                    {
                        dropDestinations.RemoveAt(0);
                        pickDestinations.RemoveAt(0);
                        pickedAlphabet.GetComponent<curveFollower>().setMyTarget(EffectsManager.Instance.instParent, PlacementPos.position);
                        pickedAlphabet.GetComponent<curveFollower>().enabled = true;

                        pickedAlphabet.GetComponent<alphabet>().finalPlacement();
                        pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
                        currentDestination = null;

                        StartCoroutine(wait());
                        StartCoroutine(placementWait(pickedAlphabet));

                        pickedAlphabet = null;
                        tempTime = 0;
                        fillImage.transform.parent.gameObject.SetActive(false);
                        fillImage.fillAmount = 0;
                        currentDestination = null;
                        Agent.ResetPath();

                        if (onPlacement != null)
                        {
                            onPlacement.Invoke();
                        }

                        if (weaponObtained != null)
                        {
                            weaponObtained.transform.parent = weaponParent;
                            weaponObtained.transform.localRotation = Quaternion.identity;
                            weaponObtained.transform.localPosition = Vector3.zero;
                            weaponObtained.transform.localScale = new Vector3(5, 5, 5);
                            weaponObtained.tag = "Untagged";
                        }

                        hasAlphabet = false;

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
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Picked", true);
        Agent.speed = pushSpeed;
    }

    private IEnumerator placementWait(GameObject go)
    {
        go.GetComponent<alphabet>().LetterDropped();
        go.GetComponent<alphabet>().placed = true;
        anim.SetBool("Picked", false);
        yield return new WaitForSeconds(0.5f);
        Agent.speed = runSpeed;
        anim.SetBool("Picked", false);
    }
    
    public void dropAlphabet()
    {
        if (pickedAlphabet != null)
        {   
            pickedAlphabet.transform.parent = EffectsManager.Instance.instParent;
            pickedAlphabet.GetComponent<curveFollower>().enabled = true;
            pickedAlphabet.GetComponent<curveFollower>().targetNull();

            anim.SetBool("Picked", false);
            Agent.speed = runSpeed;
            pickedAlphabet.transform.localScale = new Vector3(55, 55, 55);
            StartCoroutine(waitRetag(pickedAlphabet));
            pickedAlphabet.GetComponent<alphabet>().LetterDropped();
            pickedAlphabet = null;
            hasAlphabet = false;
            fillImage.transform.parent.gameObject.SetActive(false);
            tempTime = 0;
        }
    }

    private IEnumerator waitRetag(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        obj.transform.tag = "Pickable";
    }

    public void takeDamage(Vector3 direction)
    {
        cf.finalRot = Agent.transform.rotation;
        lastrot = Agent.transform.rotation;

        if (pickedAlphabet != null)
        {
            pickedAlphabet.GetComponent<curveFollower>().visual.localPosition = Vector3.zero;
        }

        

        gameObject.layer = 0;
        isStunned = true;
        dropAlphabet();
        
        StartCoroutine(stunMe());

        if (weaponObtained != null)
        {
            weaponObtained.GetComponent<Resetter>().resetMe();
            weaponObtained = null;
        }

        Agent.ResetPath();
        Agent.velocity = Vector3.zero;
        Agent.speed = runSpeed;
        Agent.isStopped = true;
        Agent.enabled = false;

        Vector3 newDir = transform.localPosition + new Vector3(direction.x * 2, 0, direction.z * 2);

        cf.setMyTarget(transform.parent, newDir);
        cf.enabled = true;

        fillImage.transform.parent.gameObject.SetActive(false);
        anim.SetBool("Picked", false);
        anim.SetBool("Stun", true);
    }

    private IEnumerator stunMe()
    {
        anim.SetBool("Picked", false);
        anim.SetBool("Stun", true);
        yield return new WaitForSeconds(3f);
        Agent.enabled = true;
        isStunned = false;
        anim.SetBool("Stun", false);
        Agent.speed = runSpeed;
        gameObject.layer = 6;
        Agent.isStopped = false;
    }

}
