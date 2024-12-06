using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UGUIImage = UnityEngine.UI.Image;


public class GameOverPanelScript : MonoBehaviour
{

    public GameObject menuPanel;
    public GameObject rematchPanel;

    private int MENU_SELECTED = 0;
    private int REMATCH_SELECTED = 1;

    public int gameOverStateToSelect; //this can be "rematch" or "menu"

    // Start is called before the first frame update
    
    void Start()
    {
        UGUIImage menuPanelImage = menuPanel.GetComponent<UGUIImage>();
        UGUIImage rematchPanelImage = rematchPanel.GetComponent<UGUIImage>();
        gameOverStateToSelect = REMATCH_SELECTED; //this can be "rematch" or "menu"

        SelectPanel(rematchPanelImage);
        DeSelectPanel(menuPanelImage);

        //rematchPanelImage.color = new Color(0f, 0f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        UGUIImage menuPanelImage = menuPanel.GetComponent<UGUIImage>();
        UGUIImage rematchPanelImage = rematchPanel.GetComponent<UGUIImage>();

        print("WE ARE IN THE UPDATE");

        if (Input.GetKeyDown(KeyCode.A)){
            print("KEY PRESSED DOWN");
            if (gameOverStateToSelect == REMATCH_SELECTED){
                print("MENU SELECTED");

                SelectPanel(menuPanelImage);
                DeSelectPanel(rematchPanelImage);
                gameOverStateToSelect = MENU_SELECTED;
            }

            if (gameOverStateToSelect == MENU_SELECTED){
                print("REMATCH SELECTED");

                SelectPanel(rematchPanelImage);
                DeSelectPanel(menuPanelImage);
                gameOverStateToSelect = REMATCH_SELECTED;
            }

        }
    }

    void SelectPanel(UGUIImage imageToUpdate){
        Color selectColor = Color.blue;
        imageToUpdate.color = selectColor;
    }

    void DeSelectPanel(UGUIImage imageToUpdate){
        Color deselectColor = Color.white;
        imageToUpdate.color = deselectColor;
    }
}
