using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    // Keycodes for moves
    public KeyCode p1High, p1Mid, p1Block, p1Right, p1Left, p2High, p2Mid, p2Block, p2Left, p2Right;

    public KeyCode checkKey(string key) {
        switch (key) {
            case "p1High":
                return p1High;
            case "p1Mid":
                return p1Mid;
            case "p1Block":
                return p1Block;
            case "p1Left":
                return p1Left;
            case "p1Right":
                return p1Right;
            case "p2High":
                return p2High;
            case "p2Mid":
                return p2Mid;
            case "p2Block":
                return p2Block;
            case "p2Left":
                return p2Left;
            case "p2Right":
                return p2Right;
            default:
                return KeyCode.None;
        }
    }
}
