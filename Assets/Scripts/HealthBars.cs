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
    /*
    void Update()
    {
         ----- FOR TESTING -----
        int PLAYER_1 = 1;
        int PLAYER_2 = 2;
        if(Input.GetKeyDown(KeyCode.Return)){
            setHealthBar(PLAYER_1, p1HealthBar.fillAmount*100f - 10);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            setHealthBar(PLAYER_2, p2HealthBar.fillAmount*100f - 10);
        }
        
    }
    */

    public void setHealthBar(int player, float currentHealth){
        
        if (player == 1){
            p1HealthAmount = currentHealth;
            p1HealthBar.fillAmount = p1HealthAmount / 100f;   
        }
        //player == 2
        else if (player == 2){
            p2HealthAmount = currentHealth;
            p2HealthBar.fillAmount = p2HealthAmount / 100f;  
        }
        else{
            print("ERROR: PLAYER IS NOT 1 OR 2 IN HEALTHBAR SCRIPT");
        }

        
    }
}

