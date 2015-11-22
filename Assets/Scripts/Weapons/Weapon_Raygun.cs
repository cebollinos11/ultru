using UnityEngine;
using System.Collections;

public class Weapon_Raygun : Weapon {

    [SerializeField] int _damage = 10;
    [SerializeField] int _ammoCostPerShot = 0;
    [SerializeField] float _fireRate = 2;
    [SerializeField] float _range = float.MaxValue;
    [SerializeField] Transform[] _shootOrigin;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] Color laserColor;
    [SerializeField] Transform chargeSphere;
    [SerializeField] float chargeSphereRotationSpeed = 0.2f;
    [SerializeField] float chargeSphereMaxSize = 2f;
    [SerializeField] float _chargeTime = 2;

    AudioSource chargeSound;
    
    Material chargeMat;
    MeshRenderer meshRenderer; 

    // Use this for initialization
    void Start () {
        base.damage = _damage;
        base.ammoCostPerShot = _ammoCostPerShot;
        base.fireRate = _fireRate;
        base.range = _range;
        base.shootOrigin = _shootOrigin;
        base.chargeTime = _chargeTime;
        meshRenderer = chargeSphere.GetComponent<MeshRenderer>();
        chargeMat = meshRenderer.material;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        chargeSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        meshRenderer.enabled = isFiring;
        if (autoFire && chargeTotal >= chargeTime) {
            StopFiring();
        }
	    if (isFiring) {
            if (chargeTotal == 0) {
                AudioSource.PlayClipAtPoint(chargeSound.clip,transform.position);
                //chargeSound.Play();
            }
            chargeTotal += Time.deltaTime;
            float currentPercent = (chargeSphereMaxSize / chargeTime) * chargeTotal;
            chargeMat.SetFloat("_LineWidth", currentPercent);
            chargeSphere.Rotate(Vector3.forward, chargeSphereRotationSpeed * Time.deltaTime);
            if (chargeTotal > chargeTime) {
                chargeTotal = chargeTime;
            }
        }
        else if (!isFiring && chargeTotal >= chargeTime) {
            Hitcheck();
            if (teamToHit == GameController.Teams.Player) {
                transform.LookAt(player.transform);
            }
        }
        else if (!isFiring && chargeTotal < chargeTime && chargeTotal > 0) {
            chargeTotal = 0;
            if (teamToHit == GameController.Teams.Player) {
                transform.LookAt(player.transform);
            }
        }
	}

    void Hitcheck() {
        Ray ray = new Ray(shootOrigin[0].position, shootOrigin[0].forward);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, range)) {
            if (teamToHit == GameController.Teams.Enemy) {
                Enemy enemy = hitinfo.transform.GetComponent<Enemy>();
                if (enemy != null) {
                    enemy.Hit(damage);
                }
            }
            else if (teamToHit == GameController.Teams.Player) {
                Player p = hitinfo.transform.GetComponent<Player>();
                if (p != null) {
                    p.Hit(damage);
                }
            }

        }
        chargeTotal = 0;
        GameObject laser = Instantiate(laserPrefab, shootOrigin[0].position, shootOrigin[0].rotation) as GameObject;
        laser.GetComponent<Raygun_Laser>().Initialize(Vector3.zero, hitinfo.point, laserColor);
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

    public override void AutoFire() {
        base.AutoFire();
        isFiring = true;
    }

    public override void StopFiring() {
        base.StopFiring();
        isFiring = false;
    }
}
