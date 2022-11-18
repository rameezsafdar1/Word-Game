using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

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
    public TextMeshProUGUI levelnumber;

    [Header("Colors")]
    public Material buildingsMat;
    public Camera maincamera;
    public Color[] colors;
    public Image hintbg;

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("Level");
        boundarymat.color = Color.white;
        Levels[currentLevel].SetActive(true);
        Player.lManager = Levels[currentLevel].GetComponent<LevelManager>();
        levelnumber.text = "Level " + (currentLevel + 1).ToString();
        navmesh.BuildNavMesh();

        buildingsMat.color = colors[currentLevel];
        maincamera.backgroundColor = colors[currentLevel];
        hintbg.color = colors[currentLevel];

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
        StartCoroutine(FailDisplay());
    }

    public void GameComplete()
    {
        currentLevel++;

        if (currentLevel >= Levels.Length)
        {
            currentLevel = 0;
        }

        PlayerPrefs.SetInt("Level", currentLevel);
        gameCompleted = true;
        StartCoroutine(StartConfetti());
    }

    private IEnumerator StartConfetti()
    {
        Player.animHandler.anim.SetTrigger("Win");
        confetti.SetActive(true);
        yield return new WaitForSeconds(4f);
        WinPanel.SetActive(true);
    }

    private IEnumerator FailDisplay()
    {
        yield return new WaitForSeconds(0.5f);
        FailPanel.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void rebuildNavigation()
    {
        navmesh.BuildNavMesh();
    }
}
