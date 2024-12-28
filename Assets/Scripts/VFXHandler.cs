using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VFXHandler : MonoBehaviour
{
    
    public List<GameObject> effects = new List<GameObject>();

    public Dictionary<string, GameObject> effectDict = new Dictionary<string, GameObject>();

    // Dictionary Key Convention: "Main Player Animation Name" + "." + "Opponnent Stance"(If needed)

    void Start(){
        for(int i = 0; i < effects.Count; i++){
            effectDict.Add(effects[i].name,effects[i]);
        }
    }

    public void CallEffect(string effectName, string opponentStance = "")
    {
        string key = effectName;

        if(!String.Equals(opponentStance,"")){
            key = String.Concat(key,".",opponentStance);
        }

        GameObject effect = effectDict[key];

        effect.SetActive(false);
        effect.SetActive(true);
        
    }
}
