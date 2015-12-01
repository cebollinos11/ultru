using UnityEngine;
using System.Collections;

public class Weapon_Blaster : Weapon {

    [SerializeField] int _damage = 3;
    [SerializeField] int _ammoCostPerShot = 0;
    [SerializeField] float _fireRate = 2;
    [SerializeField] Transform[] _shootOrigin;
    [SerializeField] GameObject laserShotPrefab;
    [SerializeField] Color laserColor;
    AudioSource bulletSound;

    public bool autoAim = false;

    struct GunBarrel {
        public Transform barrelEnd;
        public float cooldownRemaining;
    }

    GunBarrel[] barrels;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        base.damage = _damage;
        base.ammoCostPerShot = _ammoCostPerShot;
        base.fireRate = _fireRate;
        base.shootOrigin = _shootOrigin;
        base.chargeTime = 0;
        GunBarrel barrel1 = new GunBarrel();
        barrel1.barrelEnd = shootOrigin[0];
        barrel1.cooldownRemaining = 0;
        GunBarrel barrel2 = new GunBarrel();
        barrel2.barrelEnd = shootOrigin[1];
        barrel2.cooldownRemaining = fireRate/2;
        barrels = new GunBarrel[] {barrel1, barrel2};

        bulletSound = GetComponent<AudioSource>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!(barrels[0].cooldownRemaining <= 0) && !(barrels[1].cooldownRemaining <= 0)) {
            barrels[0].cooldownRemaining -= Time.deltaTime;
            barrels[1].cooldownRemaining -= Time.deltaTime;
        }

        if (teamToHit == GameController.Teams.Player && autoAim) {
            transform.LookAt(player.transform);
        }

        if (isFiring || autoFire) {
            for (int i = 0; i < barrels.Length; i++) {
                if (barrels[i].cooldownRemaining <= 0) {
                    GameObject laser = Instantiate(laserShotPrefab, barrels[i].barrelEnd.position, barrels[i].barrelEnd.rotation) as GameObject;
                    Blaster_Laser blaser = laser.GetComponent<Blaster_Laser>();
                    blaser.Initialize(laserColor, damage, teamToHit);
                    barrels[i].cooldownRemaining = fireRate;
                    //bulletSound.PlayOneShot(bulletSound.clip);
                    bulletSound.Play();
                }
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
