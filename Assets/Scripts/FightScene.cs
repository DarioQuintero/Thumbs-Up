using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : MonoBehaviour
{
    public int frame = 0;

    public int startFrame(int current, string attack){
        if (attack == "neutral mid"){
            return current + 10;
        }
        else if (attack == "forward mid"){
            return current + 16;
        }
        else if (attack == "neutral high"){
            return current + 10;
        }
        else if (attack == "forward high"){
            return current + 12;
        }
        else if (attack == "neutral throw"){
            return current + 15;
        } 
        else if (attack == "forward throw"){
            return current + 15;
        } 
        else{
            return 0
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frame = frame + 1;
    }
}
