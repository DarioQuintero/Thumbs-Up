using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Keybindings keybindings;

    void awake() {
        if (instance == null){
            instance = this;
        }
        else if (instance != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    
    public bool KeyDown(string key) {
        if (Input.GetKey(keybindings.checkKey(key))) {
            return true;
        }
        else {
            return false;
        }
    }
    
}
