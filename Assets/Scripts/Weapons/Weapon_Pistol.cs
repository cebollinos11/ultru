using UnityEngine;
using System.Collections;

public class Weapon_Pistol : Weapon {

    [SerializeField] int _damage = 3;
    [SerializeField] int _ammoCostPerShot = 0;
    [SerializeField] float _fireRate = 2;
    [SerializeField] Transform[] _shootOrigin;
    [SerializeField] GameObject laserShotPrefab;
    [SerializeField] Color laserColor;
    AudioSource bulletSound;

    public bool autoAim = false;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        base.damage = _damage;
        base.ammoCostPerShot = _ammoCostPerShot;
        base.fireRate = _fireRate;
        base.shootOrigin = _shootOrigin;
        base.chargeTime = 0;
        bulletSound = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update() {
        if (fireRateCooldown > 0) {
            fireRateCooldown -= Time.deltaTime;
        }

        if (teamToHit == GameController.Teams.Player && autoAim) {
            transform.LookAt(player.transform);
        }

        if (isFiring || autoFire) {
            if (fireRateCooldown <= 0) {
                GameObject laser = Instantiate(laserShotPrefab, _shootOrigin[0].position, _shootOrigin[0].rotation) as GameObject;
                Blaster_Laser blaser = laser.GetComponent<Blaster_Laser>();
                blaser.Initialize(laserColor, damage, teamToHit);
                fireRateCooldown = fireRate;
                bulletSound.Play();
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
