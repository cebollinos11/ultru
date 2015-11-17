using UnityEngine;
using System.Collections;

public class Enemy_Orb : Enemy {

    [SerializeField] Transform[] _turretHardpoints;
    [SerializeField] Weapons[] _weaponsOnHardpoints;
    [SerializeField] float _playerLocationUpdateTime = 3;
    [SerializeField] float _viewRange = 100;
    [SerializeField] float _minDistanceToPlayer = 0.5f;

    TurretHardpoint[] hardpoints;
    bool chasing;
    bool waiting;
    bool shooting;
    // Use this for initialization
    new void Start () {
        base.Start();
        base.playerLocationUpdateTime = _playerLocationUpdateTime;
        base.viewRange = _viewRange;
        base.minDistanceToPlayer = _minDistanceToPlayer;
        hardpoints = new TurretHardpoint[_turretHardpoints.Length];

        //For testing. Should be run by MapGenerator.
        SpawnTurrets(4);
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
        if (!hasPlayerLOS) {
            shooting = false;
        }
        else if (hasPlayerLOS && !shooting) { 
            transform.LookAt(player.transform);
        }
        if (hasPlayerLOS && Mathf.Abs(Vector3.Distance(lastKnownPlayerLocation, transform.position)) > _minDistanceToPlayer && (!chasing || waiting)) {
            chasing = true;
            waiting = false;
            navAgent.Resume();
        }
        if (chasing) {

            if (hasPlayerLOS && !shooting) //added by pablo
            {
                shooting = true;
            }

            if (Mathf.Abs(Vector3.Distance(lastKnownPlayerLocation, transform.position)) < _minDistanceToPlayer) {
                navAgent.Stop();
                waiting = true;
                rbody.velocity = Vector3.zero;
                navAgent.destination = transform.position;
                //if (hasPlayerLOS && !shooting) {  //commented by pablo
                //    shooting = true;
                //}
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

        if (shooting) {
            navAgent.Stop();
            foreach (TurretHardpoint t in hardpoints) {
                t.weapon.AutoFire();
            }
        }
        else {
            navAgent.Resume();
            foreach (TurretHardpoint t in hardpoints) {
                if (t.weapon.isFiring) {
                    t.weapon.StopFiring();
                }
            }
        }
    }

    public void SpawnTurrets(int nrOfGuns) {
        if (nrOfGuns > _turretHardpoints.Length) nrOfGuns = _turretHardpoints.Length;
        for (int i = 0; i < nrOfGuns; i++) {
            TurretHardpoint point = hardpoints[i];
            point.hardpoint = _turretHardpoints[i];
            point.weaponOnHardpoint = _weaponsOnHardpoints[i];

            GameObject newTurret = Instantiate(weaponPrefabs[(int)point.weaponOnHardpoint], point.hardpoint.position, point.hardpoint.rotation) as GameObject;
            point.weapon = newTurret.GetComponent<Weapon>();
            point.weapon.teamToHit = GameController.Teams.Player;
            newTurret.transform.SetParent(point.hardpoint, false);
            newTurret.transform.localPosition = Vector3.zero;
            hardpoints[i] = point;
            
        }
    }
}
