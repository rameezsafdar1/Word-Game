using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject FailPanel;
    public CinemachineVirtualCamera followcam;
    public Material boundarymat;

    private void Start()
    {
        boundarymat.color = Color.white;
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
