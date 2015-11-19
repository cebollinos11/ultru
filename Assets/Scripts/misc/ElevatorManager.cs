using UnityEngine;
using System.Collections;

public class ElevatorManager : Singleton<ElevatorManager> {

    [SerializeField]
    GameObject cylinder;

    public bool isActive;

	// Use this for initialization
	void Start () {
        cylinder.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void ActivateElevator() {

        Instance.isActive = true;
        Instance.cylinder.SetActive(true);
    }
}
