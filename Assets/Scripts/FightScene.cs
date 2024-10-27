using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightScene : MonoBehaviour
{

    // Defining player 1 and player2 scripts
    public Player1 player1Script;
    public Player2 player2Script;

    public TMP_Text timerText;
    public TMP_Text fullscreenText;

    public RoundWinCounters winCounterScript;

    public HealthBars HealthBarScript;


    // Constants
    private const int player1MaxHealth = 100;
    private const int player2MaxHealth = 100; 
    private const int PLAYER_1 = 1;
    private const int PLAYER_2 = 2;
    private const int roundWinLimit = 3; // How many round wins necessary to win the game
    private const int roundIntermissionTime = 360; // Number of frames paused in between rounds 

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
    private string gamePauseReason = "None";
    private int player1RoundWins = 0;
    private int player2RoundWins = 0;
    private int player1HealthUI = player1MaxHealth;
    private int player1OldHealthUI = player1MaxHealth;
    private int player2HealthUI = player2MaxHealth;
    private int player2OldHealthUI = player2MaxHealth;

    void startRound() {
        roundTimer = 99;
        fullscreenText.text = "";
        timerText.text = roundTimer.ToString();
        sceneFrameCounter = 0;
        logicFrameCounter = 0;
        roundInProgress = true;
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
        }
        else { // In between rounds
            winCounterScript.updateWinCounters(player1RoundWins, player2RoundWins);
            roundIntermissionCounter = roundIntermissionTime;
            gamePauseReason = "Round Intermission";

        }
        // StartCoroutine (RoundDelay());
        // startRound();
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
        startRound();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (roundInProgress) {
            // Only run these on logical frames (60 fps)
            if (sceneFrameCounter % sceneFPSOver60 == 0) {
                // Call p1 and p2 to take their actions that frame
                player1Script.doAction();
                player2Script.doAction();

                // If player died, set the flag to end the current round at end of frame

                // Decrease timer
                if (logicFrameCounter % 60 == 0) {
                    if (roundTimer > 0) {
                        roundTimer -= 1;
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
                    case "Round Intermission":
                        roundIntermissionCounter--;

                        if (180 < roundIntermissionCounter && roundIntermissionCounter <= 270) {
                            fullscreenText.text = lastWinner + " Wins Round"; 
                        }
                        if (120 < roundIntermissionCounter && roundIntermissionCounter <= 180) {
                            fullscreenText.text = "3"; 
                        }
                        if (60 < roundIntermissionCounter && roundIntermissionCounter <= 120) {
                            fullscreenText.text = "2"; 
                        }
                        if (0 < roundIntermissionCounter && roundIntermissionCounter <= 60) {
                            fullscreenText.text = "1"; 
                        }
                        if (roundIntermissionCounter <= 0) {
                            callRoundStartOrEnd = true;
                            roundNumber++;
                            fullscreenText.text = "";
                        }

                        break;
                    case "Game Over":
                        // Put code here for the rematch menu after the game has ended
                        // Remember to reset round win scores and remove the fullscreen text
                        break;
                    default:
                        print("ERROR: GAME PAUSED FOR UNKNOWN REASON");
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
        
        
        /*
        // Testing functions
        if (Input.GetKeyDown("n")){
            player1Script.currentAction = "NHA";
            player1Script.currentFrameCount = 0;
        }
        if (player1Script.currentAction == "NHA"){
            player1Script.neutralHighAttack();
            if (player1Script.currentAction == "NHA"){
                player1Script.currentFrameCount++;
            }
        }
        if (Input.GetKeyDown("f")){
            player1Script.currentAction = "FHA";
            player1Script.currentFrameCount = 0;
        }
        if (player1Script.currentAction == "FHA"){
            player1Script.forwardHighAttack();
            if (player1Script.currentAction == "FHA"){
                player1Script.currentFrameCount++;
            }
        }

        if (player1Script.currentAction == "actionable"){
            if (Input.GetKey("a")) {
                player1Script.setPlayerPosition("backward");
            }
            else if (Input.GetKey("d")) {
                player1Script.setPlayerPosition("forward");
            }
            else {
                player1Script.setPlayerPosition("neutral");
            }
        }

        if (player2Script.currentAction == "actionable"){
            if (Input.GetKey("right")) {
                player2Script.setPlayerPosition("backward");
            }
            else if (Input.GetKey("left")) {
                player2Script.setPlayerPosition("forward");
            }
            else {
                player2Script.setPlayerPosition("neutral");
            }
        }
        */
    }
}
