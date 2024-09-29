using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player1 : MonoBehaviour
{
    // Methods and variables need to be in class   
    public Player2 player2Script;
 
    public int maxHealth; // Deprecate
    public int currentHealth;
    public int oldHealth; 
    public List<int> p1Hurtbox = new List<int>{2};
    public string p1Stance; // Position. ("backward", "neutral", "forward")
    public List<bool> inputHistory = new List<bool>(); 
    public string currentAction = "actionable"; 
    public int currentFrameCount = 0;

    public List<int> revertHurtbox() {
        switch (p1Stance) {
            case "backward":
                return new List<int> [1];
            case "neutral":
                return new List<int> [2];
            case "forward":
                return new List<int> [3];
            default:
                print("Revert Hurtbox Failed");
        }
    }

    public bool isBlocking() {
        if (currentAction == "acitionable" && p1Stance == "neutral") {
            return true;
        }
        return false;
    }

    //Helper function for when we set the actions and frames to avoid redundant code
    //NOTE: Its possible we need to pass in the currentAction and currentFrameCount
    //      because if the public variables will be set in the helper properly
    public void setActionAndFrame(inAction, inFrame){
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
            PLAYER_1 = 1;
            FightScence.changeHealthBars(PLAYER_1, oldHealth, currentHealth);
            setActionAndFrame("hitstun", stunFrames);
            if (currentHealth <= 0) {
                FightScene.gameOver(); // TODO
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
        int attackStartup = 15;
        int damage = 50;
        int hitstun = 45+1; //since it is a +0 move on hit, then this would be same as recovery + 1
        // int anim //TODO: would need to change the type of this
        int attackRecovery = 45;
        int List hitbox = [4]; //only hits forward position
        int List extendedHurtbox = [3, 4, 5, 6];

        //play attack animation
        switch (currentFrameCount){
            //state[1] should be how many frames into the action the player is
            case 0:
                playAttackAnim();

            case < (attackStartup - 1):
                continue; 
            
            case (attackStartup - 1):
               if(hasOverlap(Player2.p2Hurtbox, hitbox))
                {   //Values are at the beginning of the function
                    //Player2.getHit(damage, hitstun, anim); TODO: uncomment when anim type implemented
                    Player2.getHit(damage, hitstun, 0);
                }
            case < (attackStartup + attackRecovery - 1):
            //Attacking player hurtbox extended during recovery frames
                p1Hurtbox = extendedHurtbox;
                continue;
            case (attackStartup + attackRecovery):
            //Attacking player hurtbox resets to previous hurtbox
                p1Hurtbox = revertHurtbox();
                currentAction = "actionable";
                currentFrameCount = 0;
            default:
                print("DEFAULT CASE IS RUNNING IN FORWARD THROW");
        } 
    }

    public void neutralThrowAttack() 
    {
        int attackStartup = 15;
        int damage = 30;
        int hitstun = 20+1; //same as recovery, as it is +0 on hit  + 1
        //int anim; //TODO: would need to change the type of this
        int attackRecovery = 20;
        int List hitbox = [5]; //only hits neutral position of opponent, would this be 2?
        int List extendedHurtbox = [2, 3, 4, 5]; //TODO: ask if this is how to do the extension
        //play attack animation
        switch (currentFrameCount)
        {
            //state[1] should be how many frames into the action the player is
            case 0:
                playAttackAnim();

            case < (attackStartup - 1):
                continue; 
            
            case (attackStartup - 1):
                if(hasOverlap(Player2.p2Hurtbox, hitbox))
                {   //Values are at the beginning of the function
                    //Player2.getHit(damage, hitstun, anim); TODO: uncomment when anim type implemented
                    Player2.getHit(damage, hitstun, 0);
                }
            case < (attackStartup + attackRecovery - 1):
                p1Hurtbox = extendedHurtbox;
                continue;
            case (attackStartup + attackRecovery - 1):
                p1Hurtbox = revertHurtbox();
                currentAction = "actionable";
                currentFrameCount = 0;
            default:
                print("DEFAULT CASE IS RUNNING IN NEUTRAL THROW");
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
            case attackStartup:
                if(has_overlap(hitbox, player2Script.p2Hurtbox) && player2Script.p2Stance != "forward")
                {   //Values are at the beginning of the function
                    if (player2Script.block == false){
                        player2Script.getHit(damage, false, hitstun);
                    }
                    else {
                        player2Script.getHit(damage, true, blockstun);
                    }
                    p1Hurtbox = extendedHurtbox;
                    print("player 2 hit");
                }
                break;
            case (attackStartup + attackRecovery):
                currentAction = "actionable";
                currentFrameCount = 0;
                p1Hurtbox = new List<int> {2};
                print("end recovery");
                break;
            default:
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
            case attackStartup:
                if(has_overlap(hitbox, player2Script.p2Hurtbox))
                {   //Values are at the beginning of the function
                    if (player2Script.block == false){
                        player2Script.getHit(damage, false, hitstun);
                    }
                    else {
                        player2Script.getHit(damage, true, blockstun);
                    }
                    print("player 2 hit");
                    p1Hurtbox = extendedHurtbox;
                }
                break;
            case (attackStartup + attackRecovery):
                currentAction = "actionable";
                currentFrameCount = 0;
                p1Hurtbox = new List<int> {3};
                print("end recovery");
                break;
            default:
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
                continue;
            case attackStartup - 1:
                if(has_overlap(hitbox, player2Script.p2Hurtbox))
                {   
                    block = isBlocking()
                    player2Script.getHit(damage, block, hitstun);
                    //print("player 2 hit");
                    p1Hurtbox = extendedHurtbox;
                }
                break;
            case < attackStartup + attackRecovery - 1:
                continue;
            case (attackStartup + attackRecovery - 1):
                currentAction = "actionable";
                currentFrameCount = 0;
                p1Hurtbox = revertHurtbox();
                //print("end recovery");
                break;
            default:
                print("Default running");
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
                continue;
            case attackStartup - 1:
                if(has_overlap(hitbox, player2Script.p2Hurtbox))
                {   
                    block = isBlocking()
                    player2Script.getHit(damage, block, hitstun);
                    //print("player 2 hit");
                    p1Hurtbox = extendedHurtbox;
                }
                break;
            case < attackStartup + attackRecovery - 1:
                continue;
            case attackStartup + attackRecovery - 1:
                currentAction = "actionable";
                currentFrameCount = 0;
                p1Hurtbox = revertHurtbox();
                //print("end recovery");
                break;
            default:
                print("Default running");
        } 
    }

    // Deprecate
    // Start is called before the first frame update
    
    // Resets player to the start of round position. Called at the start of every round.
    public void reset()
    {
        // Reset properties of player
        currentHealth = maxHealth;
        
        oldHealth = maxHealth;
        //public bool block; // Deprecate
        //public List<int> p1Pos = new List<int>{1,2,3,4,5,6}; // Deprecate
        //public List<int> p1Hurtbox = new List<int>{2};
        p1Stance = "neutral"; // Position. ("backward", "neutral", "forward")
        inputHistory = new List<bool>(); 
        //public int frame = 0; // Deprecate
        //public string p1State = "actionable"; // Deprecate ("blocking", "hittable", "hitstun", "blockstun", "actionable")
        currentAction = "actionable";
        currentFrameCount = 0;
    }

    // Do action for that frame. Called by FightScene every frame during a round.
    public void doAction() {
        // Take in input and update input history
        // 
    }

    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    // Resets player to the start of round position. Called at the start of every round.
    public void reset(int maxHealth) {
        // Reset properties of player
        currentHealth = maxHealth;
        public List<int> p1Pos = new List<int>{1,2,3,4,5,6}; // Deprecate
        public List<int> p1Hurtbox = new List<int>{2};
        public string p1Stance; // Position. ("backward", "neutral", "forward")
        public List<bool> inputHistory = new List<bool>(); 
        public int frame = 0; // Deprecate
        public string p1State = "actionable" // Deprecate ("blocking", "hittable", "hitstun", "blockstun", "actionable")
        public string currentAction = "actionable";
        public int actionFrameCount = 0;
    }

    // Do action for that frame. Called by FightScene every frame during a round.
    public void doAction() {
        // Take in input and update input history

        // 
    }

    // Deprecate
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f")) {
            print("f pressed on " + frame);
            StartCoroutine(neutral_high());
        }

        frame = frame + 1;
        if (currentHealth == 0){
            //end game
        }
        
        
        if (attack && isHit){
            damage = 9000;
            changeHealth(damage);
        }
        

    } 
}
