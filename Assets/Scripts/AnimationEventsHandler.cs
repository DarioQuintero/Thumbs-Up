using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
    public GameObject instance;
    public bool triggererd;
    
    // Start is called before the first frame update
    void Start()
    {
        triggererd = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstanceVFX()
    {
        if(!triggererd)
        {
            instance.SetActive(true);
            triggererd = true;
        }
        
    }
    void SetSelfInactive()
    {
        gameObject.SetActive(false);
    }
    void ResetTrigger()
    {
        Debug.Log("Reset Trigger");
        triggererd = false;
    }

}
