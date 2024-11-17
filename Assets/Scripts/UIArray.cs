using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArray : MonoBehaviour
{

    public UserInput userInputScript;

    public List<GameObject> UIElements = new List<GameObject>();

    public int currentUIElement = 0;

    // Start is called before the first frame update
    void Start()
    {
        setSelectedElement(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        if (userInputScript.upJustPressed) {
            currentUIElement--;
            if (currentUIElement < 0) {
                currentUIElement = UIElements.Count - 1;
            }
            setSelectedElement(currentUIElement);
        }
        if (userInputScript.downJustPressed) {
            currentUIElement++;
            if (currentUIElement >= UIElements.Count) {
                currentUIElement = 0;
            }
            setSelectedElement(currentUIElement);
        }
    }

    public void setSelectedElement(int i) {
        currentUIElement = i;
        for (int j = 0; j < UIElements.Count; j++) {
            UIElements[j].transform.Find("Pointer").gameObject.SetActive(false);
        }
        UIElements[i].transform.Find("Pointer").gameObject.SetActive(true);
    }
}
