using UnityEngine;
using System.Collections;

public class Interactable_HackTerminal : Interactable {
    
    void Start() {
        
    }

    public override void DoHighlight() {
        base.DoHighlight();
        GUIManager.Instance.interactableLabelText.text = "Press E to hack the terminal";
    }
    public override void DoDeHighlight() {
        base.DoDeHighlight();
        GUIManager.Instance.interactableLabelText.text = "";
    }
    public override void DoInteractButtonDown() {
        base.DoInteractButtonDown();
               
        GameController.Instance.ShutDown();
        
        disableTerminal();        
    }
    public override void DoInteractButtonUp() {
        base.DoInteractButtonUp();
    }

    private void disableTerminal(){

       
        for (int i = 1; i < transform.parent.childCount; i++)
        {
           
                transform.parent.GetChild(i).gameObject.layer = 0;
        }
    
    }
	
}
