using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    [SerializeField] Transform hand;
    [SerializeField] AudioClip weapon_Pickup;
    [SerializeField] Weapon.Weapons startWeapon;
    [SerializeField] GameObject theReveal;

    InteractionManager ioManager;
    GameController gameController;
    [HideInInspector] public Weapon.Weapons weaponInHand;
    Weapon equippedWeapon;
    LifeManager lifeManager;
    AudioSource audioSource;
    Animator animator;
    FirstPersonController fpsController;
    CharacterController charController;

    float MAXSTAMINA = 20;
    float currentStamina = 20;
    
    public bool canSprint = true;

	// Use this for initialization
	void Start () {
        weaponInHand = Weapon.Weapons.Nothing;
        lifeManager = GetComponent<LifeManager_Player>();
        ioManager = GetComponent<InteractionManager>();
        gameController = GameController.Instance;

        EquipWeapon(startWeapon);

        lifeManager.onDamage += Hit;
        lifeManager.onDeath += Death;
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
        fpsController = GetComponent<FirstPersonController>();
        charController = GetComponent<CharacterController>();
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


        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            currentStamina -= Time.deltaTime;
            canSprint = (currentStamina <= 0);
        }
    }

    public void Hit(Vector3 hitPos) {

    }

    public void Death() {

    }

    public void PickupWeapon(Weapon.Weapons weapon) {
        EquipWeapon(weapon);
        audioSource.PlayOneShot(weapon_Pickup, 0.8f);
    }

    public void EquipWeapon(Weapon.Weapons weaponToEquip) {
        if (weaponToEquip == weaponInHand) return;
        if (hand.childCount > 0) {
            Transform temp = hand.GetChild(0);
            if (temp != null) Destroy(temp.gameObject);
            equippedWeapon = null;
        }
        if (weaponToEquip == Weapon.Weapons.Nothing) {
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
                //Grenade Launcher
                case 2:
                    equippedWeapon = GetComponentInChildren<Weapon_GrenadeLauncher>();
                    break;
                case 3:
                    equippedWeapon = GetComponentInChildren<Weapon_Pistol>();
                    break;

            }
            equippedWeapon.teamToHit = GameController.Teams.Enemy;
            foreach (Transform t in newWeapon.GetComponentsInChildren<Transform>()) {
                t.gameObject.layer = LayerMask.NameToLayer("Weapon");
            }
            weaponInHand = weaponToEquip;
        }

    }

    public void PlayOutro() {
        Instantiate(theReveal, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y +180, transform.rotation.z));
        charController.enabled = false;
        fpsController.enabled = false;
        hand.gameObject.SetActive(false);
        GUIManager.Instance.gameObject.SetActive(false);
        animator.enabled = true;
        
        animator.SetTrigger("ShouldPan");
    }
}
