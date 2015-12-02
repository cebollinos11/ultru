using UnityEngine;
using System.Collections;

public class FinalBossStageController : MonoBehaviour {

    bool hasTheBossSpawned;

	// Use this for initialization
	void Start () {

        Invoke("startMusic", 2f);
	
	}

    public void SpawnBoss() {
        if (hasTheBossSpawned)
            return;

        hasTheBossSpawned = true;
        AudioManager.AudioShutdown();

    }

    public void BigBossKilled() {
        GameObject.Find("Elevator").GetComponent<ElevatorManager>().ActivateElevator();          
    }

    void startMusic() {

        AudioManager.PlayBgSong(0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}



}
