using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : MonoBehaviour
{
    
    public List<GameObject> effects = new List<GameObject>();

    public Dictionary<string, GameObject> effectDict = new Dictionary<string, GameObject>();

    void Start(){
        for(int i = 0; i < effects.Count; i++){
            effectDict.Add(effects[i].name,effects[i]);
            
            
        }
    }

    public void CallEffect(string effectName)
    {
        GameObject effect = effectDict[effectName];
        effect.SetActive(false);
        effect.SetActive(true);
        //StartCoroutine(PlayEffect(effect));
    }

    IEnumerator PlayEffect(GameObject effect){
        yield return new WaitForSeconds(1);
        effect.SetActive(false);
    }
}
