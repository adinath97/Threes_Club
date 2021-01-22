using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public void optionOne()
    {
        /*SPEEDSTER OUT FIRST*/
        //option #1 = Gunner
        if(PlayerRotator.speedsterOut && !PlayerRotator.gunnerOut && !PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.gunnerChosen = true;
        }

        //only plunderer available
        if(PlayerRotator.speedsterOut && PlayerRotator.gunnerOut && !PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.plundererChosen = true;
        }

        //only gunner available
        if (PlayerRotator.speedsterOut && !PlayerRotator.gunnerOut && PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.gunnerChosen = true;
        }

        /*PLUNDERER OUT FIRST*/
        //option #1 = SPEEDSTER
        if (!PlayerRotator.speedsterOut && !PlayerRotator.gunnerOut && PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.speedsterChosen = true;
        }

        //only SPEEDSTER available
        if (!PlayerRotator.speedsterOut && PlayerRotator.gunnerOut && PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.speedsterChosen = true;
        }

        //only gunner available
        if (PlayerRotator.speedsterOut && !PlayerRotator.gunnerOut && PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.gunnerChosen = true;
        }

        /*GUNNER OUT FIRST*/
        //option #1 = SPEEDSTER
        if (!PlayerRotator.speedsterOut && PlayerRotator.gunnerOut && !PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.speedsterChosen = true;
        }

        //only SPEEDSTER available
        if (!PlayerRotator.speedsterOut && PlayerRotator.gunnerOut && PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.speedsterChosen = true;
        }

        //only plunderer available
        if (PlayerRotator.speedsterOut && PlayerRotator.gunnerOut && !PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.plundererChosen = true;
        }
    }

    public void optionTwo()
    {
        /*SPEEDSTER OUT FIRST*/
        //option 2 == Plunderer
        if (PlayerRotator.speedsterOut && !PlayerRotator.gunnerOut && !PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.plundererChosen = true;
        }

        /*PLUNDERER OUT FIRST*/
        //option 2 == gunner
        if (!PlayerRotator.speedsterOut && !PlayerRotator.gunnerOut && PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.gunnerChosen = true;
        }

        /*GUNNER OUT FIRST*/
        //option 2 == Plunderer
        if (!PlayerRotator.speedsterOut && PlayerRotator.gunnerOut && !PlayerRotator.plundererOut)
        {
            PlayerRotator.playerOutOfRotation = false;
            PlayerRotator.plundererChosen = true;
        }
    }

    public void PlayBtn()
    {
        SceneManager.LoadScene("Level1");
    }

    public void NextBtn()
    {
        SceneManager.LoadScene("Background");
    }

    public void NextBtn2()
    {
        SceneManager.LoadScene("Controls");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
