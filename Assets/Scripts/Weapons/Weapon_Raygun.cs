using UnityEngine;
using System.Collections;

public class Weapon_Raygun : Weapon {

    [SerializeField] float _damage = 10;
    [SerializeField] int _ammoCostPerShot = 0;
    [SerializeField] float _fireRate = 2;
    [SerializeField] float _range = float.MaxValue;
    [SerializeField] Transform _shootOrigin;

    [SerializeField] float chargeTime = 2;

    float chargeTimeTotal = 0;


    // Use this for initialization
    void Start () {
        base.damage = _damage;
        base.ammoCostPerShot = _ammoCostPerShot;
        base.fireRate = _fireRate;
        base.range = _range;
        base.shootOrigin = _shootOrigin;

	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButton(0)) {
            chargeTimeTotal += Time.deltaTime;
            if (chargeTimeTotal > chargeTime) {
                chargeTimeTotal = chargeTime;
            }
        }
        else if (!Input.GetMouseButton(0) && chargeTimeTotal >= chargeTime) {
            
        }
	}

    new void Fire() {
        if (_shootOrigin == null) {
            Debug.LogError("The transform shootOrigin is not set, and " + GetType().Name + " can therefore not fire.");
            return;
        }
        base.Fire();
        chargeTimeTotal = 0;
        //Play shoot animation
        Ray ray = new Ray(_shootOrigin.position, _shootOrigin.forward);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, _range)) {
            Enemy enemy = hitinfo.transform.GetComponent<Enemy>();
            if (enemy != null) {
                Debug.Log("PEWPEW!");
            }
        }
    }
}
