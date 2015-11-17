using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    InteractionManager ioManager;
    GameController gameController;
    Weapon equippedWeapon;
    LifeManager lifeManager;

	// Use this for initialization
	void Start () {

        lifeManager = GetComponent<LifeManager_Player>();
        ioManager = GetComponent<InteractionManager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        equippedWeapon = GetComponentInChildren<Weapon_Raygun>();
        equippedWeapon.teamToHit = GameController.Teams.Enemy;
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: Change to mouse button
        if (Input.GetKey(KeyCode.R)) {
            equippedWeapon.Fire();
        }
        else if (Input.GetKeyUp(KeyCode.R)) {
            equippedWeapon.StopFiring();
        }
	}

    public void Hit(float damage) {
        Debug.Log("Player hit for " + damage + " dmg!");
        lifeManager.ReceiveDamage((int)damage);
    }

}
