using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public int maxHealth;
public int currentHealth;
int oldHealth;
public bool block;
public List<int> p1Pos = new List<int>{1,2,3,4,5,6};
public string p1Stance;
public List<List> inputs = new List<List>();


public void changeHealth(int damage){
    oldHealth = currentHealth;
    currentHealth -= damage;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    //run animation for health change (oldHealth to currentHealth)
}


public bool isHit(attack, position1, position2, block){

    return True
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
        if (currentHealth == 0){
            //end game
        }
        
        if (attack && isHit){
            damage = ##;
            changeHealth(damage);
        }

    }
}
