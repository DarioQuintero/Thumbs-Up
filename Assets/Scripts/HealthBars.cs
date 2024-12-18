using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{
    public Image p1HealthBar;
    public Image p1TrailingHealthBar;
    private Animator p1TrailAnim;
    private Animator p1HealthAnim;
    public float p1HealthAmount = 100f;

    public Image p2HealthBar;
    public Image p2TrailingHealthBar;
    private Animator p2TrailAnim;
    private Animator p2HealthAnim;
    public float p2HealthAmount = 100f;

    Coroutine trailingHealth;


    struct HealthBar{
        public Image image;
        public float currentHealth;
        public Animator anim;
        public HealthBar(Image img, float health, Animator animator)
        {
            image = img;
            currentHealth = health;
            anim = animator;
        }
    }

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

    void Start()
    {
        p1TrailAnim = p1TrailingHealthBar.gameObject.GetComponent<Animator>();
        p2TrailAnim = p2TrailingHealthBar.gameObject.GetComponent<Animator>();
        p1HealthAnim = p1HealthBar.gameObject.GetComponent<Animator>();
        p2HealthAnim = p2HealthBar.gameObject.GetComponent<Animator>();
    }
    public void setHealthBar(int player, float currentHealth){

        if (player == 1){
            //p1HealthAmount = currentHealth;
            p1HealthBar.fillAmount = currentHealth / 100f;   
            p1HealthAnim.SetTrigger("Hit");
            triggerTrailingHealthBar(p1TrailingHealthBar, currentHealth, p1TrailAnim);
        }
        //player == 2
        else if (player == 2){
            //p2HealthAmount = currentHealth;
            p2HealthBar.fillAmount = currentHealth / 100f;  
            p2HealthAnim.SetTrigger("Hit");
            triggerTrailingHealthBar(p2TrailingHealthBar, currentHealth, p2TrailAnim);
        }
        else{
            print("ERROR: PLAYER IS NOT 1 OR 2 IN HEALTHBAR SCRIPT");
        }

        
    }

    void triggerTrailingHealthBar(Image trailingHealthBar, float currentHealth, Animator anim)
    {
        HealthBar healthBar = new HealthBar(trailingHealthBar,currentHealth, anim);
        
        StartCoroutine("updateTrail",healthBar);
    }

    IEnumerator updateTrail(HealthBar healthBar)
    {
        

        if(healthBar.image.fillAmount < healthBar.currentHealth/100f)
        {
            healthBar.image.fillAmount = healthBar.currentHealth/100f;
        }
        else{
            healthBar.anim.SetTrigger("Hit");
        }
        while(healthBar.image.fillAmount > healthBar.currentHealth/100f)
        {
            healthBar.image.fillAmount = Mathf.Lerp(healthBar.image.fillAmount,
                                                    healthBar.currentHealth/100f,
                                                    Time.deltaTime) - 0.05f*Time.deltaTime;
            yield return null;
        }

    }
}

