using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    // Keycodes for moves
    public KeyCode high, mid, left, right;

    public KeyCode checkKey(string key) {
        switch (key) {
            case "high":
                return high;
            case "mid":
                return mid;
            case "left":
                return left;
            case "right":
                return right;
            default:
                return KeyCode.None;
        }
    }
}
