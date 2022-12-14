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
    public Color[] colors, buildingColors;
    public Image hintbg;
    private LevelManager lmanager;

    [Header("Fall Texts")]
    public GameObject[] falltexts;

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("Level");
        boundarymat.color = Color.white;
        Levels[currentLevel].SetActive(true);
        lmanager = Levels[currentLevel].GetComponent<LevelManager>();
        Player.lManager = Levels[currentLevel].GetComponent<LevelManager>();
        levelnumber.text = "Level " + (currentLevel + 1).ToString();
        navmesh.BuildNavMesh();

        if (currentLevel < buildingColors.Length)
        {
            buildingsMat.color = buildingColors[currentLevel];
        }
        else
        {
            buildingsMat.color = colors[currentLevel];
        }
        maincamera.backgroundColor = colors[currentLevel];
        hintbg.color = buildingColors[currentLevel];

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
        gameCompleted = true;
        lmanager.stopAtOnce();
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
        FailPanel.SetActive(false);
        WinPanel.SetActive(true);
    }

    private IEnumerator FailDisplay()
    {
        yield return new WaitForSeconds(0.5f);
        WinPanel.SetActive(false);
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

    public void randomfalltext()
    {
        if (!gameCompleted)
        {
            int x = Random.Range(0, falltexts.Length);
            falltexts[x].SetActive(true);
        }
    }

    public void falldeath()
    {
        lmanager.falldeath = true;
        lmanager.ai1anim.gameObject.SetActive(false);
        lmanager.ai2anim.gameObject.SetActive(false);
    }
}
