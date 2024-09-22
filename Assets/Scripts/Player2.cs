using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public int maxHealth = 200;
    public int currentHealth = 200;
    public int oldHealth;
    public bool block;
    public List<int> p2Hurtbox = new List<int>{5};
    public void changeHealth(int damage){
        oldHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //run animation for health change (oldHealth to currentHealth)
    }
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 200;
        currentHealth = 200;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
