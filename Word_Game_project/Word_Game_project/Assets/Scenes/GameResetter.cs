using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResetter : MonoBehaviour
{
    private void Start()
    {
        int x = PlayerPrefs.GetInt("Reset");

        if (x < 1)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("Reset", 1);
            SceneManager.LoadScene(1);
        }

    }

}
