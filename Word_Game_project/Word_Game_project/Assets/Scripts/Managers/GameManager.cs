using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public pickHandler Player;
    public GameObject FailPanel, WinPanel, confetti;
    public CinemachineVirtualCamera followcam;
    public Material boundarymat;
    public GameObject[] Levels;
    public int currentLevel;
    public NavMeshSurface navmesh;
    public bool gameStarted, gameCompleted;
    public GameObject startText;

    private void Start()
    {
        //currentLevel = PlayerPrefs.GetInt("Level");
        boundarymat.color = Color.white;
        Levels[currentLevel].SetActive(true);
        Player.lManager = Levels[currentLevel].GetComponent<LevelManager>();
        navmesh.BuildNavMesh();
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0)
            {
                gameStarted = true;
                startText.SetActive(false);
            }
        }        
    }

    public void GameOver()
    {
        followcam.Follow = null;
        followcam.LookAt = null;
        FailPanel.SetActive(true);
    }

    public void GameComplete()
    {
        currentLevel++;

        if (currentLevel >= Levels.Length)
        {
            PlayerPrefs.SetInt("Level", currentLevel);
        }

        gameCompleted = true;
        StartCoroutine(StartConfetti());
    }

    private IEnumerator StartConfetti()
    {
        Player.animHandler.anim.SetTrigger("Win");
        confetti.SetActive(true);
        yield return new WaitForSeconds(6f);
        WinPanel.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
