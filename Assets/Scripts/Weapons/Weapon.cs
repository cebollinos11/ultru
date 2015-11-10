using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public enum AmmoType {
        
    }

    protected Transform shootOrigin;
    protected int ammoCostPerShot = 0;
    protected float damage = 0;
    protected float fireRate = 1;
    protected float range = float.MaxValue;

	public void Fire() {
        //Subtract ammunition from player
    }

    public void Reload() {

    }


}
