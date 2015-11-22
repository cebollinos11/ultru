using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blaster_Laser : MonoBehaviour {
    
    [SerializeField] float moveSpeed;
    [SerializeField] ParticleSystem particleSystemPrefab;
    VolumetricLines.VolumetricLineBehavior lightStrip;

    Color laserColor;
    int damage;
    GameController.Teams teamToHit;
    List<ParticleSystem> particles;

    // Use this for initialization
    void Start () {
        particles = new List<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        
	}
    
    public void Initialize(Color laserColor, int damage, GameController.Teams teamToHit) {
        this.laserColor = laserColor;
        this.damage = damage;
        this.teamToHit = teamToHit;
        
    }

    void CheckHit(RaycastHit hit) {
        if (teamToHit == GameController.Teams.Player) {
            LifeManager_Player lifeMan = hit.transform.GetComponent<LifeManager_Player>();
            if (lifeMan != null) {
                lifeMan.ReceiveDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (teamToHit == GameController.Teams.Enemy) {
            LifeManager lifeMan = hit.transform.GetComponent<LifeManager>();
            if (lifeMan != null) {
                lifeMan.ReceiveDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.isTrigger) return;
        if (teamToHit == GameController.Teams.Player) {
            LifeManager_Player lifeMan = col.gameObject.GetComponent<LifeManager_Player>();
            if (lifeMan != null) {
                lifeMan.ReceiveDamage(damage);
            }
            else {
                SpawnParticles();
            }
        }
        else if (teamToHit == GameController.Teams.Enemy) {
            LifeManager lifeMan = col.gameObject.GetComponent<LifeManager>();
            if (lifeMan != null) {
                lifeMan.ReceiveDamage(damage);
            }
            else {
                SpawnParticles();
            }
        }
        Destroy(gameObject);

    }

    void SpawnParticles() {
        GameObject asd = Instantiate(particleSystemPrefab.gameObject, transform.position, transform.rotation) as GameObject;
        ParticleSystem ps = asd.GetComponent<ParticleSystem>();
        particles.Add(ps);
    }
    
}
