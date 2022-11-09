using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameObject FailPanel;
    public CinemachineVirtualCamera followcam;
    public Material boundarymat;
    public GameObject[] Levels;
    public int currentLevel;
    public NavMeshSurface navmesh;

    private void Start()
    {
        boundarymat.color = Color.white;
        Levels[currentLevel].SetActive(true);
        navmesh.BuildNavMesh();
    }

    public void GameOver()
    {
        followcam.Follow = null;
        followcam.LookAt = null;
        FailPanel.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
