using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

    public delegate void HighlightHandler();
    public delegate void DeHighlightHandler();
    public delegate void DoInteractButtonDownHandler();
    public delegate void DoInteractButtonUpHandler();

    public event HighlightHandler OnHighLight;
    public event HighlightHandler OnDeHighLight;
    public event HighlightHandler OnDoInteractButtonUp;
    public event HighlightHandler OnDoInteractButtonDown;

    public virtual void DoHighlight() {
        //GuiManager.DisplayInfoText("Selected " + gameObject.name);
        if(OnHighLight!=null)
        OnHighLight();
    }
    public virtual void DoDeHighlight() {
        if (OnDeHighLight != null)
        OnDeHighLight();
    }
    public virtual void DoInteractButtonDown() {
        Debug.Log("PRESS");
        if (OnDoInteractButtonUp != null)
        OnDoInteractButtonUp();

    }
    public virtual void DoInteractButtonUp() {
        Debug.Log("BUTTON UP");
        if (OnDoInteractButtonDown != null)
        OnDoInteractButtonDown();
    }
}
