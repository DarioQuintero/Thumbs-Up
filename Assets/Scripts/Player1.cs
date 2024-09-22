using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    // Methods and variables need to be in class    
      
    public int maxHealth; // Deprecate
    public int currentHealth;
    public int oldHealth;
    public List<int> p1Hurtbox = new List<int>{2};
    public string p1Stance; // Position. ("backward", "neutral", "forward")
    public List<bool> inputHistory = new List<bool>(); 
    public string currentAction = "actionable";
    public int currentFrameCount = 0;

    public void getHit(int damage, bool wasBlocked, int stunFrames){
        // Take damage
        oldHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Take hitstun/blockstun
        /*
        if wasBlocked
        action = ["blockstun", stunFrames]
        else
        action = ["hitstun", stunFrames]
        */

        //run animation for health change (oldHealth to currentHealth)
        // query fightScene

    }

    private void forwardThrowAttack() {
        int attackStartup = 15;
        int damage = 50;
        int hitstun = 45; //since it is a +0 move on hit, then this would be same as recovery
        // int anim //TODO: would need to change the type of this
        int attackRecovery = 45;
        // int attack // Deprecate
        int List hitbox = [4]; //only hits forward position
        int List extendedHurtbox = [3, 4, 5, 6];
        int List oldHurtbox = [3]; //move can only be thrown in forward state

        //play attack animation
        switch (state[1]){
            //state[1] should be how many frames into the action the player is
            case 0:
                playAttackAnim();

            case < (attackStartup - 1):
                continue; 
            
            case attackStartup:
                if(hitbox.contains(Player2.p2Hurtbox) && Player2.p2Stance != "neutral" && Player2.p2Stance != "backward")
                {   //Values are at the beginning of the function
                    Player2.getHit(damage, hitstun, anim);
                }
            case < (attackStartup + attackRecovery):
            //Attacking player hurtbox extended during recovery frames
                p1Hurtbox = extendedHurtbox;
                continue;
            case (attackStartup + attackRecovery):
            //Attacking player hurtbox resets to previous hurtbox
                p1Hurtbox = oldHurtbox;
                state = ["actionable", 0];
            default:
                console.log("DEFAULT CASE IS RUNNING IN THROW");
        } 
    }

    public void neutralThrowAttack(){
        int attackStartup = 15;
        int damage = 30;
        int hitstun = 20; //same as recovery, as it is +0 on hit 
        int anim //TODO: would need to change the type of this
        int attackRecovery = 20;
        int attack
        int List hitbox = [5]; //only hits neutral position of opponent, would this be 2?
        int List extendedHurtbox = [2, 3, 4, 5]; //TODO: ask if this is how to do the extension
        int List oldHurtbox = [2];
        //play attack animation
        switch (state[1])
        {
            //state[1] should be how many frames into the action the player is
            case 0:
                playAttackAnim();

            case < (attackStartup - 1):
                continue; 
            
            case attackStartup:
                if(hitbox.contains(Player2.p2Hurtbox) && Player2.p2Hurtbox != "forward" && Player2.p2Stance != "backward")
                {   //Values are at the beginning of the function
                    Player2.getHit(damage, hitstun, anim);
                }
            case < (attackStartup + attackRecovery):
                p1Hurtbox = extendedHurtbox;
                continue;
            case (attackStartup + attackRecovery):
                p1Hurtbox = oldHurtbox;
                state = ["actionable", 0];
            default:
                console.log("DEFAULT CASE IS RUNNING IN THROW");
        } 
    }

    private void neutralHighAttack() {
        p1Status = "charging"; // deprecate
        int attackStartup = 10;
        int attackRecover = 12;
        int damage = 10;

        int attackPressed = frame;
        int startFrame = frame + attackStartup;

        while (frame < startFrame) {
            yield return null;
        }
        print("Neutral high executed on " + frame);
        if (true){
            player2.changeHealth(damage);
        }
        p1Status = "recovering";
        int recovered = frame + attackRecover;
        while (frame < recovered) {
            yield return null;
        }
        p1Status = "active";

    }
    
    // TODO: forwardHighAttack()

    private void neutralMidAttack(int pos2, bool block){
        int current = frame;
        int attackStartup = 10;
        int startFrame = current + attackStartup;
        List<int> hitbox = new List<int>{4};
        if (frame == startFrame){
            if (hitbox.Contains(pos2)){
                if (block == false){
                    int damage = 20;
                    //p2.changeHealth(damage); //add p2 character
                    //p2.stun(); //add stun function 
                }
                else{
                    //run block frame loss
                }
            }
            else{
                //run whiff frame loss
            }
        }
        int recovery = frame + 12;
        while (frame < recovery){
            continue;
        }
    }

    private void forwardMidAttack(int pos2, bool block){
        int current = frame;
        int attackStartup = 16;
        int startFrame = current + attackStartup;
        List<int> hitbox = new List<int>{4,5};
        if (frame == startFrame){
            if (hitbox.Contains(pos2)){
                if (block == false){
                    //damage = 20;
                    //p2.changeHealth(20); //add p2 character
                    //p2.stun(); //add stun function 
                }
                else{
                    //run block frame loss
                }
            }
            else{
                //run whiff frame loss
            }
        }
        int recovery = frame + 24;
        while (frame < recovery){
            continue;
        }
    }

    // Deprecate
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    // Resets player to the start of round position. Called at the start of every round.
    public void reset() {
        // Reset properties of player
        currentHealth = maxHealth;
        public int oldHealth;
        public bool block; // Deprecate
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
        
        /*
        if (attack && isHit){
            damage = 9000;
            changeHealth(damage);
        }
        */

    } 
}
