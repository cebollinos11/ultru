using UnityEngine;
using System.Collections;

public class Weapon_Pickup : MonoBehaviour {

    [SerializeField] public Weapon.Weapons pickup = Weapon.Weapons.Raygun;
    [SerializeField] float rotationSpeed = 50;
    [SerializeField] Transform spawnLocation;

    GameObject weaponPrefab;
    GameObject weapon;

	// Use this for initialization
	void Start () {

        Initialise((Weapon.Weapons)Random.Range(0, 4));
        
	}
	
	// Update is called once per frame
	void Update () {
        spawnLocation.Rotate(transform.up, rotationSpeed * Time.deltaTime, Space.World);
	}

    public void PickUp() {

        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player != null)
        {
            player.PickupWeapon(pickup);
            Destroy(gameObject);
        }
    
    }

    

    public void Initialise(Weapon.Weapons pickup) {
        if (pickup == Weapon.Weapons.Nothing) 
            Destroy(gameObject);
        
        this.pickup = pickup;
        weaponPrefab = GameController.Instance.weaponPrefabs[(int)pickup];
        weapon = Instantiate(weaponPrefab, spawnLocation.position, Quaternion.identity) as GameObject;
        weapon.transform.parent = spawnLocation;
        weapon.transform.localPosition = new Vector3(0, 0, 0);
    }

}
