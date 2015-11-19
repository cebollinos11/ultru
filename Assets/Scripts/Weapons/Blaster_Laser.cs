using UnityEngine;
using System.Collections;

public class Blaster_Laser : MonoBehaviour {
    
    [SerializeField] float moveSpeed;
    VolumetricLines.VolumetricLineBehavior lightStrip;

    Color laserColor;
    int damage;
    GameController.Teams teamToHit;
    // Use this for initialization
    void Start () {
	
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

    void OnTriggerEnter(Collider col) {
        if (teamToHit == GameController.Teams.Player) {
            LifeManager_Player lifeMan = col.gameObject.GetComponent<LifeManager_Player>();
            if (lifeMan != null)
                lifeMan.ReceiveDamage(damage);
        }
        else if (teamToHit == GameController.Teams.Enemy) {
            LifeManager lifeMan = col.gameObject.GetComponent<LifeManager>();
            if (lifeMan != null)
                lifeMan.ReceiveDamage(damage);
        }
        Destroy(gameObject);

    }
}
