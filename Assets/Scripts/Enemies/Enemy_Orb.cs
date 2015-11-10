using UnityEngine;
using System.Collections;

public class Enemy_Orb : Enemy {

    [SerializeField] float _playerLocationUpdateTime = 3;
    [SerializeField] float _viewRange = 20;
    [SerializeField] float _minDistanceToPlayer = 0.5f; 

    bool chasing;
    bool waiting;
    // Use this for initialization
    new void Start () {
        base.Start();
        base.playerLocationUpdateTime = _playerLocationUpdateTime;
        base.viewRange = _viewRange;
        base.minDistanceToPlayer = _minDistanceToPlayer;
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
        if (hasPlayerLOS && Mathf.Abs(Vector3.Distance(lastKnownPlayerLocation, transform.position)) > _minDistanceToPlayer && (!chasing || waiting)) {
            chasing = true;
            waiting = false;
            navAgent.Resume();
        }
        if (chasing) {
            if (Mathf.Abs(Vector3.Distance(lastKnownPlayerLocation, transform.position)) < _minDistanceToPlayer) {
                navAgent.Stop();
                waiting = true;
                rbody.velocity = Vector3.zero;
                navAgent.destination = transform.position;
            }
            else {
                navAgent.destination = lastKnownPlayerLocation;
            }
        }
        if (Mathf.Abs(Vector3.Distance(lastKnownPlayerLocation, transform.position)) < _minDistanceToPlayer && !hasPlayerLOS && chasing) {
            chasing = false;
            waiting = false;
            GoHome();
            navAgent.Resume();
        }
    }
}
