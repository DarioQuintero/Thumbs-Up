using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    // Methods and variables need to be in class  
    public int playerIndex = 0;
    public Player2 player2Script;

    public InputManager inputManager;
    public UserInput controller;

    // Get FightScene's Script
    public FightScene fightSceneScript;
    
    public P1Animations p1AnimationsScript;
    // public int maxHealth; // Deprecate
    public int currentHealth;
    public int oldHealth; 
    public List<int> p1Hurtbox = new List<int>{2};
    public string p1Stance; // Position ("backward", "neutral", "forward")
    public List<bool> inputHistory = new List<bool>(); 
    public Dictionary<string, bool> currentInput = new Dictionary<string, bool>() {
        {"high", false},
        {"mid", false},
        {"forward", false},
        {"backward", false}
    };
    public string currentAction = "Actionable"; 
    public int currentFrameCount = 0;

    const int BUFFERLENGTH = 5;
    const int INPUTLENIENCY = 3;
    struct ActionInput {
        public string action;
        public int count;
        public ActionInput(string a, int c) {
            this.action = a;
            this.count = c;
        }
    }
    ActionInput actionBuffer = new ActionInput("Actionable", 0);


    public Animator anim;

    VFXHandler vfxHandler;

    private int hitFreezeFrames = 5;

    private int blockFreezeFrames = 5;

    public SoundGroupScript highSounds, highAttempt, midSounds, midAttempt, grabHitSounds, blockSound;


    public List<AudioSource> hurtSounds = new List<AudioSource>();

    void Start(){
        vfxHandler = gameObject.GetComponent<VFXHandler>();
    }

    public int getPlayerIndex() {
        return playerIndex;
    }

    public void setPlayerPosition(string position) { // postion: ("backward", "neutral", "forward")
        switch (position) {
            case "backward":
                p1Stance = "backward";
                p1Hurtbox = new List<int>{1};
                break;
            case "neutral":
                p1Stance = "neutral";
                p1Hurtbox = new List<int>{2};
                break;
            case "forward":
                p1Stance = "forward";
                p1Hurtbox = new List<int>{3};
                break;
            default:
                print("bad input for setPlayerPosition");
                break;
        }
    }

    public List<int> revertHurtbox() {
        switch (p1Stance) {
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
        if (currentAction == "Actionable" && p1Stance == "neutral") {
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

    // public void playBlockSound() {
    //     blockSound.Stop();
    //     blockSound.time = 1.4f;
    //     blockSound.Play();
    // }

    // public void playHitSound() {
    //     hitSound.Stop();
    //     hitSound.time = 0.2f;
    //     hitSound.Play();
    //     playHurtSound();
    // }

    // public void playmissSound() {
    //     missSound.Stop();
    //     missSound.time = 0.1f;
    //     missSound.Play();
    // }

    // public void playHurtSound() {
    //     // Random rnd = new Random();

    //     int x = 0; //rnd.Next(1, 6);
    //     hurtSounds[x].Stop();
    //     hurtSounds[x].time = 0.1f;
    //     hurtSounds[x].Play();
    // }
    
    public void getHit(int damage, bool wasBlocked, int stunFrames){
        
        // Action was blocked -> blockstun
        if (wasBlocked) {
            
            // playBlockSound();
            fightSceneScript.frozenFrames = blockFreezeFrames;
            setActionAndFrame("Blockstun", stunFrames);
            anim.SetBool("Block",true);
            vfxHandler.CallEffect("Block");
        }
        //Action was not blocked -> hitstun
        else{
            // playHitSound();
            // Take damage
            oldHealth = currentHealth;
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0); // forces current health between 0 and maxHealth
            fightSceneScript.frozenFrames = hitFreezeFrames;

            //run animation for health change (oldHealth to currentHealth)
            // query fightScene
            int PLAYER_1 = 1;
            fightSceneScript.changeHealthBars(PLAYER_1, oldHealth, currentHealth);
            anim.SetBool("Hit",true); //Dario
            setActionAndFrame("Hitstun", stunFrames);
            if (currentHealth <= 0) {
                // fightSceneScript.gameOver(); // TODO
                //anim.SetBool("Hit", false);
            }

        }
    }

    public bool hasOverlap(List<int> l1, List<int> l2){
        for (int i = 0; i < l1.Count; i++){
            if (l2.Contains(l1[i])){
                return true;
            }
        }
        // print("miss");
        // playmissSound(); // janky but works
        return false;
    }
    
    private void forwardThrowAttack() {
        print("P1 IN FORWARD THROW");
        const int attackStartup = 15;
        const int damage = 40;
        const int hitstun = 45+1; //since it is a +0 move on hit, then this would be same as recovery + 1
        // int anim //TODO: would need to change the type of this
        const int attackRecovery = 45;
        List<int> hitbox = new List<int> {4}; //only hits forward position
        List<int> extendedHurtbox = new List<int> {3, 4, 5, 6};
        List<int> invincibleHurtbox = new List<int> {};
        //play attack animation
        switch (currentFrameCount){
            //state[1] should be how many frames into the action the player is
            case 0:
                setPlayerPosition("forward");
                anim.SetTrigger("Throw"); //Dario
                currentFrameCount++; // Delete this after changing current frame count to index from 1
                p1Hurtbox = invincibleHurtbox;
                print("p1 is invincible");
                break;
            case < (attackStartup - 1):
                print("current hurtbox: " + string.Join(",", p1Hurtbox));
                break; 
            
            case (attackStartup - 1):
               if(hasOverlap(player2Script.p2Hurtbox, hitbox))
                {   //Values are at the beginning of the function
                    //Player2.getHit(damage, hitstun, anim); TODO: uncomment when anim type implemented
                    fightSceneScript.damageBroadcast(1, damage, false, hitstun);
                    grabHitSounds.playRandomSound();
                }
                break;
            case < (attackStartup + attackRecovery - 1):
            //Attacking player hurtbox extended during recovery frames
                p1Hurtbox = extendedHurtbox;
                print("p1 is vulnerable");
                break;
            case (attackStartup + attackRecovery - 1):
            //Attacking player hurtbox resets to previous hurtbox
                p1Hurtbox = revertHurtbox();
                currentAction = "Actionable";
                currentFrameCount = 0;
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
        print("P1 IN NEUTRAL THROW");
        const int attackStartup = 15;
        const int damage = 30;
        const int hitstun = 20+1; //same as recovery, as it is +0 on hit  + 1
        //int anim; //TODO: would need to change the type of this
        const int attackRecovery = 20;
        List<int> hitbox = new List<int>{5}; //only hits neutral position of opponent, would this be 2?
        List<int> extendedHurtbox = new List<int> {2, 3, 4, 5}; //TODO: ask if this is how to do the extension

        switch (currentFrameCount)
        {
            //state[1] should be how many frames into the action the player is
            case 0:
                setPlayerPosition("neutral");
                anim.SetTrigger("Throw"); //Dario
                currentFrameCount++; // Delete this after changing current frame count to index from 1
                print("CASE 0");
                break;
            case < (attackStartup - 1):
                break; 

                print("CASE 1");
            case (attackStartup - 1):

                print("CASE 2");
                if(hasOverlap(player2Script.p2Hurtbox, hitbox))
                {   //Values are at the beginning of the function
                    //Player2.getHit(damage, hitstun, anim); TODO: uncomment when anim type implemented
                    fightSceneScript.damageBroadcast(1, damage, false, hitstun);
                    grabHitSounds.playRandomSound();
                    //neutralThrowHit
                }
                break;
            case < (attackStartup + attackRecovery - 1):
                print("CASE 3");
                p1Hurtbox = extendedHurtbox;
                break;
            case (attackStartup + attackRecovery - 1):
                print("CASE 4");
                p1Hurtbox = revertHurtbox();
                currentAction = "Actionable";
                currentFrameCount = 0;
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
        print("P1 IN NEUTRAL HIGH");
        const int damage = 10;

        const int attackStartup = 10; // Charging frames
        const int blockstun = 12; // Frames opponent would be stunned on a blocked hit for -1 diff
        const int hitstun = 16; // Frames opponent would be stunned on a direct hit for +3 diff
        const int attackRecovery = 12; // You need to recover from attack

        List<int> hitbox = new List<int> {3, 4, 5, 6}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {2, 3, 4, 5}; // places where you can take damage after launching attack

        switch (currentFrameCount){
            case 0:
                setPlayerPosition("neutral");
                anim.SetTrigger("High"); 
                print("start attack");
                currentFrameCount++; // Delete this after changing current frame count to index from 1
                highAttempt.playRandomSound();
                break;
            case < (attackStartup - 1):
                break; 
            case (attackStartup -1): // -1 because currentFrameCount starts at 0
                if(hasOverlap(hitbox, player2Script.p2Hurtbox) && player2Script.p2Stance != "forward")
                {   //Values are at the beginning of the function
                    if (player2Script.isBlocking() == false){
                        fightSceneScript.damageBroadcast(1, damage, false, hitstun);
                        highSounds.playRandomSound();
                        vfxHandler.CallEffect("NeutralHigh",player2Script.p2Stance);

                    }
                    else {
                        fightSceneScript.damageBroadcast(1, damage, true, blockstun);
                        blockSound.playRandomSound();
                    }
                    p1Hurtbox = extendedHurtbox;
                    print("player 2 hit with neutral high attack");
                }
                break;
            case < (attackStartup + attackRecovery - 1):
                break; 
            case (attackStartup + attackRecovery - 1):
                currentAction = "Actionable";
                currentFrameCount = 0;
                p1Hurtbox = revertHurtbox();
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

    public void forwardHighAttack() {
        print("P1 IN FORWARD HIGH");
        const int damage = 10;

        const int attackStartup = 12; // Charging frames
        const int blockstun = 8; // Frames opponent would be stunned on a blocked hit for -3 diff
        const int hitstun = 10; // Frames opponent would be stunned on a direct hit for -1 diff
        const int attackRecovery = 10; // You need to recover from attack

        List<int> hitbox = new List<int> {4, 5, 6}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {3, 4, 5}; // places where you can take damage after launching attack


        switch (currentFrameCount){
            case 0:
                setPlayerPosition("forward");
                anim.SetTrigger("High"); //Dario
                currentFrameCount++; // Delete this after changing current frame count to index from 1
                print("start attack");
                highAttempt.playRandomSound();
                break;
            case < (attackStartup - 1):
                break; 
            case (attackStartup -1): // -1 because currentFrameCount starts at 0
                if(hasOverlap(hitbox, player2Script.p2Hurtbox))
                {   //Values are at the beginning of the function
                    if (player2Script.isBlocking() == false){
                        fightSceneScript.damageBroadcast(1, damage, false, hitstun);
                        highSounds.playRandomSound();
                        vfxHandler.CallEffect("ForwardHigh",player2Script.p2Stance);
                        
                    }
                    else {
                        fightSceneScript.damageBroadcast(1, damage, true, blockstun);
                        blockSound.playRandomSound();
                        //anim.SetBool("Hit",true); // Placeholder for recoil animation
                    }
                    p1Hurtbox = extendedHurtbox;
                    print("player 2 hit with forward high attack");
                }
                break;
            case < (attackStartup + attackRecovery - 1):
                break; 
            case (attackStartup + attackRecovery - 1):
                currentAction = "Actionable";
                //anim.SetBool("Hit",false); // Placeholder for recoil animation
                currentFrameCount = 0;
                p1Hurtbox = revertHurtbox();
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
        print("P1 IN NEUTRAL MID");
        const int damage = 20;
        const int attackStartup = 10;
        const int blockstun = 18; // Shouldnt be able to be blocked
        const int hitstun = 18;
        const int attackRecovery = 12;

        List<int> hitbox = new List<int> {3,4}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {2,3};


        switch (currentFrameCount){
            case 0:
                print("CASE 0");
                setPlayerPosition("neutral");
                anim.SetTrigger("Mid"); //Dario
                //print("start attack");
                currentFrameCount++; // Delete this after changing current frame count to index from 1
                midAttempt.playRandomSound();
                break;
            case < attackStartup - 1:
                print("CASE 1");
                break;
            case attackStartup - 1:
                print("CASE 2");
                if(hasOverlap(hitbox, player2Script.p2Hurtbox))
                {   
                    fightSceneScript.damageBroadcast(1, damage, player2Script.isBlocking(), hitstun);
                    //print("player 2 hit");
                    p1Hurtbox = extendedHurtbox;
                    if (player2Script.isBlocking() == false){
                        midSounds.playRandomSound();
                        vfxHandler.CallEffect("NeutralMid",player2Script.p2Stance);
                    }
                    else {
                        blockSound.playRandomSound();
                    }
                }
                break;
            case < attackStartup + attackRecovery - 1:
                print("CASE 3");
                break;
            case (attackStartup + attackRecovery - 1):
                print("CASE 4");
                currentAction = "Actionable";
                currentFrameCount = 0;
                p1Hurtbox = revertHurtbox();
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
        const int blockstun = 28; // // How does this work?? +3 block
        const int hitstun = 30;
        const int attackRecovery = 24;

        List<int> hitbox = new List<int> {4,5}; // places where opponent can take damage
        List<int> extendedHurtbox = new List<int> {3,4};


        switch (currentFrameCount){
            case 0:
                setPlayerPosition("forward");
                anim.SetTrigger("Mid"); //Dario
                // Delete this after changing current frame count to index from 1
                currentFrameCount++;
                midAttempt.playRandomSound();
                //print("start attack");
                break;
            case < attackStartup - 1:
                break;
            case attackStartup - 1:
                if(hasOverlap(hitbox, player2Script.p2Hurtbox))
                {   
                    fightSceneScript.damageBroadcast(1, damage, player2Script.isBlocking(), hitstun);
                    //print("player 2 hit");
                    p1Hurtbox = extendedHurtbox;
                    if (player2Script.isBlocking() == false){
                        vfxHandler.CallEffect("ForwardMid",player2Script.p2Stance);
                        midSounds.playRandomSound();
                    }
                    else {
                        blockSound.playRandomSound();
                    }
                }
                break;
            case < attackStartup + attackRecovery - 1:
                break;
            case attackStartup + attackRecovery - 1:
                currentAction = "Actionable";
                currentFrameCount = 0; 
                p1Hurtbox = revertHurtbox();
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
        /*
        currentInput = new Dictionary<string, bool>() {
            {"high", inputManager.KeyDown("p1High")},
            {"mid", inputManager.KeyDown("p1Mid")},
            {"block", inputManager.KeyDown("p1Block")},
            {"forward", inputManager.KeyDown("p1Right")},
            {"backward", inputManager.KeyDown("p1Left")}
        };
        */
        currentInput = new Dictionary<string, bool>() {
            {"high", controller.highJustPressed},
            {"mid", controller.midJustPressed},
            {"block", controller.blockBeingHeld},
            {"forward", controller.rightBeingHeld},
            {"backward", controller.leftBeingHeld}
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
                anim.SetInteger("Position",0); //Dario
                return "Neutral Mid";
            case 5:
                anim.SetInteger("Position",1); //Dario
                return "Forward Mid";
            case 7:
                anim.SetInteger("Position",0); //Dario
                return "Neutral Mid";
            case 8:
                anim.SetInteger("Position",0); //Dario
                return "Neutral High";
            case 9:
                anim.SetInteger("Position",1); //Dario
                return "Forward High";
            case 11:
                anim.SetInteger("Position",0); //Dario
                return "Neutral High";
            case 12:
                anim.SetInteger("Position",0); //Dario
                return "Neutral Throw";
            case 13:
                anim.SetInteger("Position",1); //Dario
                return "Forward Throw";
            case 15:
                anim.SetInteger("Position",0); //Dario
                return "Neutral Throw";
            default:
                return "Actionable";
        }
    }

    void queueAction() {
        //print("IN QUEUE ACTION ----QQQQQQQ-------");
        // TODO: Implement an action queue and pop it
        string thisFrameAction = inputsToActions();
        if (currentAction != "Actionable" && thisFrameAction != "Actionable") {
           // Store action in buffer
            actionBuffer.action = thisFrameAction;
            actionBuffer.count = 0;
            print(actionBuffer.action);
            print(actionBuffer.count);
        }
        else {
            actionBuffer.count += 1;
            if (actionBuffer.count <= BUFFERLENGTH && actionBuffer.action != "Actionable") {
                print(actionBuffer.action + "is still buffered");
                print(actionBuffer.count);
            }
        }
        if (currentAction == "Actionable") {
            //print("WE ARE ACTIONABLE------AAAAAAA-------");
            if (actionBuffer.action != "Actionable" && actionBuffer.count <= BUFFERLENGTH) {
                currentAction = actionBuffer.action;
                print("using buffered action");
            }
            else {
                currentAction = inputsToActions();
            }
            actionBuffer = new ActionInput("Actionable", 0);
        }
        else if ((currentAction == "Neutral High" ||
              currentAction == "Neutral Mid") && 
              currentFrameCount <= INPUTLENIENCY) {
            if (controller.highBeingHeld && controller.midBeingHeld) {
                currentAction = "Neutral Throw";
                currentFrameCount = 0;
            }
        }
        else if ((currentAction == "Forward High" ||
              currentAction == "Forward Mid") &&
              currentFrameCount <= INPUTLENIENCY) {
            if (controller.highBeingHeld && controller.midBeingHeld) {
                currentAction = "Forward Throw";
                currentFrameCount = 0;
            }
        }
    }

    // Do action for that frame. Called by FightScene every frame during a round.
    public void doAction(bool frozen) {
        //print("IN DO ACTION_________________");
        //print("-----------P1-"+p1Stance+"--------------");
        if (controller.highJustPressed == true) {
            print("P1 CONTROLLER INPUT DETECTED");
            print("Input from controller index " + controller.controllerIndex);
        }
        updateInputs();
        queueAction();
        if (!frozen) {
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
                    p1Stance = "neutral"; // Subject to change
                    currentFrameCount -= 1;
                    if (currentFrameCount <= 0) {
                        currentAction = "Actionable";
                        currentFrameCount = 0;
                        anim.SetBool("Hit",false); //Dario
                    }
                    break;
                case "Blockstun":
                    p1Stance = "neutral"; // Subject to change
                    currentFrameCount -= 1;
                    if (currentFrameCount <= 0) {
                        currentAction = "Actionable";
                        currentFrameCount = 0;
                        anim.SetBool("Block",false);
                    }
                    break;
                default:
                    //print("Actionable");
                    // TODO: Implement a movement cooldown with adjustable frames relative to a const
                    // Use setPlayerPosition to also update stance? 
                    if (currentInput["backward"] && !currentInput["forward"] && !currentInput["block"]) {
                        // p1Stance = "backward";
                        setPlayerPosition("backward");
                        anim.SetInteger("Position",-1); //Dario
                    }
                    else if (currentInput["forward"] && !currentInput["backward"] && !currentInput["block"]) {
                        // p1Stance = "forward";
                        setPlayerPosition("forward");
                        anim.SetInteger("Position",1); //Dario
                    }
                    else {
                        setPlayerPosition("neutral");
                        anim.SetInteger("Position",0); //Dario
                        // p1Stance = "neutral";
                    }
                    break;
            }
        }
    }

    public bool pressingAnyAttacks() {
        updateInputs();
        if (currentInput["high"] || currentInput["mid"]) {
            print("p1 pressing any attacks");
            return true;
        }
        else {
            return false;
        }
    }
    public void reset(int maxHealth) {
        // Reset properties of player

        currentHealth = maxHealth;
        // p1Pos = new List<int>{1,2,3,4,5,6}; // Deprecate
        p1Hurtbox = new List<int>{2};
        p1Stance = "neutral"; // Position. ("backward", "neutral", "forward")
        inputHistory = new List<bool>(); 
        // frame = 0; // Deprecate
        // p1State = "Actionable"; // Deprecate ("blocking", "hittable", "hitstun", "blockstun", "Actionable")
        currentAction = "Actionable";
        currentFrameCount = 0;
        ActionInput actionBuffer = new ActionInput("Actionable", 0);
        anim.SetBool("Hit", false);
    }
    // Resets player to the start of round position. Called at the start of every round.

    // Do action for that frame. Called by FightScene every frame during a round.
    

    // Deprecate
    // Update is called once per frame
    

}
