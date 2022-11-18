using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameManager Manager;
    public int Ai1Placements, Ai2Placements, PlayerPlacements;
    public GameObject podiumcamera;
    public Animator ai1anim, ai2anim;
    private bool resultsout;
    public Transform winposition, losepos1, losepos2;
    public GameObject Fade, confetti;
    public UnityEvent onComplete;
    [Space(20)]
    public GameObject Podium;
    public Transform win1Pos, win2Pos, playerWinPos;
    public TextMeshPro podiumtext;
    public Color ai1Col, ai2Col, pCol;

    private void Update()
    {
        if (resultsout)
        {
            ai1anim.gameObject.SetActive(true);
            ai2anim.gameObject.SetActive(true);
            Manager.Player.gameObject.SetActive(true);
        }
    }

    public void Aip1()
    {
        Ai1Placements--;
        if (Ai1Placements <= 0)
        {
            if (!resultsout)
            {
                podiumtext.color = ai1Col;
                Manager.gameCompleted = true;
                resultsout = true;
                StartCoroutine(ww1());
                StartCoroutine(waitFail());
            }
        }
    }

    private IEnumerator ww1()
    {
        ai1anim.SetTrigger("Win");
        yield return new WaitForSeconds(2f);
        Fade.SetActive(true);       

        yield return new WaitForSeconds(1.3f);

        Podium.transform.position = win1Pos.position;

        if (onComplete != null)
        {
            onComplete.Invoke();
        }

        Manager.gameCompleted = true;
        resultsout = true;

        ai1anim.GetComponent<AIMover>().hideweapon();
        podiumcamera.SetActive(true);
        ai1anim.GetComponent<NavMeshAgent>().ResetPath();
        ai2anim.GetComponent<NavMeshAgent>().ResetPath();
        ai1anim.gameObject.SetActive(true);
        ai1anim.GetComponent<NavMeshAgent>().enabled = false;
        ai1anim.transform.position = winposition.position;
        ai1anim.transform.rotation = winposition.localRotation;
        ai1anim.SetFloat("Velocity", 0);
        ai1anim.GetComponent<AIMover>().dropAlphabet();

        ai2anim.gameObject.SetActive(true);
        ai2anim.GetComponent<NavMeshAgent>().enabled = false;
        ai2anim.transform.position = losepos1.position;
        ai2anim.transform.rotation = losepos1.localRotation;
        ai2anim.SetFloat("Velocity", 0);
        ai2anim.GetComponent<AIMover>().dropAlphabet();

        Manager.Player.transform.position = losepos2.position;
        Manager.Player.transform.rotation = losepos2.localRotation;
        Manager.Player.animHandler.anim.SetFloat("Velocity", 0);
        Manager.Player.dropAlphabet();

        ai1anim.SetBool("Picked", false);
        ai1anim.SetBool("Stun", false);

        ai2anim.SetBool("Picked", false);
        ai2anim.SetBool("Stun", false);
        ai2anim.SetTrigger("Lose");

        Manager.Player.animHandler.anim.SetBool("Picked", false);
        Manager.Player.animHandler.anim.SetBool("Stun", false);
        Manager.Player.animHandler.anim.SetTrigger("Lose");
    }

    public void Aip2()
    {
        Ai2Placements--;
        if (Ai2Placements <= 0)
        {
            if (!resultsout)
            {
                podiumtext.color = ai2Col;
                Manager.gameCompleted = true;
                resultsout = true;
                StartCoroutine(ww2());
                StartCoroutine(waitFail());
            }
        }
    }

    private IEnumerator ww2()
    {
        ai2anim.SetTrigger("Win");
        yield return new WaitForSeconds(2f);
        Fade.SetActive(true);        

        yield return new WaitForSeconds(1.3f);

        Podium.transform.position = win2Pos.position;

        if (onComplete != null)
        {
            onComplete.Invoke();
        }

        Manager.gameCompleted = true;
        resultsout = true;
        ai2anim.GetComponent<AIMover>().hideweapon();
        podiumcamera.SetActive(true);

        ai1anim.GetComponent<NavMeshAgent>().ResetPath();
        ai2anim.GetComponent<NavMeshAgent>().ResetPath();

        ai2anim.GetComponent<NavMeshAgent>().enabled = false;
        ai2anim.transform.position = winposition.position;
        ai2anim.transform.rotation = winposition.localRotation;
        ai2anim.GetComponent<AIMover>().dropAlphabet();

        ai1anim.gameObject.SetActive(true);
        ai1anim.SetFloat("Velocity", 0);
        ai1anim.GetComponent<NavMeshAgent>().enabled = false;
        ai1anim.transform.position = losepos2.position;
        ai1anim.transform.rotation = losepos2.localRotation;

        ai1anim.GetComponent<AIMover>().dropAlphabet();
        ai2anim.gameObject.SetActive(true);
        ai1anim.SetFloat("Velocity", 0);

        Manager.Player.transform.position = losepos1.position;
        Manager.Player.transform.rotation = losepos1.localRotation;
        Manager.Player.animHandler.anim.SetFloat("Velocity", 0);
        Manager.Player.dropAlphabet();

        ai1anim.SetBool("Picked", false);
        ai1anim.SetBool("Stun", false);
        ai1anim.SetTrigger("Lose");

        ai2anim.SetBool("Picked", false);
        ai2anim.SetBool("Stun", false);

        Manager.Player.animHandler.anim.SetBool("Picked", false);
        Manager.Player.animHandler.anim.SetBool("Stun", false);
        Manager.Player.animHandler.anim.SetTrigger("Lose");
    }

    public void playerplacement()
    {
        PlayerPlacements--;
        if (PlayerPlacements <= 0)
        {
            podiumtext.color = pCol;
            Manager.gameCompleted = true;
            resultsout = true;
            StartCoroutine(ww3());
            Manager.GameComplete();
        }
    }

    private IEnumerator ww3()
    {
        yield return new WaitForSeconds(2f);
        Fade.SetActive(true);       

        yield return new WaitForSeconds(1.3f);

        Podium.transform.position = playerWinPos.position;

        if (onComplete != null)
        {
            onComplete.Invoke();
        }

        resultsout = true;
        podiumcamera.SetActive(true);
        Manager.Player.transform.position = winposition.position;
        Manager.Player.transform.rotation = winposition.localRotation;
        Manager.Player.animHandler.anim.SetFloat("Velocity", 0);
        Manager.Player.dropAlphabet();
        Manager.Player.hideweapon();
        ai1anim.gameObject.SetActive(true);
        ai2anim.GetComponent<NavMeshAgent>().enabled = false;
        ai2anim.transform.position = losepos1.position;
        ai2anim.transform.rotation = losepos1.localRotation;
        ai2anim.SetFloat("Velocity", 0);
        ai2anim.GetComponent<AIMover>().dropAlphabet();

        ai1anim.gameObject.SetActive(true);
        ai1anim.GetComponent<NavMeshAgent>().enabled = false;
        ai1anim.transform.position = losepos2.position;
        ai1anim.transform.rotation = losepos2.localRotation;
        ai1anim.SetFloat("Velocity", 0);
        ai1anim.GetComponent<AIMover>().dropAlphabet();

        ai1anim.SetBool("Picked", false);
        ai1anim.SetBool("Stun", false);
        ai1anim.SetTrigger("Lose");

        ai2anim.SetBool("Picked", false);
        ai2anim.SetBool("Stun", false);
        ai2anim.SetTrigger("Lose");

        Manager.Player.animHandler.anim.SetBool("Picked", false);
        Manager.Player.animHandler.anim.SetBool("Stun", false);
        confetti.SetActive(true);
    }

    private IEnumerator waitFail()
    {
        yield return new WaitForSeconds(7f);
        Manager.GameOver();
    }

}
