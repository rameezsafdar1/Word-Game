using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;

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

    public void Aip1()
    {
        Ai1Placements--;
        if (Ai1Placements <= 0)
        {
            if (!resultsout)
            {
                Fade.SetActive(true);

                Podium.transform.position = win1Pos.position;

                if (onComplete != null)
                {
                    onComplete.Invoke();
                }

                Manager.gameCompleted = true;
                resultsout = true;
                ai1anim.SetTrigger("Win");
                ai1anim.GetComponent<AIMover>().hideweapon();
                podiumcamera.SetActive(true);
                ai1anim.GetComponent<NavMeshAgent>().ResetPath();
                ai2anim.GetComponent<NavMeshAgent>().ResetPath();

                StartCoroutine(ww1());

                StartCoroutine(waitFail());
            }
        }
    }

    private IEnumerator ww1()
    {
        yield return new WaitForSeconds(1f);
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

        confetti.SetActive(true);
    }

    public void Aip2()
    {
        Ai2Placements--;
        if (Ai2Placements <= 0)
        {
            if (!resultsout)
            {
                Fade.SetActive(true);


                Podium.transform.position = win2Pos.position;

                if (onComplete != null)
                {
                    onComplete.Invoke();
                }

                Manager.gameCompleted = true;
                resultsout = true;
                ai2anim.SetTrigger("Win");
                ai2anim.GetComponent<AIMover>().hideweapon();
                podiumcamera.SetActive(true);

                ai1anim.GetComponent<NavMeshAgent>().ResetPath();
                ai2anim.GetComponent<NavMeshAgent>().ResetPath();

                StartCoroutine(ww2());

                StartCoroutine(waitFail());
            }
        }
    }

    private IEnumerator ww2()
    {
        yield return new WaitForSeconds(1f);
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

        confetti.SetActive(true);
    }

    public void playerplacement()
    {
        PlayerPlacements--;
        if (PlayerPlacements <= 0)
        {
            Fade.SetActive(true);


            Podium.transform.position = playerWinPos.position;


            if (onComplete != null)
            {
                onComplete.Invoke();
            }

            resultsout = true;
            StartCoroutine(ww3());

            podiumcamera.SetActive(true);
            Manager.GameComplete();
        }
    }

    private IEnumerator ww3()
    {
        yield return new WaitForSeconds(1f);

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

        confetti.SetActive(true);
    }

    private IEnumerator waitFail()
    {
        yield return new WaitForSeconds(4f);
        Manager.GameOver();
    }

}
