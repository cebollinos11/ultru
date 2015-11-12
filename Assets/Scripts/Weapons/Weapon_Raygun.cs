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
    bool isFiring = false;
    LineRenderer lineRenderer;

    // Use this for initialization
    void Start () {
        base.damage = _damage;
        base.ammoCostPerShot = _ammoCostPerShot;
        base.fireRate = _fireRate;
        base.range = _range;
        base.shootOrigin = _shootOrigin;
        lineRenderer = GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (isFiring) {
            chargeTimeTotal += Time.deltaTime;
            if (chargeTimeTotal > chargeTime) {
                chargeTimeTotal = chargeTime;
            }
            Debug.Log("CHARGING MA LAZORZ " + chargeTimeTotal);
        }
        else if (!isFiring && chargeTimeTotal >= chargeTime) {
            Ray ray = new Ray(_shootOrigin.position, Camera.main.transform.forward);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, _range)) {
                Enemy enemy = hitinfo.transform.GetComponent<Enemy>();
                if (enemy != null) {
                    Debug.Log("PEWPEW!");
                }
                lineRenderer.SetVertexCount(2);
                lineRenderer.SetPosition(0, _shootOrigin.position);
                lineRenderer.SetPosition(1, hitinfo.point);
            }
            chargeTimeTotal = 0;
        }
        else if (!isFiring && chargeTimeTotal < chargeTime && chargeTimeTotal > 0) {
            Debug.Log("No pew. :(");
            chargeTimeTotal = 0;
        }
	}
    public override void Fire() {
        if (_shootOrigin == null) {
            Debug.LogError("The transform shootOrigin is not set, and " + GetType().Name + " can therefore not fire.");
            return;
        }
        base.Fire();
        isFiring = true;
        //Play shoot animation
        
    }

    public override void StopFiring() {
        isFiring = false;
    }
}
