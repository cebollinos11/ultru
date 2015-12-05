using UnityEngine;
using System.Collections;

public class Enemy_Orb : Enemy {

    [SerializeField] Transform[] _turretHardpoints;
    [SerializeField] float _playerLocationUpdateTime;
    [SerializeField] float _viewRange = 100;

    [SerializeField] float minDistanceToPlayer = 0.5f;

    TurretHardpoint[] hardpoints;
    [SerializeField]bool chasing;
    [SerializeField]
    bool waiting;
    
    //For testing - To remove
    [SerializeField] Weapon.Weapons[] _weaponsOnHardpoints;
    
    // Use this for initialization
    protected override void Start () {
        base.Start();
        base.playerLocationUpdateTime = _playerLocationUpdateTime;
        base.viewRange = _viewRange;
        hardpoints = new TurretHardpoint[_turretHardpoints.Length];
        MAXHARDPOINTS = 4;

        //For testing. Should be run by MapGenerator.
        SpawnTurrets(4, _weaponsOnHardpoints);

        lastKnownPlayerLocation = transform.position;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if (!hasPlayerLOS) {
            chasing = true;
            isShooting = false;
        }
        else if (hasPlayerLOS && !isShooting) {
            transform.LookAt(player.transform);
        }
        if (hasPlayerLOS && Mathf.Abs(Vector3.Distance(lastKnownPlayerLocation, transform.position)) > minDistanceToPlayer && (!chasing || waiting) && Mathf.Abs(Vector3.Distance(lastKnownPlayerLocation, transform.position)) < viewRange) {
            waiting = false;
            navAgent.Resume();
        }
        if (chasing) {

            if (hasPlayerLOS && !isShooting) //added by pablo
            {
                isShooting = true;
            }

            if (Mathf.Abs(Vector3.Distance(lastKnownPlayerLocation, transform.position)) < minDistanceToPlayer && hasPlayerLOS) {
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
        if (Mathf.Abs(Vector3.Distance(lastKnownPlayerLocation, transform.position)) < 0.6f && !hasPlayerLOS && chasing) {

            //Debug.Log("PROBLEM IS HERE MOFO");
            //chasing = false;
            waiting = false;
            GoHome();
            navAgent.Resume();
        }

        if (isShooting) {
            //navAgent.Stop();
            foreach (TurretHardpoint t in hardpoints) {
                if (t.weapon != null)
                    t.weapon.AutoFire();
            }
        }
        else {
            navAgent.Resume();
            foreach (TurretHardpoint t in hardpoints) {
                if (t.weapon != null) 
                    t.weapon.StopFiring();
            }
        }
    }

    public void SpawnTurrets(int nrOfGuns, Weapon.Weapons[] weapons) {
        if (nrOfGuns > _turretHardpoints.Length) nrOfGuns = _turretHardpoints.Length;
        for (int i = 0; i < nrOfGuns; i++) {
            TurretHardpoint point = hardpoints[i];
            point.hardpoint = _turretHardpoints[i];
            point.weaponOnHardpoint = weapons[i];

            GameObject newTurret = Instantiate(GameController.Instance.weaponPrefabs[(int)point.weaponOnHardpoint], point.hardpoint.position, point.hardpoint.rotation) as GameObject;
            point.weapon = newTurret.GetComponent<Weapon>();
            point.weapon.teamToHit = GameController.Teams.Player;
            newTurret.transform.SetParent(point.hardpoint, false);
            newTurret.transform.localPosition = Vector3.zero;
            hardpoints[i] = point;
            
        }
    }

}
