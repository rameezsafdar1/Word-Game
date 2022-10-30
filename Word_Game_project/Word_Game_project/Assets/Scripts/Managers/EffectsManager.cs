using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;
    public GameObject Player;
    public CinemachineBrain brain;
    public Camera mainCamera;
    public CinemachineVirtualCamera lookCamera;
    private int currentPooledAudio;
    private float tempPitchTime;
    [Header("Instantiate Settings")]
    public Transform instParent;    
    public GameObject upgradesMenu;
    public int scene;
    [HideInInspector]
    public int hostagesFreed;

    public AudioSource[] bulletSound, gemSound;
    private int currentBulletSound;

    private void Awake()
    {
        setTimeScale(1);
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    private void Update()
    {
        if (currentPooledAudio > 0)
        {
            tempPitchTime += Time.deltaTime;

            if (tempPitchTime >= 0.5f)
            {
                tempPitchTime = 0;
                currentPooledAudio = 0;
            }
        }
    }


    public void changeFov(float value)
    {
        lookCamera.m_Lens.FieldOfView = Mathf.Lerp(lookCamera.m_Lens.FieldOfView, value, 1.5f * Time.deltaTime);
    }

    public void playPickupSound()
    {
        tempPitchTime = 0;
        gemSound[currentPooledAudio].Play();
        currentPooledAudio++;

        if (currentPooledAudio >= gemSound.Length)
        {
            currentPooledAudio = gemSound.Length - 1;
        }
    }

    public string currencyShortener(float currency)
    {
        string converted = "";

        if (currency >= 1000)
        {
            converted = (currency / 1000).ToString() + "K";
        }
        else
        {
            converted = currency.ToString();
        }
        return converted;
    }

    public void setTimeScale(float f)
    {
        Time.timeScale = f;
    }

    public void changeCamTransTime(float time)
    {
        brain.m_DefaultBlend.m_Time = time;
    }

    public void callBulletSound()
    {
        bulletSound[currentBulletSound].Play();
        currentBulletSound++;

        if (currentBulletSound >= bulletSound.Length)
        {
            currentBulletSound = 0;
        }
    }

}
