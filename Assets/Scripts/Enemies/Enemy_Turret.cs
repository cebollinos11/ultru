using UnityEngine;
using System.Collections;

public class Enemy_Turret : Enemy {

        [SerializeField] enemySurfacePlacement _placedOnSurface;
    [SerializeField] Transform[] _turretHardpoints;
    [SerializeField] Weapon.Weapons[] _weaponsOnHardpoints;
    [SerializeField] float _playerLocationUpdateTime = 3;
    [SerializeField] float _viewRange = 100;

    [SerializeField] Transform housing;
    [SerializeField] Transform barrel1;
    [SerializeField] Transform barrel2;
    [SerializeField] float housingRotationSpeed = 20;
    [SerializeField] float barrelRotationSpeed = 10;

    TurretHardpoint[] hardpoints;

    Quaternion barrel1StartRot;
    Quaternion barrel2StartRot;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        base.playerLocationUpdateTime = _playerLocationUpdateTime;
        base.viewRange = _viewRange;
        hardpoints = new TurretHardpoint[_turretHardpoints.Length];
        base.placedOnSurface = _placedOnSurface;
        barrel1StartRot = barrel1.localRotation;
        barrel2StartRot = barrel2.localRotation;

        SpawnTurrets(2, _weaponsOnHardpoints);
        
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        float distanceToPlayer = Vector3.Dot(housing.up, player.transform.position - housing.position);
        Vector3 playerPoint = player.transform.position - housing.up * distanceToPlayer;
        Quaternion housingRot = Quaternion.LookRotation(playerPoint - housing.position, housing.up);
        housing.rotation = Quaternion.RotateTowards(housing.rotation, housingRot, housingRotationSpeed * Time.deltaTime);
        
        Vector3 temp1 = new Vector3(0, distanceToPlayer, (playerPoint - housing.position).magnitude);
        temp1.y -= 1;
        Quaternion barrelRot = Quaternion.LookRotation(temp1);

        barrel1.localRotation = barrel2.localRotation = Quaternion.RotateTowards(barrel2.localRotation, barrelRot, barrelRotationSpeed * Time.deltaTime);

        if (!hasPlayerLOS) {
            isShooting = false;
        }
        else if (hasPlayerLOS && !isShooting) {
            isShooting = true;
        }

        if (isShooting) {
            foreach (TurretHardpoint t in hardpoints) {
                t.weapon.AutoFire();
            }
        }
        else {
            foreach (TurretHardpoint t in hardpoints) {
                if (t.weapon.isFiring) {
                    t.weapon.StopFiring();
                }
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
