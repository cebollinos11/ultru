using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [SerializeField] float rayCastTimer = 3;
    [SerializeField] float viewRange = 20;

    NavMeshAgent navAgent;
    float rayCastTimerRemaining = 0;
    Vector3 lastKnownPlayerLocation;
    Player player;
    bool chasing;
    bool playerLOS;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        navAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (rayCastTimerRemaining <= 0) {
            CheckForPlayerLOS();
            rayCastTimerRemaining = rayCastTimer;
        }
        else {
            rayCastTimerRemaining -= Time.deltaTime;
        }
        if (chasing) {
            //navAgent.Move(lastKnownPlayerLocation - transform.position);
            navAgent.destination = lastKnownPlayerLocation;
        }
        //Debug.Log(lastKnownPlayerLocation);
	}

    void CheckForPlayerLOS() {
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, viewRange)) {
            if (hit.transform.tag == "Player") {
                lastKnownPlayerLocation = hit.transform.position;
                chasing = true;
                playerLOS = true;
            }
        }
    }

}
