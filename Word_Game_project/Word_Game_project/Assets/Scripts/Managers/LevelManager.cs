using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameManager Manager;
    public int Ai1Placements, Ai2Placements, PlayerPlacements;
    public GameObject[] sprites;
    private int currentSprite;
    public Image endimage;

    private void Start()
    {
        endimage.sprite = sprites[sprites.Length - 1].GetComponent<SpriteRenderer>().sprite;
        endimage.SetNativeSize();
    }

    public void Aip1()
    {
        Ai1Placements--;
        if (Ai1Placements <= 0)
        {
            Manager.GameOver();
        }
    }

    public void Aip2()
    {
        Ai2Placements--;
        if (Ai2Placements <= 0)
        {
            Manager.GameOver();
        }
    }

    public void playerplacement()
    {
        PlayerPlacements--;
        if (PlayerPlacements <= 0)
        {
            Manager.GameComplete();
        }
    }

    public void uploadSprite()
    {
        if (currentSprite < sprites.Length)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].SetActive(false);
            }
            sprites[currentSprite].SetActive(true);
            currentSprite++;
        }
    }

}
