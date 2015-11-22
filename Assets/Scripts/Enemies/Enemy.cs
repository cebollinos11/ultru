using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public enum enemySurfacePlacement {
        Floor,
        Celling,
        Wall
    }

    [SerializeField] public struct TurretHardpoint {
        public Transform hardpoint;
        public Weapon.Weapons weaponOnHardpoint;
        public Weapon weapon;
        public float cooldownRemaining;
    }

    [SerializeField] protected GameObject[] weaponPrefabs;
    [SerializeField] protected int maxHp = 100;

    protected float playerLocationUpdateTime = 3;
    protected float viewRange = 100;

    protected NavMeshAgent navAgent;
    protected float rayCastTimerRemaining = 1;
    protected Vector3 lastKnownPlayerLocation;
    protected Player player;
    protected bool hasPlayerLOS;
    protected Vector3 originalLocation;
    protected Rigidbody rbody;
    protected bool isShooting;
    protected enemySurfacePlacement placedOnSurface;
    protected LifeManager lifeManager;

    RaycastHit hit;

    // Use this for initialization
    protected virtual void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        originalLocation = transform.position;
        rbody = GetComponent<Rigidbody>();
        lifeManager = GetComponent<LifeManager>();
        lifeManager.Init(maxHp);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
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
        int layerMask = 1 << 11;
        layerMask = ~layerMask;
        Ray ray = new Ray(transform.position, (player.transform.position - transform.position).normalized);
        //RaycastHit hit;
        if (Physics.Raycast(ray, out hit, viewRange, layerMask)) {
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

    void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, hit.point);
        Gizmos.DrawSphere(hit.point, 1);
    }
    
}
