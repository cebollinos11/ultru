using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    InteractionManager ioManager;
    GameController gameController;
    Weapon equippedWeapon;

	// Use this for initialization
	void Start () {
        ioManager = GetComponent<InteractionManager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        equippedWeapon = GetComponentInChildren<Weapon_Raygun>();
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: Change to mouse button
        if (Input.GetKey(KeyCode.R)) {
            equippedWeapon.Fire();
        }
        else {
            equippedWeapon.StopFiring();
        }
	}

}
