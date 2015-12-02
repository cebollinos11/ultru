using UnityEngine;
using System.Collections;

public class LevelManager : Singleton<LevelManager> {

    public int lvl = 1;

    public float orb, turret,help,weapon;
    public int spaceStationSize;

	// Use this for initialization
	void Start () {

        gameObject.name = "LevelManager";
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoToMainMenu() {
        Application.LoadLevel(0);    
    }

    public void StartGame() {
        Application.LoadLevel(1);
    }

    public void UpdateValues() {
        spaceStationSize = (lvl) * 10;
        orb = (lvl+1)*10;
        turret = (lvl + 1) * 10;
        help = 50-lvl*4;
        weapon = 10 - lvl;
    
    }

    public bool Roll(int limit)
    {
        int die = Random.Range(0, 100);
        return die < limit;
    }

    public bool Roll(float limit)
    {
        int die = Random.Range(0, 100);

        Debug.Log("Rolling " + die.ToString() + " -> limit " + limit.ToString());
        return die < limit;
    }
}
