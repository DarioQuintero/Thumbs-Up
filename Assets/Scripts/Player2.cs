using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player2 : MonoBehaviour
{
    // Methods and variables need to be in class   
    public Player1 player1Script;

    public InputManager inputManager;


    public FightScene fightSceneScript;

    public P2Animations p2AnimationsScript;
 
    public int currentHealth;
    public int oldHealth; 
    public List<int> p2Hurtbox = new List<int>{2};
    public string p2Stance; // Position. ("backward", "neutral", "forward")
    public List<bool> inputHistory = new List<bool>(); 
    public Dictionary<string, bool> currentInput = new Dictionary<string, bool>() {
        {"high", false},
        {"mid", false},
        {"forward", false},
        {"backward", false}
    };
    public string currentAction = "Actionable"; 
    public int currentFrameCount = 0;

    private Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
    }

    
    public void setPlayerPosition(string position) { // postion: ("backward", "neutral", "forward")
        switch (position) {
            case "backward":
                p2Stance = "backward";
                p2Hurtbox = new List<int>{6};
                break;
            case "neutral":
                p2Stance = "neutral";
                p2Hurtbox = new List<int>{5};
                break;
            case "forward":
                p2Stance = "forward";
                p2Hurtbox = new List<int>{4};
                break;
            default:
                print("bad input for setPlayerPosition");
                break;
        }
    }

    public List<int> revertHurtbox() {
        switch (p2Stance) {
            case "backward":
                return new List<int> {6};
                break;
            case "neutral":
                return new List<int> {5};
                break;
            case "forward":
                return new List<int> {4};
                break;
            default:
                print("Revert Hurtbox Failed");
                return new List<int> {};
                break;
        }
    }

    public bool isBlocking() {
        if (currentAction == "Actionable" && p2Stance == "neutral") {
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
            setActionAndFrame("Blockstun", stunFrames);
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
            anim.SetBool("Hit",true); //Dario
            setActionAndFrame("Hitstun", stunFrames);
            if (currentHealth <= 0) {
                // fightSceneScript.gameOver(); // TODO
                anim.SetBool("Hit", false);
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
        print("P2 IN FORWARD THROW");
        const int attackStartup = 15;
        const int damage = 50;
        const int hitstun = 45+1; //since it is a +0 move on hit, then this would be same as recovery + 1
        // int anim //TODO: would need to change the type of this
        const int attackRecovery = 45;
        List<int> hitbox = new List<int> {3}; //only hits forward position
        List<int> extendedHurtbox = new List<int> {1, 2, 3, 4};

        //play attack animation 

        switch (currentFrameCount){
            //state[1] should be how many frames into the action the player is
            case 0:
                anim.SetTrigger("Throw"); //Dario
                currentFrameCount++; // Delete this after changing current frame count to index from 1
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
                currentAction = "Actionable";
                currentFrameCount = 0;;
                break;
            default:
                print("DEFAULT CASE IS RUNNING IN FORWARD THROW");
                break;
        }
        //Delete this after changing current frame count to index from 1
        if (currentFrameCount != 0) {
            currentFrameCount = currentFrameCount + 1;
        }
    }

    public void neutralThrowAttack() 
    {
        print("P2 IN NEUTRAL THROW");
        const int attackStartup = 15;
        const int damage = 30;
        const int hitstun = 20+1; //same as recovery, as it is +0 on hit  + 1
        //int anim; //TODO: would need to change the type of this
        const int attackRecovery = 20;
        List<int> hitbox = new List<int>{2}; //only hits neutral position of opponent - yes this is 2
        List<int> extendedHurtbox = new List<int> {2, 3, 4, 5}; //TODO: ask if this is how to do the extension
        //play attack animation

        switch (currentFrameCount){
            //state[1] should be how many frames into the action the player is
            case 0:
                anim.SetTrigger("Throw"); //Dario
                currentFrameCount++; // Delete this after changing current frame count to index from 1
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
                currentAction = "Actionable";
                currentFrameCount = 0;;
                break;
            default:
                print("DEFAULT CASE IS RUNNING IN NEUTRAL THROW");
                break;
        }
        //Delete this after changing current frame count to index from 1
        if (currentFrameCount != 0) {
            currentFrameCount = currentFrameCount + 1;
        }
    }

    public void neutralHighAttack() {
        print("P2 IN NEUTRAL HIGH");
        const int damage = 10;

        const int attackStartup = 10; // Charging frames
        const int blockstun = 12; // Frames opponent would be stunned on a blocked hit for -1 diff
        const int hitstun = 16; // Frames opponent would be stunned on a direct hit for +3 diff
        const int attackRecovery = 12; // You need to recover from attack

        List<int> hitbox = new List<int> {2, 3, 4}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {3, 4, 5}; // places where you can take damage after launching attack


        switch (currentFrameCount){
            case 0:
                anim.SetTrigger("High"); //Dario
                currentFrameCount++; // Delete this after changing current frame count to index from 1
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
                    print("player 1 hit with neutral high attack");
                }
                break;
            case < (attackStartup + attackRecovery - 1):
                break; 
            case (attackStartup + attackRecovery - 1):
                currentAction = "Actionable";
                currentFrameCount = 0;;
                p2Hurtbox = revertHurtbox();
                print("end recovery");
                break;
            default: 
                print("DEFAULT CASE IS RUNNING IN NEUTRAL HIGH");
                break;
        }
        //Delete this after changing current frame count to index from 1
        if (currentFrameCount != 0) {
            currentFrameCount = currentFrameCount + 1;
        }
    }
    
    // TODO: forwardHighAttack()

    public void forwardHighAttack() {
        print("P2 IN FORWARD HIGH");
        const int damage = 10;

        const int attackStartup = 12; // Charging frames
        const int blockstun = 8; // Frames opponent would be stunned on a blocked hit for -3 diff
        const int hitstun = 14; // Frames opponent would be stunned on a direct hit for +3 diff
        const int attackRecovery = 10; // You need to recover from attack

        List<int> hitbox = new List<int> {1, 2, 3}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {2, 3, 4}; // places where you can take damage after launching attack


        switch (currentFrameCount){
            case 0:
                anim.SetTrigger("High"); //Dario
                currentFrameCount++; // Delete this after changing current frame count to index from 1
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
                    print("player 1 hit with forward high attack");
                }
                break;
            case < (attackStartup + attackRecovery - 1):
                break; 
            case (attackStartup + attackRecovery - 1):
                currentAction = "Actionable";
                currentFrameCount = 0;;
                p2Hurtbox = revertHurtbox();
                print("end recovery");
                break;
            default:
                print("DEFAULT CASE IS RUNNING IN FORWARD HIGH");
                break;
        }
        //Delete this after changing current frame count to index from 1
        if (currentFrameCount != 0) {
            currentFrameCount = currentFrameCount + 1;
        }
    }

    public void neutralMidAttack(){
        print("P2 IN NEUTRAL MID");
        const int damage = 20;
        const int attackStartup = 10;
        const int blockstun = 8; // Don't know yet
        const int hitstun = 18;
        const int attackRecovery = 12;

        List<int> hitbox = new List<int> {3,4}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {4,5};


        switch (currentFrameCount){
            case 0:
                print("CASE 0");
                anim.SetTrigger("Mid"); //Dario
                currentFrameCount++; // Delete this after changing current frame count to index from 1
                //print("start attack");
                break;
            case < attackStartup - 1:
                print("CASE 1");
                break;
            case attackStartup - 1:
                print("CASE 2");
                if(hasOverlap(hitbox, player1Script.p1Hurtbox))
                {   
                    player1Script.getHit(damage, player1Script.isBlocking(), hitstun);
                    //print("player 2 hit");
                    p2Hurtbox = extendedHurtbox;
                }
                break;
            case < attackStartup + attackRecovery - 1:
                print("CASE 3");
                break;
            case (attackStartup + attackRecovery - 1):
                print("CASE 4");
                currentAction = "Actionable";
                currentFrameCount = 0;;
                p2Hurtbox = revertHurtbox();
                //print("end recovery");
                break;
            default:
                print("Default running");
                break;
        } 
        //Delete this after changing current frame count to index from 1
        if (currentFrameCount != 0) {
            currentFrameCount = currentFrameCount + 1;
        }
    }

    private void forwardMidAttack(){
        print("P1 IN FORWARD MID");
        const int damage = 20;
        const int attackStartup = 16;
        const int blockstun = 14; // // How does this work?? +3 block
        const int hitstun = 18;
        const int attackRecovery = 24;

        List<int> hitbox = new List<int> {2,3}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {3,4};


        switch (currentFrameCount){
            case 0:
                anim.SetTrigger("Mid"); //Dario
                currentFrameCount++; // Delete this after changing current frame count to index from 1
                //print("start attack");
                break;
            case < attackStartup - 1:
                break;
            case attackStartup - 1:
                if(hasOverlap(hitbox, player1Script.p1Hurtbox))
                {   
                    player1Script.getHit(damage, player1Script.isBlocking(), hitstun);
                    //print("player 1 hit");
                    p2Hurtbox = extendedHurtbox;
                }
                break;
            case < attackStartup + attackRecovery - 1:
                break;
            case attackStartup + attackRecovery - 1:
                currentAction = "Actionable";
                currentFrameCount = 0;;
                p2Hurtbox = revertHurtbox();
                //print("end recovery");
                break;
            default:
                print("Default running");
                break;
        }
        //Delete this after changing current frame count to index from 1
        if (currentFrameCount != 0) {
            currentFrameCount = currentFrameCount + 1;
        }
    }
   void updateInputs() {
        //print("IN UPDATE INPUTS--------------");
        // TODO: Save old input dict in input history to be read later
        currentInput = new Dictionary<string, bool>() {
            {"high", inputManager.KeyDown("p2High")},
            {"mid", inputManager.KeyDown("p2Mid")},
            {"forward", inputManager.KeyDown("p2Forward")},
            {"backward", inputManager.KeyDown("p2Backward")}
        };
    }

    int boolToInt(bool myBool) {
        return myBool ? 1 : 0;
    }

    string inputsToActions() {
        //print("IN INPUTSTOACTION ----IIIIII-------");
        int inputsAsBinary = boolToInt(currentInput["high"]) * 8 + boolToInt(currentInput["mid"]) * 4
                           + boolToInt(currentInput["backward"]) * 2 + boolToInt(currentInput["forward"]) * 1;
        switch (inputsAsBinary) {
            case 4:
                return "Neutral Mid";
            case 5:
                return "Forward Mid";
            case 7:
                return "Neutral Mid";
            case 8:
                return "Neutral High";
            case 9:
                return "Forward High";
            case 11:
                return "Neutral High";
            case 12:
                return "Neutral Throw";
            case 13:
                return "Forward Throw";
            case 15:
                return "Neutral Throw";
            default:
                return "Actionable";
        }
    }

    void queueAction() {
        //print("IN QUEUE ACTION ----QQQQQQQ-------");
        // TODO: Implement an action queue and pop it
        if (currentAction == "Actionable") {
            //print("WE ARE ACTIONABLE------AAAAAAA-------");
            currentAction = inputsToActions();
        }
    }

    // Do action for that frame. Called by FightScene every frame during a round.
    public void doAction() {
        //print("IN DO ACTION_________________");
        print("-----------P2-"+p2Stance+"--------------");
        updateInputs();
        queueAction();
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
            case "Hitstun":
                p2Stance = "neutral"; // Subject to change
                currentFrameCount -= 1;
                if (currentFrameCount <= 0) {
                    currentAction = "Actionable";
                    currentFrameCount = 0;
                    anim.SetBool("Hit",false); //Dario
                }
                break;
            case "Blockstun":
                p2Stance = "neutral"; // Subject to change
                currentFrameCount -= 1;
                if (currentFrameCount <= 0) {
                    currentAction = "Actionable";
                    currentFrameCount = 0;
                }
                break;
            default:
                print("Actionable");
                // TODO: Implement a movement cooldown with adjustable frames relative to a const
                // Use setPlayerPosition to also update stance? 
                if (currentInput["forward"] && !currentInput["backward"]) {
                    // p2Stance = "forward";
                    anim.SetInteger("Position",1); //Dario
                    setPlayerPosition("forward");
                }
                else if (currentInput["backward"] && !currentInput["forward"]) {
                    // p2Stance = "backward";
                    anim.SetInteger("Position",-1); //Dario
                    setPlayerPosition("backward");
                }
                else {
                    anim.SetInteger("Position",0); //Dario
                    setPlayerPosition("neutral");
                    // p2Stance = "neutral";
                }
                break;
        }
    }
    public void reset(int maxHealth) {
        // Reset properties of player

        currentHealth = maxHealth;
        // p1Pos = new List<int>{1,2,3,4,5,6}; // Deprecate
        p2Hurtbox = new List<int>{5};
        p2Stance = "neutral"; // Position. ("backward", "neutral", "forward")
        inputHistory = new List<bool>(); 
        // frame = 0; // Deprecate
        // p1State = "Actionable"; // Deprecate ("blocking", "hittable", "hitstun", "blockstun", "Actionable")
        currentAction = "Actionable";
        currentFrameCount = 0;
        
    }
    

}
