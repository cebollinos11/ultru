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
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0)) {
            equippedWeapon.Fire();
        }
        else {
            equippedWeapon.StopFiring();
        }
	}

}
