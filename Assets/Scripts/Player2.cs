using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player2 : MonoBehaviour
{
    // Methods and variables need to be in class   
    public Player1 player1Script;

    public FightScene fightSceneScript;

 
    public int maxHealth; // Deprecate
    public int currentHealth;
    public int oldHealth; 
    public List<int> p2Hurtbox = new List<int>{2};
    public string p2Stance; // Position. ("backward", "neutral", "forward")
    public List<bool> inputHistory = new List<bool>(); 
    public string currentAction = "actionable"; 
    public int currentFrameCount = 0;

    public List<int> revertHurtbox() {
        switch (p2Stance) {
            case "backward":
                return new List<int> {1};
                break;
            case "neutral":
                return new List<int> {2};
                break;
            case "forward":
                return new List<int> {3};
                break;
            default:
                print("Revert Hurtbox Failed");
                return new List<int> {};
                break;
        }
    }

    public bool isBlocking() {
        if (currentAction == "acitionable" && p2Stance == "neutral") {
            return true;
        }
        return false;
    }

    //Helper function for when we set the actions and frames to avoid redundant code
    //NOTE: Its possible we need to pass in the currentAction and currentFrameCount
    //      because if the public variables will be set in the helper properly
    public void setActionAndFrame(string inAction, int inFrame){
        currentAction = inAction;
        currentFrameCount = inFrame;
        
    }
    
    public void getHit(int damage, bool wasBlocked, int stunFrames){
        
        // Action was blocked -> blockstun
        if (wasBlocked){
            setActionAndFrame("blockstun", stunFrames);
        }
        //Action was not blocked -> hitstun
        else{
            // Take damage
            oldHealth = currentHealth;
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0); // forces current health between 0 and maxHealth

            //run animation for health change (oldHealth to currentHealth)
            // query fightScene
            int PLAYER_2 = 2;
            fightSceneScript.changeHealthBars(PLAYER_2, oldHealth, currentHealth);
            setActionAndFrame("hitstun", stunFrames);
            if (currentHealth <= 0) {
                // fightSceneScript.gameOver(); // TODO
            }
        }
    }

    public bool hasOverlap(List<int> l1, List<int> l2){
        for (int i = 0; i < l1.Count; i++){
            if (l2.Contains(l1[i])){
                return true;
            }
        }
        return false;
    }
    
    private void forwardThrowAttack() {
        const int attackStartup = 15;
        const int damage = 50;
        const int hitstun = 45+1; //since it is a +0 move on hit, then this would be same as recovery + 1
        // int anim //TODO: would need to change the type of this
        const int attackRecovery = 45;
        List<int> hitbox = new List<int> {4}; //only hits forward position
        List<int> extendedHurtbox = new List<int> {3, 4, 5, 6};

        //play attack animation
        switch (currentFrameCount){
            //state[1] should be how many frames into the action the player is
            case 0:
                //playAttackAnim();
                break;
            case < (attackStartup - 1):
                break; 
            
            case (attackStartup - 1):
               if(hasOverlap(player1Script.p1Hurtbox, hitbox))
                {   //Values are at the beginning of the function
                    //Player2.getHit(damage, hitstun, anim); TODO: uncomment when anim type implemented
                    player1Script.getHit(damage, false, hitstun);
                }
                break;
            case < (attackStartup + attackRecovery - 1):
            //Attacking player hurtbox extended during recovery frames
                p2Hurtbox = extendedHurtbox;
                break;
            case (attackStartup + attackRecovery):
            //Attacking player hurtbox resets to previous hurtbox
                p2Hurtbox = revertHurtbox();
                currentAction = "actionable";
                currentFrameCount = 0;
                break;
            default:
                print("DEFAULT CASE IS RUNNING IN FORWARD THROW");
                break;
        } 
    }

    public void neutralThrowAttack() 
    {
        const int attackStartup = 15;
        const int damage = 30;
        const int hitstun = 20+1; //same as recovery, as it is +0 on hit  + 1
        //int anim; //TODO: would need to change the type of this
        const int attackRecovery = 20;
        List<int> hitbox = new List<int>{5}; //only hits neutral position of opponent, would this be 2?
        List<int> extendedHurtbox = new List<int> {2, 3, 4, 5}; //TODO: ask if this is how to do the extension
        //play attack animation
        switch (currentFrameCount)
        {
            //state[1] should be how many frames into the action the player is
            case 0:
                // playAttackAnim();
                break;
            case < (attackStartup - 1):
                break; 
            
            case (attackStartup - 1):
                if(hasOverlap(player1Script.p1Hurtbox, hitbox))
                {   //Values are at the beginning of the function
                    //Player2.getHit(damage, hitstun, anim); TODO: uncomment when anim type implemented
                    player1Script.getHit(damage, false, hitstun);
                }
                break;
            case < (attackStartup + attackRecovery - 1):
                p2Hurtbox = extendedHurtbox;
                break;
            case (attackStartup + attackRecovery - 1):
                p2Hurtbox = revertHurtbox();
                currentAction = "actionable";
                currentFrameCount = 0;
                break;
            default:
                print("DEFAULT CASE IS RUNNING IN NEUTRAL THROW");
                break;
        } 
    }

    public void neutralHighAttack() {
        const int damage = 10;

        const int attackStartup = 10; // Charging frames
        const int blockstun = 12; // Frames opponent would be stunned on a blocked hit for -1 diff
        const int hitstun = 16; // Frames opponent would be stunned on a direct hit for +3 diff
        const int attackRecovery = 12; // You need to recover from attack

        List<int> hitbox = new List<int> {3, 4, 5}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {2, 3, 4}; // places where you can take damage after launching attack

        switch (currentFrameCount){
            case 0:
                // playAttackAnim();
                print("start attack");
                break;
            case < (attackStartup - 1):
                break; 
            case (attackStartup -1): // -1 because currentFrameCount starts at 0
                if(hasOverlap(hitbox, player1Script.p1Hurtbox) && player1Script.p1Stance != "forward")
                {   //Values are at the beginning of the function
                    if (player1Script.isBlocking() == false){
                        player1Script.getHit(damage, false, hitstun);
                    }
                    else {
                        player1Script.getHit(damage, true, blockstun);
                    }
                    p2Hurtbox = extendedHurtbox;
                    print("player 2 hit");
                }
                break;
            case < (attackStartup + attackRecovery - 1):
                break; 
            case (attackStartup + attackRecovery - 1):
                currentAction = "actionable";
                currentFrameCount = 0;
                p2Hurtbox = revertHurtbox();
                print("end recovery");
                break;
            default:
                print("DEFAULT CASE IS RUNNING IN NEUTRAL HIGH");
                break;
        } 
    }
    
    // TODO: forwardHighAttack()

    public void forwardHighAttack() {
        const int damage = 10;

        const int attackStartup = 12; // Charging frames
        const int blockstun = 8; // Frames opponent would be stunned on a blocked hit for -3 diff
        const int hitstun = 14; // Frames opponent would be stunned on a direct hit for +3 diff
        const int attackRecovery = 10; // You need to recover from attack

        List<int> hitbox = new List<int> {4, 5, 6}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {3, 4, 6}; // places where you can take damage after launching attack

        switch (currentFrameCount){
            case 0:
                // playAttackAnim();
                print("start attack");
                break;
            case < (attackStartup - 1):
                break; 
            case (attackStartup -1): // -1 because currentFrameCount starts at 0
                if(hasOverlap(hitbox, player1Script.p1Hurtbox))
                {   //Values are at the beginning of the function
                    if (player1Script.isBlocking() == false){
                        player1Script.getHit(damage, false, hitstun);
                    }
                    else {
                        player1Script.getHit(damage, true, blockstun);
                    }
                    p2Hurtbox = extendedHurtbox;
                    print("player 2 hit");
                }
                break;
            case < (attackStartup + attackRecovery - 1):
                break; 
            case (attackStartup + attackRecovery - 1):
                currentAction = "actionable";
                currentFrameCount = 0;
                p2Hurtbox = revertHurtbox();
                print("end recovery");
                break;
            default:
                print("DEFAULT CASE IS RUNNING IN FORWARD HIGH");
                break;
        } 
    }



    public void neutralMidAttack(){
        const int damage = 20;
        const int attackStartup = 10;
        const int blockstun = 8; // Don't know yet
        const int hitstun = 18;
        const int attackRecovery = 12;

        List<int> hitbox = new List<int> {3,4}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {2,3,4};

        switch (currentFrameCount){
            case 0:
                // playAttackAnim();
                //print("start attack");
                break;
            case < attackStartup - 1:
                break;
            case attackStartup - 1:
                if(hasOverlap(hitbox, player1Script.p1Hurtbox))
                {   
                    player1Script.getHit(damage, player1Script.isBlocking(), hitstun);
                    //print("player 2 hit");
                    p2Hurtbox = extendedHurtbox;
                }
                break;
            case < attackStartup + attackRecovery - 1:
                break;
            case (attackStartup + attackRecovery - 1):
                currentAction = "actionable";
                currentFrameCount = 0;
                p2Hurtbox = revertHurtbox();
                //print("end recovery");
                break;
            default:
                print("Default running");
                break;
        } 
    }

    private void forwardMidAttack(){
        const int damage = 20;
        const int attackStartup = 16;
        const int blockstun = 14; // // How does this work?? +3 block
        const int hitstun = 18;
        const int attackRecovery = 24;

        List<int> hitbox = new List<int> {3,4,5}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {2,3,4};

        switch (currentFrameCount){
            case 0:
                // playAttackAnim();
                //print("start attack");
                break;
            case < attackStartup - 1:
                break;
            case attackStartup - 1:
                if(hasOverlap(hitbox, player1Script.p1Hurtbox))
                {   
                    player1Script.getHit(damage, player1Script.isBlocking(), hitstun);
                    //print("player 2 hit");
                    p2Hurtbox = extendedHurtbox;
                }
                break;
            case < attackStartup + attackRecovery - 1:
                break;
            case attackStartup + attackRecovery - 1:
                currentAction = "actionable";
                currentFrameCount = 0;
                p2Hurtbox = revertHurtbox();
                //print("end recovery");
                break;
            default:
                print("Default running");
                break;
        } 
    }

    // Do action for that frame. Called by FightScene every frame during a round.
    public void doAction() {
        switch (currentAction){
            case "Forward Throw":
                forwardThrowAttack();
                break;
            case "Neutral Throw":
                neutralThrowAttack();
                break;
            case "Forward High":
                forwardHighAttack();
                break;
            case "Neutral High":
                neutralHighAttack();
                break;
            case "Forward Mid":
                forwardMidAttack();
                break;
            case "Neutral Mid":
                neutralMidAttack();
                break;
            default:
                print("Default");
                break;
        }
    }
    public void reset(int maxHealth) {
        // Reset properties of player
        //Fix this
        /*
        currentHealth = maxHealth;
        p1Pos = new List<int>{1,2,3,4,5,6}; // Deprecate
        p2Hurtbox = new List<int>{2};
        p1Stance; // Position. ("backward", "neutral", "forward")
        inputHistory = new List<bool>(); 
        frame = 0; // Deprecate
        p1State = "actionable"; // Deprecate ("blocking", "hittable", "hitstun", "blockstun", "actionable")
        currentAction = "actionable";
        actionFrameCount = 0;
        */
    }
    // Resets player to the start of round position. Called at the start of every round.

    // Do action for that frame. Called by FightScene every frame during a round.
    

    // Deprecate
    // Update is called once per frame
    

}
