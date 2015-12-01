using UnityEngine;
using System.Collections;

public class Interactable_Elevator : Interactable
{

    ElevatorManager eM;

    void Start()
    {
        eM = GetComponent<ElevatorManager>();
    }

    public override void DoHighlight()
    {
        base.DoHighlight();
        if (eM.isActive)
        {
            GUIManager.Instance.interactableLabelText.text = "Press E to use elevator";
        }
        else {
            GUIManager.Instance.interactableLabelText.text = "The elevator cant be used now";
        }
        
    }
    public override void DoDeHighlight()
    {
        base.DoDeHighlight();
        GUIManager.Instance.interactableLabelText.text = "";
    }
    public override void DoInteractButtonDown()
    {
        base.DoInteractButtonDown();
        if (eM.isActive)
        {
            GUIManager.Instance.interactableLabelText.text = "You successfully escaped!";
            Invoke("WinGame",2f);
        }

        else{
            GUIManager.Instance.interactableLabelText.text = "Nothing happens";
        }
    }


    public override void DoInteractButtonUp()
    {
        base.DoInteractButtonUp();
    }

    void WinGame() {
        LevelManager.Instance.lvl++;
        LevelManager.Instance.GoToMainMenu();
    }

    

}
