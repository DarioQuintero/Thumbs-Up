using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCounters : MonoBehaviour
{
    public GameObject p1Counter1;
    public GameObject p1Counter2;
    public GameObject p1Counter3;

    public GameObject p2Counter1;
    public GameObject p2Counter2;
    public GameObject p2Counter3;


    public void updateWinCounters(int p1Wins, int p2Wins) {
        p1Counter1.SetActive(p1Wins >= 1);
        p1Counter2.SetActive(p1Wins >= 2);
        p1Counter3.SetActive(p1Wins >= 3);

        p2Counter1.SetActive(p2Wins >= 1);
        p2Counter2.SetActive(p2Wins >= 2);
        p2Counter3.SetActive(p2Wins >= 3);

    }
    // Start is called before the first frame update
    void Start()
    {
        p1Counter1.SetActive(false);
        p1Counter2.SetActive(false);
        p1Counter3.SetActive(false);
        p2Counter1.SetActive(false);
        p2Counter2.SetActive(false);
        p2Counter3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
