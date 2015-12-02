using UnityEngine;
using System.Collections;

public class Boss_Orb : Enemy {

    [SerializeField] Transform[] _turretHardpoints;
    [SerializeField] float _playerLocationUpdateTime = 3;
    [SerializeField] float _viewRange = 100;
    TurretHardpoint[] hardpoints;

    //For testing - To remove
    [SerializeField] Weapon.Weapons[] _weaponsOnHardpoints;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        base.playerLocationUpdateTime = _playerLocationUpdateTime;
        base.viewRange = _viewRange;
        hardpoints = new TurretHardpoint[_turretHardpoints.Length];
        MAXHARDPOINTS = 4;

        //For testing. Should be run by MapGenerator.
        SpawnTurrets(2, _weaponsOnHardpoints);
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        if (!hasPlayerLOS) {
            isShooting = false;
        }
        else if (hasPlayerLOS && !isShooting) {
            isShooting = true;
        }

        transform.LookAt(player.transform);

        if (isShooting) {
            foreach (TurretHardpoint t in hardpoints) {
                if (t.weapon != null)
                    t.weapon.AutoFire();
            }
        }
        else {
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

    public override void Death() {
        base.Death();
        transform.parent = null;
    }
}
