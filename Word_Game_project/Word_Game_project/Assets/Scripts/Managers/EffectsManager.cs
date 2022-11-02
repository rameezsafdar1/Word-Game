using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;
    public GameObject Player;
    public GameObject mainCamera;
    [Header("Instantiate Settings")]
    public Transform instParent;   

    private void Awake()
    {
        setTimeScale(1);
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    public void setTimeScale(float f)
    {
        Time.timeScale = f;
    }
}
