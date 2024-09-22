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

    IEnumerator neutral_high() {
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



    }

    public void neutral_mid(int pos2, bool block){
        int current = frame;
        int attackStartup = 10;
        int startFrame = current + attackStartup;
        List<int> hitbox = new List<int>{4};
        if (frame == startFrame){
            if (hitbox.Contains(pos2)){
                if (block == false){
                    // damage = 20;
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
