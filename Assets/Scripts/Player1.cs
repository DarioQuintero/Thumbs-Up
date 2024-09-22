using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    // Methods and variables need to be in class    
      
    public int maxHealth;
    public int currentHealth;
    public int oldHealth;
    public bool block;
    public List<int> p1Pos = new List<int>{1,2,3,4,5,6};
    public List<int> p2Pos = new List<int>{4,5,6};
    public string p1Stance;
    public List<bool> inputs = new List<bool>();
    public int frame = 0;

    public List<int> p1Hurtbox = new List<int>{2};

    public string p1Status = "active"; //Player1's status: active, charging, recovered, etc
   
    //public GameObject p2;
    public Player2 player2;

    public void changeHealth(int damage){
        oldHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //run animation for health change (oldHealth to currentHealth)
    }

    public bool isHit(int attack, int position1, int position2, int block){

        return true;
    }

    public void stun(){

    }

    public void forwardThrowAttack()
{
    int attackStartup = 15;
    int damage = 50;
    int hitstun = 45; //since it is a +0 move on hit, then this would be same as recovery
    int anim //TODO: would need to change the type of this
    int attackRecovery = 45;
    int attack
    int List hitbox = [4]; //only hits forward position
    int List hurtbox_extended = [2, 3, 4, 5]; //TODO: ask if this is how to do the extension

    //play attack animation
    switch state[1]
    {
        //state[1] should be how many frames into the action the player is
        case 0:
            playAttackAnim();

        case state[1] < (attackStartup - 1):
            continue; 
        
        case (attackStartup):
            if(hitbox.contains(Opponent.position) && Opponent.stance != "neutral" && Opponent.stance != "backward")
            {   //Values are at the beginning of the function
                Opponent.getHit(damage, hitstun, anim);
            }
        case (state[1] < (attackStartup + attackRecovery)):
            continue;
        case (attackStartup + attackRecovery):
            state = ["actionable", 0];
        default:
            console.log("DEFAULT CASE IS RUNNING IN THROW");
    } 
}

public void neutralThrowAttack() 

{
    int attackStartup = 15;
    int damage = 30;
    int hitstun = 20; //same as recovery, as it is +0 on hit 
    int anim //TODO: would need to change the type of this
    int attackRecovery = 20;
    int attack
    int List hitbox = [5]; //only hits neutral position of opponent, would this be 2?
    int List hurtbox_extended = [2, 3, 4, 5]; //TODO: ask if this is how to do the extension

    //play attack animation
    switch state[1]
    {
        //state[1] should be how many frames into the action the player is
        case 0:
            playAttackAnim();

        case state[1] < (attackStartup - 1):
            continue; 
        
        case (attackStartup):
            if(hitbox.contains(Opponent.position) && Opponent.stance != "forward" && Opponent.stance != "backward")
            {   //Values are at the beginning of the function
                Opponent.getHit(damage, hitstun, anim);
            }
        case (state[1] < (attackStartup + attackRecovery)):
            continue;
        case (attackStartup + attackRecovery):
            state = ["actionable", 0];
        default:
            console.log("DEFAULT CASE IS RUNNING IN THROW");
    } 
}

    IEnumerator neutral_high() {
        p1Status = "charging";
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

    public void neutral_mid(int pos2, bool block){
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

    public void forward_mid(int pos2, bool block){
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

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

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
