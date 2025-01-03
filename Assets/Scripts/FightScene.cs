using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class FightScene : MonoBehaviour
{

    // Defining player 1 and player2 scripts
    public Player1 player1Script;
    public Player2 player2Script;
    public GameObject P1Input;
    public GameObject p2Input;

    public TMP_Text timerText;
    public TMP_Text fullscreenText;

    public GameObject roundSign;
    public TMP_Text roundText;

    public GameObject countdownSign;
    public TMP_Text countdownText;


    public RoundWinCounters winCounterScript;

    public HealthBars HealthBarScript;

    public GameObject RematchMenu;

    // Constants
    private const int player1MaxHealth = 100;
    private const int player2MaxHealth = 100; 
    private const int PLAYER_1 = 1;
    private const int PLAYER_2 = 2;
    private const int roundWinLimit = 3; // How many round wins necessary to win the game
    private const int roundIntermissionTime = 330; // Number of frames paused in between rounds 
    private const int gameStartIntermissionTime = 180;

    private int roundIntermissionCounter = 0;

    private string lastWinner;

    public int roundNumber = 1;

    // Changing values
    public int sceneFrameCounter = 0; // The current number of game fps frames elapsed since round start
    public int sceneFPSOver60 = 1; // Self explanatory. Used to convert actual frames to logic frames
    public int logicFrameCounter = 0; // The current number of 60fps frames elapsed since the round start
    public int roundTimer = 99; // Timer displayed in game
    private bool roundInProgress = false; // Used to track whether or not to perform gameplay checks
    private bool gameInProgress = false; // Used to track whether or not to perform round checks
    private bool callRoundStartOrEnd = false; // Used to check for a round end. Probably could be merged with
                                       // above but I'm lazy and tired. 
    private bool callGameStartCountdown = false;
    private string gamePauseReason = "None";
    private int player1RoundWins = 0;
    private int player2RoundWins = 0;
    private int player1HealthUI = player1MaxHealth;
    private int player1OldHealthUI = player1MaxHealth;
    private int player2HealthUI = player2MaxHealth;
    private int player2OldHealthUI = player2MaxHealth;
    private bool p1WasHitThisFrame = false;
    private int p1Damage = 0;
    private bool p1WasBlocked = false;
    private int p1StunFrames = 0;
    private bool p2WasHitThisFrame = false;
    private int p2Damage = 0;
    private bool p2WasBlocked = false;
    private int p2StunFrames = 0;

    public int frozenFrames = 0;

    public AudioSource fightMusic;
    public AudioSource countDownSound;
    void startRound() {
        print("round started");
        roundTimer = 99;
        fullscreenText.text = "";
        timerText.text = roundTimer.ToString();
        sceneFrameCounter = 0;
        logicFrameCounter = 0;
        roundInProgress = true; //testing out if we can get the counter at the beginning
        gameInProgress = true;
        callRoundStartOrEnd = false;
        player1HealthUI = player1MaxHealth;
        player1OldHealthUI = player1MaxHealth;
        player2HealthUI = player2MaxHealth;
        player2OldHealthUI = player2MaxHealth;
        player1Script.reset(player1MaxHealth);
        player2Script.reset(player2MaxHealth);
        winCounterScript.updateWinCounters(player1RoundWins, player2RoundWins);
        HealthBarScript.setHealthBar(PLAYER_1, player1HealthUI); //is set to max
        HealthBarScript.setHealthBar(PLAYER_2, player2HealthUI); //is set to max

        if (player1RoundWins == 0 && player2RoundWins == 0 && callGameStartCountdown == true){
            roundInProgress = false;
            gameStartCountdown();
        }
        else{
            roundInProgress = true;
        }
    }

    void gameStartCountdown() {
        roundIntermissionCounter = gameStartIntermissionTime;
        gamePauseReason = "Game Start";
    }

    void endRound() {
        roundInProgress = false;
        switch (player1HealthUI) {
            case int i when i < player2HealthUI:
                player2RoundWins += 1;
                lastWinner = "Player 2";
                break;
            case int i when i == player2HealthUI:
                print("draw");
                break;
            case int i when i > player2HealthUI:
                player1RoundWins += 1;
                lastWinner = "Player 1";
                break;
            default:
                print("DEFAULT IS RUNNING IN ENDROUND");
                break;
        }
        winCounterScript.updateWinCounters(player1RoundWins, player2RoundWins);

        if (player1RoundWins >= roundWinLimit) {
            print("player1 wins!");
            fullscreenText.text = "Player 1 Wins!";
            /* Old code
            StartCoroutine (RoundDelay()); //2 sec delay
            player1RoundWins = 0;
            player2RoundWins = 0;
            */
            // Maybe make these an "endGame()" function later
            roundInProgress = false;
            gameInProgress = false;
            sceneFrameCounter = 0;
            logicFrameCounter = 0;
            gamePauseReason = "Game Over";
            RematchMenu.SetActive(true);
        }
        else if (player2RoundWins >= roundWinLimit) {
            print("player2 wins!");
            fullscreenText.text = "Player 2 Wins!";
            /* Old code
            StartCoroutine (RoundDelay()); //2 sec delay
            player1RoundWins = 0;
            player2RoundWins = 0;
            */
            roundInProgress = false;
            gameInProgress = false;
            sceneFrameCounter = 0;
            logicFrameCounter = 0;
            gamePauseReason = "Game Over";
            RematchMenu.SetActive(true);
        }
        else { // In between rounds
            roundIntermissionCounter = roundIntermissionTime;
            gamePauseReason = "Round Intermission";

        }
        // StartCoroutine (RoundDelay());
        // startRound();
    }

    public void Reset(){
        player1RoundWins = 0;
        player2RoundWins = 0;
        callGameStartCountdown = true;
        RematchMenu.SetActive(false);
        countDownSound.Play();
        startRound();
    }

    public void loadMainMenu(){
        RematchMenu.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void damageBroadcast(int playerNum, int damage, bool wasBlocked, int stunFrames) {
        if (playerNum == 1) {
            p2WasHitThisFrame = true;
            p2Damage = damage;
            p2WasBlocked = wasBlocked;
            p2StunFrames = stunFrames;
        }
        else {
            p1WasHitThisFrame = true;
            p1Damage = damage;
            p1WasBlocked = wasBlocked;
            p1StunFrames = stunFrames;
        }
    }

    private void calculateDamage() {
        if (p1WasHitThisFrame) {
            player1Script.getHit(p1Damage, p1WasBlocked, p1StunFrames);
            p1WasHitThisFrame = false;
            p1Damage = 0;
            p1WasBlocked = false;
            p1StunFrames = 0;
        }
        if (p2WasHitThisFrame) {
            player2Script.getHit(p2Damage, p2WasBlocked, p2StunFrames);
            p2WasHitThisFrame = false;
            p2Damage = 0;
            p2WasBlocked = false;
            p2StunFrames = 0;
        }
    }

    //create a UI and tie in function
    public void changeHealthBars(int playerNum, int oldHealth, int currentHealth){
        if (playerNum == 1){
            player1HealthUI = currentHealth;
            HealthBarScript.setHealthBar(playerNum, player1HealthUI);
            player1OldHealthUI = oldHealth;

            // DONT UPDATE THE HEALTH BARS IF THE END ROUND FLAG IS TRUE!!!!
        }
        else{
            player2HealthUI = currentHealth;
            HealthBarScript.setHealthBar(playerNum, player2HealthUI);
            player2OldHealthUI = oldHealth;

        }
        
    }

    // Depracate 
    public IEnumerator RoundDelay(){
        player1Script.anim.SetInteger("Position", 0);
        player1Script.anim.SetBool("Hit", false);
        player1Script.anim.SetBool("Block", false);
        player2Script.anim.SetInteger("Position", 0);
        player2Script.anim.SetBool("Hit", false);
        player2Script.anim.SetBool("Block", false);
        yield return new WaitForSeconds(3);
    }
    //changeHealthBars(bool player, oldHealth, currentHealth);


    
    /*
    public int startFrame(int current, string attack){
        if (attack == "neutral mid"){
            return current + 10;
        }
        else if (attack == "forward mid"){
            return current + 16;
        }
        else if (attack == "neutral high"){
            return current + 10;
        }
        else if (attack == "forward high"){
            return current + 12;
        }
        else if (attack == "neutral throw"){
            return current + 15;
        } 
        else if (attack == "forward throw"){
            return current + 15;
        } 
        else{
            return 0
        }
    }
    */

    void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        sceneFPSOver60 = Application.targetFrameRate / 60;
        //P1Input.SetActive(true);
        //P2Input.SetActive(true);
        Reset();
        print("started");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Find and connect controllers to players
        /*
        if (player1Script.controller == null || player2Script.controller == null) {
            var controllers = FindObjectsByType<UserInput>;
            for (int i = 0; i < controllers.Length; i++) {
                if (controllers[i].controllerIndex == player1Script.playerIndex) {
                    player1Script.controller = controllers[i];
                }
                else if (controllers[i].controllerIndex == player2Script.playerIndex) {
                    player2Script.controller = controllers[i];
                }
            }
        }
        */

        if (roundInProgress) {
            // Only run these on logical frames (60 fps)
            if (sceneFrameCounter % sceneFPSOver60 == 0) {
                // Call p1 and p2 to take their actions that frame
                if (frozenFrames > 0) {
                    player1Script.doAction(true);
                    player2Script.doAction(true);
                    player1Script.anim.speed = 0;
                    player2Script.anim.speed = 0;
                    frozenFrames--;
                    print("frozen");
                }
                else {
                    player1Script.anim.speed = 1;
                    player2Script.anim.speed = 1;
                    player1Script.doAction(false);
                    player2Script.doAction(false);
                    calculateDamage();
                }

                // If player died, set the flag to end the current round at end of frame

                // Decrease timer
                if (logicFrameCounter % 60 == 0) {
                    if (roundTimer > 0) {
                        roundTimer -= 1;

                        timerText.text = Mathf.Max(0, roundTimer).ToString();

                    }
                }

                if (player1HealthUI <= 0 || player2HealthUI <= 0 || roundTimer <= 0) {
                    endRound();
                }
                // If a player has died, this flag will be false
                // if (roundInProgress == false) {
                //     callRoundStartOrEnd = true;
                // }

                // Increment logic frame counter by 1
                logicFrameCounter++;
            }

            // Do these every scene frame (chosen fps)
            sceneFrameCounter++;
            
            timerText.text = Mathf.Max(0, roundTimer).ToString();
            
            // End the round if necessary
            // if (callRoundStartOrEnd == true) {
            //     callRoundStartOrEnd = false;
            //     endRound();
            // }

        }
        else {
            print("game paused");
            // Only run these on logical frames (60 fps)
            if (sceneFrameCounter % sceneFPSOver60 == 0) {
                switch (gamePauseReason) {
                    case "Game Start":
                        if(roundIntermissionCounter == 180)
                        {
                            countdownSign.SetActive(true);
                        }
                        roundIntermissionCounter--;
                        if (120 < roundIntermissionCounter && roundIntermissionCounter <= 180 &&
                            !String.Equals(countdownText.text,"I")) {

                            //fullscreenText.fontSize = 40;
                            //fullscreenText.text = "I"; 
                            countdownText.text = "I";
                            countdownSign.GetComponent<Animator>().SetTrigger("Countdown");
                        }
                        if (60 < roundIntermissionCounter && roundIntermissionCounter <= 120 &&
                            !String.Equals(countdownText.text,"Declare A")) {
                            //fullscreenText.fontSize = 50;
                            //fullscreenText.text = "DECLARE"; 
                            countdownText.text = "Declare A";
                            countdownSign.GetComponent<Animator>().SetTrigger("Countdown");
                        }
                        if (0 < roundIntermissionCounter && roundIntermissionCounter <= 60 && 
                            !String.Equals(countdownText.text,"Thumb War")) {

                            //fullscreenText.fontSize = 80;
                            //fullscreenText.text = "THUMB WAR"; 
                            countdownText.text = "Thumb War";
                            countdownSign.GetComponent<Animator>().SetTrigger("Countdown");
                        }
                        if (roundIntermissionCounter <= 0) {
                            callRoundStartOrEnd = true;
                            callGameStartCountdown = false;
                            fullscreenText.text = "";
                            fullscreenText.fontSize = 40;
                            roundInProgress = true;
                            countdownSign.SetActive(false);
                        }

                        break;

                    case "Round Intermission":

                        roundIntermissionCounter--;
                        //countdownSign.SetActive(true);
                        if (roundIntermissionCounter == 329) {
                            countdownSign.SetActive(true);
                            countdownText.text = lastWinner + " Wins";
                            countdownSign.GetComponent<Animator>().SetTrigger("Countdown");

                            // roundSign.SetActive(true);
                            // roundText.text = lastWinner + " Wins";
                        }
                        if(roundIntermissionCounter == 250) {
                            countDownSound.Play();
                        }
                        
                        else if (0 < roundIntermissionCounter && roundIntermissionCounter <= 240) {    
                            countdownSign.SetActive(true);
                            
                            if (180 < roundIntermissionCounter && roundIntermissionCounter <= 240 &&
                                !String.Equals(countdownText.text,"3")) {

                                countdownText.text = "3"; 
                                countdownSign.GetComponent<Animator>().SetTrigger("Countdown");
                                
                            }
                            else if (120 < roundIntermissionCounter && roundIntermissionCounter <= 180 &&
                                !String.Equals(countdownText.text,"2")) {
                                countdownText.text = "2"; 
                                countdownSign.GetComponent<Animator>().SetTrigger("Countdown");
                            }
                            else if (60 < roundIntermissionCounter && roundIntermissionCounter <= 120 &&
                                !String.Equals(countdownText.text,"1")) {
                                countdownText.text = "1"; 
                                countdownSign.GetComponent<Animator>().SetTrigger("Countdown");
                            }
                            else if (0 < roundIntermissionCounter && roundIntermissionCounter <= 60 &&
                                !String.Equals(countdownText.text,"Fight!")) {
                                countdownText.text = "Fight!"; 
                                countdownSign.GetComponent<Animator>().SetTrigger("Countdown");
                            }
                        }
                        

                        if (roundIntermissionCounter <= 0) {
                            callRoundStartOrEnd = true;
                            roundNumber++;
                            fullscreenText.text = "";
                            countdownSign.SetActive(false);
                        }

                        break;
                    case "Game Over":
                        // Put code here for the rematch menu after the game has ended
                        // Remember to reset round win scores and remove the fullscreen text\
                        if (player1Script.pressingAnyAttacks()&&player2Script.pressingAnyAttacks()) {
                            print("resetting");
                            Reset();
                        }
                        break;
                    default:
                        print("ERROR: GAME PAUSED FOR UNKNOWN REASON: " + gamePauseReason);
                        break;
                }
            }
            sceneFrameCounter++;
            // Start the round if necessary
            if (callRoundStartOrEnd == true) {
                callRoundStartOrEnd = false;
                startRound();
            }
        }
        
    }
}
