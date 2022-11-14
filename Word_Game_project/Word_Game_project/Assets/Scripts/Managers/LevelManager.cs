using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameManager Manager;
    public int Ai1Placements, Ai2Placements, PlayerPlacements;
    public GameObject ai1Cam, ai2Cam;
    public Animator ai1anim, ai2anim;
    private bool resultsout;

    public void Aip1()
    {
        Ai1Placements--;
        if (Ai1Placements <= 0)
        {
            if (!resultsout)
            {
                resultsout = true;
                ai1Cam.SetActive(true);
                ai1anim.SetTrigger("Win");
                StartCoroutine(waitFail());
            }
        }
    }

    public void Aip2()
    {
        Ai2Placements--;
        if (Ai2Placements <= 0)
        {
            if (!resultsout)
            {
                resultsout = true;
                ai2Cam.SetActive(true);
                ai2anim.SetTrigger("Win");
                StartCoroutine(waitFail());
            }
        }
    }

    public void playerplacement()
    {
        PlayerPlacements--;
        if (PlayerPlacements <= 0)
        {
            resultsout = true;
            Manager.GameComplete();
        }
    }

    private IEnumerator waitFail()
    {
        yield return new WaitForSeconds(4f);
        Manager.GameOver();
    }

}
