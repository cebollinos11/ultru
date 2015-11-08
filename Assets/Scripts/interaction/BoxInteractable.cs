using UnityEngine;
using System.Collections;

public class BoxInteractable : Interactable {

    public Material highlightMaterial;

    Material originalMaterial;
    Renderer myRenderer;
    Vector3 originalScale;

    void Start() {
        
        myRenderer = GetComponent<Renderer>();
        originalMaterial = myRenderer.sharedMaterial; //use shared!! very importante!
        originalScale = transform.localScale;
    }

    public override void DoHighlight() {
        base.DoHighlight();
        myRenderer.sharedMaterial = highlightMaterial;
        
    }
    public override void DoDeHighlight() {
        base.DoDeHighlight();
        myRenderer.sharedMaterial = originalMaterial;
    }
    public override void DoInteractButtonDown() {

        base.DoInteractButtonDown();
        transform.localScale = originalScale * 0.9f;
        
    }
    public override void DoInteractButtonUp() {
        base.DoInteractButtonUp();
        transform.localScale = originalScale;
    }
	
}
