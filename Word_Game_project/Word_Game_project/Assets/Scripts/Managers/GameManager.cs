using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject FailPanel;
    public CinemachineVirtualCamera followcam;

    public void GameOver()
    {
        followcam.Follow = null;
        followcam.LookAt = null;
        FailPanel.SetActive(true);
    }
}
