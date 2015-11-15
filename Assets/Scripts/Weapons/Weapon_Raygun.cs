using UnityEngine;
using System.Collections;

public class Weapon_Raygun : Weapon {

    [SerializeField] float _damage = 10;
    [SerializeField] int _ammoCostPerShot = 0;
    [SerializeField] float _fireRate = 2;
    [SerializeField] float _range = float.MaxValue;
    [SerializeField] Transform _shootOrigin;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] Color laserColor;
    [SerializeField] Transform chargeSphere;
    [SerializeField] float chargeSphereRotationSpeed = 0.2f;
    [SerializeField] float chargeSphereMaxSize = 2f;

    [SerializeField] float chargeTime = 2;

    float chargeTimeTotal = 0;
    bool isFiring = false;
    Material chargeMat;
    MeshRenderer meshRenderer;

    // Use this for initialization
    void Start () {
        base.damage = _damage;
        base.ammoCostPerShot = _ammoCostPerShot;
        base.fireRate = _fireRate;
        base.range = _range;
        base.shootOrigin = _shootOrigin;
        meshRenderer = chargeSphere.GetComponent<MeshRenderer>();
        chargeMat = meshRenderer.material;
    }
	
	// Update is called once per frame
	void Update () {
        meshRenderer.enabled = isFiring;
	    if (isFiring) {
            chargeTimeTotal += Time.deltaTime;
            float currentPercent = (chargeSphereMaxSize / chargeTime) * chargeTimeTotal;
            chargeMat.SetFloat("_LineWidth", currentPercent);
            //chargeSphere.localScale = new Vector3(currentPercent, currentPercent, currentPercent);
            float currentLerp = (1 / chargeTime) * chargeTimeTotal;
            chargeSphere.Rotate(Vector3.forward, chargeSphereRotationSpeed * Time.deltaTime);
            //chargeSphere.Rotate(new Vector3(chargeSphere.rotation.eulerAngles.x * chargeSphereRotationSpeed, 0, 0));
            if (chargeTimeTotal > chargeTime) {
                chargeTimeTotal = chargeTime;
            }
        }
        else if (!isFiring && chargeTimeTotal >= chargeTime) {
            Ray ray = new Ray(_shootOrigin.position, Camera.main.transform.forward);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, _range)) {
                Enemy enemy = hitinfo.transform.GetComponent<Enemy>();
                if (enemy != null) {
                }
            }
            chargeTimeTotal = 0;
            GameObject laser = Instantiate(laserPrefab, _shootOrigin.position, _shootOrigin.rotation) as GameObject;
            laser.GetComponent<Raygun_Laser>().Initialize(Vector3.zero, hitinfo.point, laserColor);
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
