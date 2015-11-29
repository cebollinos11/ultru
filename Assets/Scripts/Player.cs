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

        lifeManager.onDamage += Hit;
        lifeManager.onDeath += Death;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
	}
	
    void OnDisable() {
        lifeManager.onDamage -= Hit;
        lifeManager.onDeath -= Death;
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

    public void Hit(Vector3 hitPos) {

    }

    public void Death() {

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
            foreach (Transform t in newWeapon.GetComponentsInChildren<Transform>()) {
                t.gameObject.layer = LayerMask.NameToLayer("Weapon");
            }
        }

    }
}
