using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    // Keycodes for moves
    public KeyCode p1High, p1Mid, p1Forward, p1Backward, p2High, p2Mid, p2Forward, p2Backward;

    public KeyCode checkKey(string key) {
        switch (key) {
            case "p1High":
                return p1High;
            case "p1Mid":
                return p1Mid;
            case "p1Forward":
                return p1Forward;
            case "p1Backward":
                return p1Backward;
            case "p2High":
                return p2High;
            case "p2Mid":
                return p2Mid;
            case "p2Forward":
                return p2Forward;
            case "p2Backward":
                return p2Backward;
            default:
                return KeyCode.None;
        }
    }
}
