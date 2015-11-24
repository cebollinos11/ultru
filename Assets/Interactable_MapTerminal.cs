using UnityEngine;
using System.Collections;

public class Interactable_MapTerminal : Interactable
{

    Camera minimapCamera;
   

    void Start()
    {
        minimapCamera = GameObject.Find("MinimapCamera").GetComponent<Camera>();
    }

    public override void DoHighlight()
    {
        base.DoHighlight();
        GUIManager.Instance.interactableLabelText.text = "Press E to activate Map Terminal";
    }
    public override void DoDeHighlight()
    {
        base.DoDeHighlight();
        GUIManager.Instance.interactableLabelText.text = "";
        minimapCamera.enabled = false;
    }
    public override void DoInteractButtonDown()
    {
        base.DoInteractButtonDown();
        minimapCamera.enabled = true;
        
    }
    public override void DoInteractButtonUp()
    {
        base.DoInteractButtonUp();
        minimapCamera.enabled = false;
    }

    

}

