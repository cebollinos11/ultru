using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    InteractionManager ioManager;
    GameController gameController;

	// Use this for initialization
	void Start () {
        ioManager = GetComponent<InteractionManager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.F)) {
            if (ioManager.currentSelection is Interactable_HackTerminal) {
                gameController.ExitHacked();
            }
        }
	}
}
