using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : MonoBehaviour
{

    // Defining player 1 and player2 scripts
    public Player1 player1Script;
    public Player2 player2Script;


    // Constants
    private const int player1MaxHealth = 100;
    private const int player2MaxHealth = 100; 

    // Changing values
    public int roundFrameCounter = 0; // The current number of frames elapsed since the round start
    public int roundTimer = 99; // Timer displayed in game
    private int player1HealthUI = 100;
    private int player2HealthUI = 100;

    void startRound() {
        roundTimer = 99;
        player1Script.reset();
        player2Script.reset();



    }

    //changeHealthBars(bool player, oldHealth, currentHealth);

        // player: false represents player 1, true represents player 2
        // use ints for clarity?

    
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Call p1 and p2 to take their actions that frame
        // p1TakeTurn(gameFrameCounter);
        // p2TakeTurn(gameFrameCounter);

        roundFrameCounter++;
        if (Input.GetKeyDown("t")){
            player1Script.currentAction = "NHA";
            player1Script.currentFrameCount = 0;

        }
        if (player1Script.currentAction == "NHA"){
            player1Script.neutralHighAttack();
            player1Script.currentFrameCount++;
        }


    }
}
