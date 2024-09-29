using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public int maxHealth = 200;
    public int currentHealth = 200;
    public int oldHealth;
    public bool block;

    public string p2Stance; // Position. ("backward", "neutral", "forward")

    public List<int> p2Hurtbox = new List<int>{5};
    public void getHit(int damage, bool wasBlocked, int stunFrames){
        // Take damage
        oldHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // forces current health between 0 and maxHealth

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
        public bool isBlocking() {
            return false;
        }
    // Start is called before the first frame update
    /*
    public void forwardThrowAttack() {
        int attackStartup = 15;
        int damage = 50;
        int hitstun = 45; //since it is a +0 move on hit, then this would be same as recovery
        // int anim //TODO: would need to change the type of this
        int attackRecovery = 45;
        // int attack // Deprecate
        int List hitbox = [4]; //only hits forward position
        int List extendedHurtbox = [2, 3, 4, 5];

        //play attack animation
        switch (state[1]){
            //state[1] should be how many frames into the action the player is
            case 0:
                playAttackAnim();

            case < (attackStartup - 1):
                continue; 
            
            case attackStartup:
                if(hitbox.contains(Player1.p1Hurtbox) && Player1.p1Stance != "neutral" && Player1.p1Stance != "backward")
                {   //Values are at the beginning of the function
                    Player1.getHit(damage, hitstun, anim);
                }
            case < (attackStartup + attackRecovery):
                continue;
            case (attackStartup + attackRecovery):
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
        int List hurtbox_extended = [2, 3, 4, 5]; //TODO: ask if this is how to do the extension

        //play attack animation
        switch (state[1])
        {
            //state[1] should be how many frames into the action the player is
            case 0:
                playAttackAnim();

            case < (attackStartup - 1):
                continue; 
            
            case attackStartup:
                if(hitbox.contains(Player1.p2Hurtbox) && Player1.p2Hurtbox != "forward" && Player1.p1Stance != "backward")
                {   //Values are at the beginning of the function
                    Player1.getHit(damage, hitstun, anim);
                }
            case < (attackStartup + attackRecovery)):
                continue;
            case (attackStartup + attackRecovery):
                state = ["actionable", 0];
            default:
                console.log("DEFAULT CASE IS RUNNING IN THROW");
        } 
    }
    */

    public void reset(){

    }
    void Start()
    {
        maxHealth = 200;
        currentHealth = 200;
    }


}
