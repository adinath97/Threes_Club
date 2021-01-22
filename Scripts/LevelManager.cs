using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool gameOver;
    public static bool playerOut;
    public GameObject fadeScreenRed;
    //public GameObject fadeScreenGreen;

    // Start is called before the first frame update
    void Start()
    {
        fadeScreenRed.SetActive(false);
        gameOver = false;
        playerOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameStatus();
    }

    private void GameStatus()
    {
        if(gameOver && playerOut)
        {
            //fade to red
            //Debug.Log("SHOULD FADE NOW");
            fadeScreenRed.SetActive(true);
            fadeScreenRed.GetComponent<Animation>().Play("FadeAnim");
        }

        else if(gameOver && !playerOut)
        {
            //fade to green
        }
    }
}
