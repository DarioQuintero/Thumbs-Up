using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{
    public Image p1HealthBar;
    public float p1HealthAmount = 100f;

    public Image p2HealthBar;
    public float p2HealthAmount = 100f;

    // Update is called once per frame
    void Update()
    {
        int PLAYER_1 = 1;
        int PLAYER_2 = 2;
        if(Input.GetKeyDown(KeyCode.Return)){
            UpdateHealthBar(PLAYER_1, 10);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            UpdateHealthBar(PLAYER_2, 10);
        }
    }

    public void UpdateHealthBar(int player, float damage){
        
        if (player == 1){
            p1HealthAmount -= damage;
            p1HealthBar.fillAmount = p1HealthAmount / 100f;   
        }
        //player == 2
        else if (player == 2){
            p2HealthAmount -= damage;
            p2HealthBar.fillAmount = p2HealthAmount / 100f;  
        }
        else{
            print("ERROR: PLAYER IS NOT 1 OR 2 IN HEALTHBAR SCRIPT");
        }

        
    }
}

