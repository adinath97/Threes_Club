using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerRotator : MonoBehaviour
{
    public GameObject[] playerTeam = new GameObject[3];
    private GameObject currentPlayer;
    private GameObject previousPlayer;
    private GameObject nextPlayer;
    public GameObject plundererEnergy;
    public GameObject gunnerEnergy;
    public GameObject speedsterEnergy;
    public GameObject PlayerDownBackground;
    public GameObject playerOptionOne;
    public GameObject playerOptionTwo;
    //public GameObject playerOptionThree;
    public GameObject whoIsOutMessage;
    public GameObject whoIsOutTitle;
    public GameObject playerOptionBtn1;
    public GameObject playerOptionBtn2;
    //public GameObject playerOptionBtn3;

    private float speedsterEnergyLevel;
    private float gunnerEnergyLevel;
    private float plundererEnergyLevel;
    public static int teamPosition;
    public static bool changed;
    private float decrementRateSpeedSter;
    private float decrementRatePlunderer;
    private float decrementRateGunner;
    public static bool speedsterOut;
    public static bool plundererOut;
    public static bool gunnerOut;
    public static bool playerOutOfRotation;
    private bool coroutineRunning;
    private bool nowKeepGoing;
    public static bool gameCanContinue;
    public static bool speedsterChosen;
    public static bool plundererChosen;
    public static bool gunnerChosen;
    private int speedsterCounter;
    private int gunnerCounter;
    private int plundererCounter;

    // Start is called before the first frame update
    void Start()
    {
        SetUpGame();
    }

    // Update is called once per frame
    void Update()
    {
        //only rotate plays as long as game is not over
        if (!LevelManager.gameOver && !playerOutOfRotation)
        {
            TrackAvailableCharacters();
        }

        if(playerOutOfRotation && !coroutineRunning)
        {
            coroutineRunning = true;
            //stop all proceedings
            StartCoroutine(WaitAndProgressRoutine());
        }

        if(nowKeepGoing)
        {
            //stop all proceedings, tell player who's out, and to select a replacement. continue game once new current player is selected
            DecideReplacementPlayer();
        }

        if(!playerOutOfRotation)
        {
            UpdateAvailableCharacters();
        }

        if(gunnerChosen)  {
            Debug.Log("GUNNER CHOSEN");
            previousPlayer = currentPlayer;
            if(previousPlayer.name == "Speedster")
            {
                speedsterOut = true;
            }
            else if(previousPlayer.name == "Plunderer")
            {
                plundererOut = true;
            }
            previousPlayer.SetActive(false);
            teamPosition = 2;
            currentPlayer = playerTeam[2];
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            gunnerChosen = false;
            nowKeepGoing = false;
            changed = true;
            Debug.Log("PLAYER OUT OF ROTATION STILL!");
            PlayerDownBackground.SetActive(false);
            whoIsOutTitle.SetActive(false);
            whoIsOutMessage.SetActive(false);
            playerOptionBtn1.SetActive(false);
            playerOptionOne.SetActive(false);
            playerOptionBtn2.SetActive(false);
            playerOptionTwo.SetActive(false);
            //playerOptionBtn3.SetActive(false);
            //playerOptionThree.SetActive(false);
        }

        if (speedsterChosen)
        {
            Debug.Log("SPEEDSTER CHOSEN");
            previousPlayer = currentPlayer;
            if (previousPlayer.name == "Gunner")
            {
                gunnerOut = true;
            }
            else if (previousPlayer.name == "Plunderer")
            {
                plundererOut = true;
            }
            previousPlayer.SetActive(false);
            teamPosition = 0;
            currentPlayer = playerTeam[0];
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            speedsterChosen = false;
            nowKeepGoing = false;
            changed = true;
            //Debug.Log("PLAYER OUT OF ROTATION STILL!");
            PlayerDownBackground.SetActive(false);
            whoIsOutTitle.SetActive(false);
            whoIsOutMessage.SetActive(false);
            playerOptionBtn1.SetActive(false);
            playerOptionOne.SetActive(false);
            playerOptionBtn2.SetActive(false);
            playerOptionTwo.SetActive(false);
            //playerOptionBtn3.SetActive(false);
            //playerOptionThree.SetActive(false);
        }

        if (plundererChosen)
        {
            Debug.Log("PLUNDERER CHOSEN");
            previousPlayer = currentPlayer;
            if (previousPlayer.name == "Speedster")
            {
                speedsterOut = true;
            }
            else if (previousPlayer.name == "Gunner")
            {
                gunnerOut = true;
            }
            previousPlayer.SetActive(false);
            teamPosition = 1;
            currentPlayer = playerTeam[1];
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            plundererChosen = false;
            nowKeepGoing = false;
            changed = true;
            Debug.Log("PLAYER OUT OF ROTATION STILL!");
            PlayerDownBackground.SetActive(false);
            whoIsOutTitle.SetActive(false);
            whoIsOutMessage.SetActive(false);
            playerOptionBtn1.SetActive(false);
            playerOptionOne.SetActive(false);
            playerOptionBtn2.SetActive(false);
            playerOptionTwo.SetActive(false);
            //playerOptionBtn3.SetActive(false);
            //playerOptionThree.SetActive(false);
        }

        if(plundererOut && gunnerOut && speedsterOut)
        {
            LevelManager.gameOver = true;
            LevelManager.playerOut = true;
            StartCoroutine(WaitAndLoadGameOverRoutine());
        }

        if(LevelManager.gameOver)
        {
            StartCoroutine(WaitAndLoadGameOverRoutine());
        }
    }

    IEnumerator WaitAndLoadGameOverRoutine()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("GameOverScene");
    }

    private void DecideReplacementPlayer()
    {
        //now replacement will be chosen, so stop running all the checks and wait for player input
        //Debug.Log("DECIDE REPLACEMENT PLAYER");
        //Debug.Log(nowKeepGoing);
        nowKeepGoing = false;
        //coroutineRunning = false;

        //speedster out first
        if (speedsterOut && !gunnerOut && !plundererOut)
        {
            whoIsOutMessage.GetComponent<Text>().text = "Speedster is out of energy. Your remaining options are:";
            playerOptionOne.GetComponent<Text>().text = "Gunner";
            playerOptionTwo.GetComponent<Text>().text = "Plunderer";
            //playerOptionThree.GetComponent<Text>().text = "";
        }

        if(speedsterOut && gunnerOut && !plundererOut)
        {
            //Debug.Log("gunner setting now");
            whoIsOutMessage.GetComponent<Text>().text = "Speedster and Gunner are out of energy. Your remaining option is:";
            playerOptionOne.GetComponent<Text>().text = "Plunderer";
            playerOptionTwo.GetComponent<Text>().text = "";
            //playerOptionThree.GetComponent<Text>().text = "";
        }

        if(speedsterOut && plundererOut && !gunnerOut)
        {
            //Debug.Log("gunner setting NOW");
            whoIsOutMessage.GetComponent<Text>().text = "Speedster and Plunderer are out of energy. Your remaining option is:";
            playerOptionOne.GetComponent<Text>().text = "Gunner";
            playerOptionTwo.GetComponent<Text>().text = "";
            //playerOptionThree.GetComponent<Text>().text = "";
        }

        //plunderer out first
        if (!speedsterOut && !gunnerOut && plundererOut)
        {
            whoIsOutMessage.GetComponent<Text>().text = "Plunderer is out of energy. Your remaining options are:";
            playerOptionOne.GetComponent<Text>().text = "Speedster";
            playerOptionTwo.GetComponent<Text>().text = "Gunner";
            //playerOptionThree.GetComponent<Text>().text = "";
        }

        if (!speedsterOut && gunnerOut && plundererOut)
        {
            //Debug.Log("gunner setting now");
            whoIsOutMessage.GetComponent<Text>().text = "Plunderer and Gunner are out of energy. Your remaining option is:";
            playerOptionOne.GetComponent<Text>().text = "Speedster";
            playerOptionTwo.GetComponent<Text>().text = "";
            //playerOptionThree.GetComponent<Text>().text = "";
        }

        if (speedsterOut && plundererOut && !gunnerOut)
        {
            //Debug.Log("gunner setting NOW");
            whoIsOutMessage.GetComponent<Text>().text = "Speedster and Plunderer are out of energy. Your remaining option is:";
            playerOptionOne.GetComponent<Text>().text = "Gunner";
            playerOptionTwo.GetComponent<Text>().text = "";
            //playerOptionThree.GetComponent<Text>().text = "";
        }

        //gunner out first
        if (!speedsterOut && gunnerOut && !plundererOut)
        {
            whoIsOutMessage.GetComponent<Text>().text = "Gunner is out of energy. Your remaining options are:";
            playerOptionOne.GetComponent<Text>().text = "Speedster";
            playerOptionTwo.GetComponent<Text>().text = "Plunderer";
            //playerOptionThree.GetComponent<Text>().text = "";
        }

        if (!speedsterOut && gunnerOut && plundererOut)
        {
            //Debug.Log("gunner setting now");
            whoIsOutMessage.GetComponent<Text>().text = "Plunderer and Gunner are out of energy. Your remaining option is:";
            playerOptionOne.GetComponent<Text>().text = "Speedster";
            playerOptionTwo.GetComponent<Text>().text = "";
            //.GetComponent<Text>().text = "";
        }

        if (speedsterOut && gunnerOut && !plundererOut)
        {
            //Debug.Log("gunner setting NOW");
            whoIsOutMessage.GetComponent<Text>().text = "Speedster and Gunner are out of energy. Your remaining option is:";
            playerOptionOne.GetComponent<Text>().text = "Plunderer";
            playerOptionTwo.GetComponent<Text>().text = "";
            //playerOptionThree.GetComponent<Text>().text = "";
        }

        PlayerDownBackground.SetActive(true);
        whoIsOutTitle.SetActive(true);
        whoIsOutMessage.SetActive(true);
        playerOptionBtn1.SetActive(true);
        playerOptionOne.SetActive(true);
        playerOptionBtn2.SetActive(true);
        playerOptionTwo.SetActive(true);
        //playerOptionBtn3.SetActive(true);
        //playerOptionThree.SetActive(true);
    }

    IEnumerator WaitAndProgressRoutine()
    {
        yield return new WaitForSeconds(1f);
        nowKeepGoing = true;
        //Debug.Log("NOW KEEP GOING!");
    }

    private void UpdateAvailableCharacters()
    {
        if (currentPlayer.tag == "Speedster" && !speedsterOut)
        {
            speedsterEnergyLevel -= Time.deltaTime * decrementRateSpeedSter;
            int speedsterRoundedEnergy = Mathf.RoundToInt(speedsterEnergyLevel);
            gunnerEnergy.GetComponent<Text>().color = Color.black;
            plundererEnergy.GetComponent<Text>().color = Color.black;
            speedsterEnergy.GetComponent<Text>().color = Color.red;
            speedsterEnergy.GetComponent<Text>().text = "Speedster: " + speedsterRoundedEnergy;
        }
        else if (currentPlayer.tag == "Plunderer" && !plundererOut)
        {
            plundererEnergyLevel -= Time.deltaTime * decrementRatePlunderer;
            int plundererRoundedEnergy = Mathf.RoundToInt(plundererEnergyLevel);
            gunnerEnergy.GetComponent<Text>().color = Color.black;
            plundererEnergy.GetComponent<Text>().color = Color.red;
            speedsterEnergy.GetComponent<Text>().color = Color.black;
            plundererEnergy.GetComponent<Text>().text = "Plunderer: " + plundererRoundedEnergy;
        }
        else if (currentPlayer.tag == "Gunner" && !gunnerOut)
        {
            gunnerEnergyLevel -= Time.deltaTime * decrementRateGunner;
            int gunnerRoundedEnergy = Mathf.RoundToInt(gunnerEnergyLevel);
            gunnerEnergy.GetComponent<Text>().color = Color.red;
            plundererEnergy.GetComponent<Text>().color = Color.black;
            speedsterEnergy.GetComponent<Text>().color = Color.black;
            gunnerEnergy.GetComponent<Text>().text = "Gunner: " + gunnerRoundedEnergy;
        }
    }

    private void TrackAvailableCharacters()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && Player.grounded)
        {
            UpdateCurrentPlayer();
        }
        //check if a player is out of energy
        if (speedsterEnergyLevel <= 0 && speedsterCounter == 0)
        {
            speedsterOut = true;
            coroutineRunning = false;
            playerOutOfRotation = true;
            speedsterCounter++;
        }
        if (plundererEnergyLevel <= 0 && plundererCounter == 0)
        {
            plundererOut = true;
            coroutineRunning = false;
            playerOutOfRotation = true;
            plundererCounter++;
        }
        if (gunnerEnergyLevel <= 0 && gunnerCounter == 0)
        {
            gunnerOut = true;
            coroutineRunning = false;
            //Debug.Log("GunnerOut is " + gunnerOut);
            playerOutOfRotation = true;
            gunnerCounter++;
        }
    }

    private void UpdateCurrentPlayer()
    {
        if(!speedsterOut && !gunnerOut && !plundererOut)
        {
            teamPosition++;
            if (teamPosition == 3)
            {
                teamPosition = 0;
            }
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }

        //speedster first out
        if(speedsterOut)
        {
            teamPosition++;
            if (teamPosition == 3)
            {
                teamPosition = 1;
            }
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }

        if(speedsterOut && gunnerOut)
        {
            teamPosition++;
            teamPosition = 1;
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }

        if (speedsterOut && plundererOut)
        {
            teamPosition++;
            teamPosition = 2;
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }

        //plunderer first out
        if (plundererOut)
        {
            //teamPosition++;
            Debug.Log("CHANGE DESIRED");
            if(teamPosition == 0)
            {
                teamPosition = 2;
            }
            else if(teamPosition == 2)
            {
                teamPosition = 0;
            }
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }

        if (speedsterOut && plundererOut)
        {
            teamPosition++;
            teamPosition = 2;
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }

        if (gunnerOut && plundererOut)
        {
            teamPosition++;
            teamPosition = 0;
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }

        //gunner first out
        if (gunnerOut)
        {
            //teamPosition++;
            //Debug.Log("CHANGE DESIRED");
            if (teamPosition == 0)
            {
                teamPosition = 1;
            }
            else if (teamPosition == 1)
            {
                teamPosition = 0;
            }
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }

        if (speedsterOut && gunnerOut)
        {
            teamPosition++;
            teamPosition = 1;
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }

        if (gunnerOut && plundererOut)
        {
            teamPosition++;
            teamPosition = 0;
            //move currentPlayer to previousPlayer and inactivate it
            previousPlayer = currentPlayer;
            previousPlayer.SetActive(false);
            //find the next player, move to currentPlayer, activate at position of previousPlayer
            nextPlayer = playerTeam[teamPosition];
            currentPlayer = nextPlayer;
            currentPlayer.transform.position = previousPlayer.transform.position;
            currentPlayer.SetActive(true);
            changed = true;
        }
    }

    private void SetUpGame()
    {
        PlayerDownBackground.SetActive(false);
        PlayerDownBackground.SetActive(false);
        whoIsOutTitle.SetActive(false);
        whoIsOutMessage.SetActive(false);
        playerOptionOne.SetActive(false);
        playerOptionTwo.SetActive(false);
        //playerOptionThree.SetActive(false);
        playerOptionBtn1.SetActive(false);
        playerOptionBtn2.SetActive(false);
        //playerOptionBtn3.SetActive(false);
        playerOutOfRotation = false;
        speedsterOut = false;
        plundererOut = false;
        gunnerOut = false;
        coroutineRunning = false;
        nowKeepGoing = false;
        decrementRateSpeedSter = Random.Range(15, 21);
        decrementRatePlunderer = Random.Range(15, 21);
        decrementRateGunner = Random.Range(15, 21);
        teamPosition = 0;
        speedsterEnergyLevel = 100f;
        gunnerEnergyLevel = 100f;
        plundererEnergyLevel = 100f;
        speedsterEnergy.GetComponent<Text>().text = "Speedster: " + speedsterEnergyLevel;
        plundererEnergy.GetComponent<Text>().text = "Plunderer: " + plundererEnergyLevel;
        gunnerEnergy.GetComponent<Text>().text = "Gunner: " + gunnerEnergyLevel;

        currentPlayer = playerTeam[teamPosition];
        playerTeam[1].SetActive(false);
        playerTeam[2].SetActive(false);
    }
}
