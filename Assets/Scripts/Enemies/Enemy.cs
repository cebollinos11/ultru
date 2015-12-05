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
    [SerializeField] GameObject onHitEffect;
    [SerializeField] GameObject onDeathEffect;
    [SerializeField] GameObject ragdollPrefab;
    [SerializeField] float ragdollPushForce;

    protected float playerLocationUpdateTime = 0.2f;
    protected float viewRange = 100;
    protected int MAXHARDPOINTS = 0;
    protected NavMeshAgent navAgent;
    protected float rayCastTimerRemaining = 0.2f;
    protected Vector3 lastKnownPlayerLocation;
    protected Player player;
    [SerializeField] protected bool hasPlayerLOS = false;
    protected Vector3 originalLocation;
    protected Rigidbody rbody;
    [SerializeField]
    protected bool isShooting;
    protected enemySurfacePlacement placedOnSurface;
    protected LifeManager lifeManager;

    RaycastHit hit;

    float waitTime = 2;
    float waitTimeRemaining = 2;

    // Use this for initialization
    protected virtual void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        originalLocation = transform.position;
        rbody = GetComponent<Rigidbody>();
        lifeManager = GetComponent<LifeManager>();
        lifeManager.Init(maxHp);
        lifeManager.onDamage += Hit;
        lifeManager.onDeath += Death;
	}

    void OnDisable() {
        lifeManager.onDamage -= Hit;
        lifeManager.onDeath -= Death;
    }

    // Update is called once per frame
    protected virtual void Update () {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        waitTimeRemaining -= Time.deltaTime;
        if (waitTimeRemaining > 0)
            return;
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

    public virtual void Hit(Vector3 hitPos) {
        if (onHitEffect != null) {
            Instantiate(onHitEffect, hitPos, Quaternion.identity);
        }

    }

    public virtual void Death() {
        if (onDeathEffect != null) {
            Instantiate(onHitEffect, transform.position, Random.rotation);
        }
        if (ragdollPrefab != null) {
            Instantiate(onDeathEffect, transform.position, transform.rotation);
            GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation) as GameObject;
            //ragdoll.GetComponent<Rigidbody>().AddForce(Random.rotation.eulerAngles.normalized * ragdollPushForce, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }

    void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, hit.point);
        Gizmos.DrawSphere(hit.point, 1);
    }
    
}
