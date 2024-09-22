using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public int maxHealth;
public int currentHealth;
int oldHealth;
public bool block;
public List<int> p1Pos = new List<int>{1,2,3,4,5,6};
public List<int> p2Pos = new List<int>{4,5,6};
public string p1Stance;
public List<List> inputs = new List<List>();
public int frame = 0;

public void changeHealth(int damage){
    oldHealth = currentHealth;
    currentHealth -= damage;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    //run animation for health change (oldHealth to currentHealth)
}


public bool isHit(attack, position1, position2, block){

    return True
}

public void stun(){

}

public void nuetral_mid(pos2, block){
    int current = frame;
    int attackStartup = 10;
    int startFrame = current + attackStartup;
    List<int> hitbox = new List<int>{4};
    if (frame == startFrame){
        if (hitbox.contains(pos2)){
            if (block != True){
                damage = 20
                p2.changeHealth(20) //add p2 character
                p2.stun() //add stun function 
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
        continue
    }
}

public void forward_mid(pos2, block){
    int current = frame;
    int attackStartup = 16;
    int startFrame = current + attackStartup;
    List<int> hitbox = new List<int>{4,5};
    if (frame == startFrame){
        if (hitbox.contains(pos2)){
            if (block != True){
                damage = 20
                p2.changeHealth(20) //add p2 character
                p2.stun() //add stun function 
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
        continue
    }
}

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100
        currentHealth = maxHealth
    }

    // Update is called once per frame
    void Update()
    {
        frame = frame + 1;
        if (currentHealth == 0){
            //end game
        }
        
        if (attack && isHit){
            damage = ##;
            changeHealth(damage);
        }

    } 
}
