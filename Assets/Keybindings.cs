using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    // Keycodes for moves
    public KeyCode p1High, p1Mid, p1Left, p1Right, p2High, p2Mid, p2Left, p2Right;

    public KeyCode checkKey(string key) {
        switch (key) {
            case "p1High":
                return p1High;
            case "p1Mid":
                return p1Mid;
            case "p1Left":
                return p1Left;
            case "p1Right":
                return p1Right;
            case "p2High":
                return p2High;
            case "p2Mid":
                return p2Mid;
            case "p2Left":
                return p2Left;
            case "p2Right":
                return p2Right;
            default:
                return KeyCode.None;
        }
    }
}
