using UnityEngine;
using System.Collections;

public class Interactable_HackTerminal : Interactable {
    
    void Start() {
        
    }

    public override void DoHighlight() {
        base.DoHighlight();
    }
    public override void DoDeHighlight() {
        base.DoDeHighlight();
    }
    public override void DoInteractButtonDown() {
        base.DoInteractButtonDown();

        Debug.Log("SHIT MAN");
        GameController.Instance.ShutDown();

        
    }
    public override void DoInteractButtonUp() {
        base.DoInteractButtonUp();
    }
	
}
