using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] Transform hand;

    InteractionManager ioManager;
    GameController gameController;
    public Weapon.Weapons weaponInHand;
    Weapon equippedWeapon;
    LifeManager lifeManager;

	// Use this for initialization
	void Start () {

        lifeManager = GetComponent<LifeManager_Player>();
        ioManager = GetComponent<InteractionManager>();
        gameController = GameController.Instance;

        EquipWeapon(weaponInHand);

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: Change to mouse button
        if (Input.GetKey(KeyCode.R)) {
            if (equippedWeapon != null)
                equippedWeapon.Fire();
        }
        else if (Input.GetKeyUp(KeyCode.R)) {
            if (equippedWeapon != null)
                equippedWeapon.StopFiring();
        }
	}

    public void Hit(float damage) {
        Debug.Log("Player hit for " + damage + " dmg!");
        lifeManager.ReceiveDamage((int)damage);
    }

    public void EquipWeapon(Weapon.Weapons weaponToEquip) {
        if (weaponToEquip == Weapon.Weapons.Nothing) {
            Transform temp = hand.GetChild(0);
            if (temp != null) Destroy(temp.gameObject);
            equippedWeapon = null;
        }
        else {
            GameObject prefabToSpawn = gameController.weaponPrefabs[(int)weaponToEquip];
            GameObject newWeapon = Instantiate(prefabToSpawn, Vector3.zero, hand.rotation) as GameObject;
            newWeapon.transform.parent = hand;
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = hand.localRotation;

            switch ((int)weaponToEquip) {
                //Raygun
                case 0:
                    equippedWeapon = GetComponentInChildren<Weapon_Raygun>();
                    break;
                //Blaster
                case 1:
                    equippedWeapon = GetComponentInChildren<Weapon_Blaster>();
                    break;
            }
            equippedWeapon.teamToHit = GameController.Teams.Enemy;
        }

    }
}
