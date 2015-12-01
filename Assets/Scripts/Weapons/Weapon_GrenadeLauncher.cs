using UnityEngine;
using System.Collections;

public class Weapon_GrenadeLauncher : Weapon {

    [SerializeField] int _damage = 3;
    [SerializeField] int _ammoCostPerShot = 0;
    [SerializeField] float _fireRate = 2;
    [SerializeField] float explosionRadius = 5;
    [SerializeField] float grenadePushForce = 100;
    [SerializeField] float shootDelay = 0.3f;
    [SerializeField] Transform[] _shootOrigin;
    AudioSource bulletSound;

    [SerializeField] GameObject grenadePrefab;

    float shootDelayRemaining = 0;
    bool waitForFire = false;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        base.damage = _damage;
        base.ammoCostPerShot = _ammoCostPerShot;
        base.fireRate = _fireRate;
        base.shootOrigin = _shootOrigin;
        base.chargeTime = 0;
        bulletSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (fireRateCooldown > 0) {
            fireRateCooldown -= Time.deltaTime;
        }

        if ((isFiring || autoFire) && fireRateCooldown <= 0) {
            waitForFire = true;
            animator.SetTrigger("Shoot");
        }
        if (waitForFire) {
            if (shootDelayRemaining < shootDelay)
                shootDelayRemaining += Time.deltaTime;
            if (shootDelayRemaining >= shootDelay) {
                waitForFire = false;
                fireRateCooldown = fireRate;
                shootDelayRemaining = 0;
                GameObject grenade = Instantiate(grenadePrefab, shootOrigin[0].position, transform.rotation) as GameObject;
                GrenadeLauncher_Grenade nade = grenade.GetComponent<GrenadeLauncher_Grenade>();
                nade.Initialize(explosionRadius, damage, teamToHit);
                nade.GetComponent<Rigidbody>().AddForce(nade.transform.forward * grenadePushForce, ForceMode.Impulse);
            }
        }
    }

    public override void Fire() {
        base.Fire();
        isFiring = true;
    }

    public override void AutoFire() {
        base.AutoFire();
    }

    public override void StopFiring() {
        base.StopFiring();
        isFiring = false;
    }
}
