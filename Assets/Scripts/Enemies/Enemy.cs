using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public enum Weapons {
        Raygun = 0
    }

    [SerializeField] public struct TurretHardpoint {
        public Transform hardpoint;
        public Weapons weaponOnHardpoint;
        public Weapon weapon;
        public float cooldownRemaining;
    }

    [SerializeField] protected GameObject[] weaponPrefabs;
    
    protected float playerLocationUpdateTime = 3;
    protected float viewRange = 100;
    protected float minDistanceToPlayer = 0.5f;

    protected NavMeshAgent navAgent;
    protected float rayCastTimerRemaining = 0;
    protected Vector3 lastKnownPlayerLocation;
    protected Player player;
    protected bool hasPlayerLOS;
    protected Vector3 originalLocation;
    protected Rigidbody rbody;
    

    // Use this for initialization
    protected void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        originalLocation = transform.position;
        rbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	protected void Update () {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (rayCastTimerRemaining <= 0) {
            CheckForPlayerLOS();
            rayCastTimerRemaining = playerLocationUpdateTime;
        }
        else {
            rayCastTimerRemaining -= Time.deltaTime;
        }
        //Debug.Log(lastKnownPlayerLocation);
	}

    void CheckForPlayerLOS() {
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, viewRange)) {
            if (hit.transform.tag == "Player") {
                lastKnownPlayerLocation = hit.transform.position;
                hasPlayerLOS = true;
            }
            else {
                hasPlayerLOS = false;
            }
        }
    }

    protected void GoHome() {
        navAgent.destination = originalLocation;
    }

    public void Hit(float damage) {
        Debug.Log("Enemy was hit for " + damage + " dmg!");
    }
    
}
