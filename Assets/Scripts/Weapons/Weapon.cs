using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public enum AmmoType {
        
    }

    public enum Weapons {
        Nothing = -1,
        Raygun = 0,
        Blaster = 1,
        GrenadeLauncher = 2,
        Pistol = 3
    }

    protected Transform[] shootOrigin;
    protected int ammoCostPerShot = 0;
    protected int damage = 0;
    protected float fireRate = 1;
    protected float fireRateCooldown = 0;
    protected float range = float.MaxValue;
    protected float chargeTime = 0;
    protected float chargeTotal = 0;
    protected bool autoFire = false;
    public bool isFiring = false;
    public GameController.Teams teamToHit;
    protected Player player;
    protected Animator animator;

    protected virtual void Start() {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

	public virtual void Fire() {
        //Subtract ammunition from player
    }

    public virtual void AutoFire() {
        if (isFiring) {
            return;
        }
        autoFire = true;
        
    }

    public virtual void StopFiring() {
        autoFire = false;
    }

    public virtual void Reload() {

    }



}
