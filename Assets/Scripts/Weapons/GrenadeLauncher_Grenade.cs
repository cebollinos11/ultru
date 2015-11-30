using UnityEngine;
using System.Collections;

public class GrenadeLauncher_Grenade : MonoBehaviour {

    [SerializeField] GameObject explosionEffect;
    
    GameController.Teams teamToHit;
    float explosionRadius = 5;
    int damage = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Initialize(float explosionRadius, int damage, GameController.Teams teamToHit) {
        this.explosionRadius = explosionRadius;
        this.damage = damage;
        this.teamToHit = teamToHit;
    }

    void OnTriggerEnter(Collider col) {
        if (col.isTrigger) return;
        Explode();
    }

    void Explode() {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in hits) {
            if (teamToHit == GameController.Teams.Player) {
                LifeManager_Player lifeMan = col.gameObject.GetComponent<LifeManager_Player>();
                if (lifeMan != null) {
                    lifeMan.ReceiveDamage(damage, transform.position);
                }
            }
            else if (teamToHit == GameController.Teams.Enemy) {
                LifeManager lifeMan = col.gameObject.GetComponent<LifeManager>();
                if (lifeMan != null) {
                    lifeMan.ReceiveDamage(damage, transform.position);
                }
            }
        }
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
