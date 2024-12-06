using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    void FixedUpdate() {
        // if (userInputScript.upJustPressed) {
        //     previousUI();
        // }
        // if (userInputScript.downJustPressed) {
        //     nextUI();
        // }
        // if (userInputScript.confirmJustPressed) {
        //     UIElements[currentUIElement].GetComponent<Button>().onClick.Invoke(); 
        // }
    }

    public void setSelectedElement(int i) {
        currentUIElement = i;
        for (int j = 0; j < UIElements.Count; j++) {
            UIElements[j].transform.Find("Pointer").gameObject.SetActive(false);
            UIElements[j].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            
        }
        UIElements[i].transform.Find("Pointer").gameObject.SetActive(true);
        UIElements[i].GetComponent<Image>().color = new Color(0.95f, 0.95f, 0.6f, 0.5f);
        // ColorBlock cb = UIElements[i].GetComponent<Button>().colors;
        // cb.highlightedColor = Color.red;
        // UIElements[i].GetComponent<Button>().colors = cb;
    }

    public void previousUI() {
        currentUIElement--;
        if (currentUIElement < 0) {
            currentUIElement = UIElements.Count - 1;
        }
        setSelectedElement(currentUIElement);
    }

    public void nextUI() {
        currentUIElement++;
        if (currentUIElement >= UIElements.Count) {
            currentUIElement = 0;
        }
        setSelectedElement(currentUIElement);
    }

    public void confirm() {
        UIElements[currentUIElement].GetComponent<Button>().onClick.Invoke(); 
    }
}
