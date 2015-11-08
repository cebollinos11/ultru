using UnityEngine;
using System.Collections;

public class InteractionManager : Singleton<InteractionManager> {

    public LayerMask interactionLayer;
    public int maxInteractionDistance;
    public Interactable currentSelection;

    


	// Update is called once per frame
	void Update () {
        Interactable newSelection = GetCurrentScreenObject();

        if (newSelection != currentSelection) {
            if (currentSelection != null) {
                currentSelection.DoDeHighlight();
                currentSelection = null;
                //AudioController.PlayClip(AudioClipsType.OnSelect);

            }

            if (newSelection != null) {
                newSelection.DoHighlight();
                currentSelection = newSelection;
                //AudioController.PlayClip(AudioClipsType.OnDeselect);
            }

        }

            if (currentSelection != null) {
                if (Input.GetButtonDown("Interact")) {
                    Debug.Log("PRESSED ON MANAGER");
                    currentSelection.DoInteractButtonDown();
                    //AudioController.PlayClip(AudioClipsType.ButtonDown);
                }
                if (Input.GetButtonUp("Interact"))
                {
                    currentSelection.DoInteractButtonUp();
                    //AudioController.PlayClip(AudioClipsType.ButtonUp);
                }
            }
        
	
	}

    Interactable GetCurrentScreenObject() {
        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), 
            out hit, maxInteractionDistance, interactionLayer)) {
                if (currentSelection != null &&
                    currentSelection.gameObject == hit.collider.gameObject) {
                        return currentSelection;
                    }
                else {
                    return hit.collider.gameObject.GetComponent<Interactable>();
                }

        }
        else{
        return null;
        }

    }
}
