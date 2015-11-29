using UnityEngine;
using System.Collections;

public class ElevatorManager : MonoBehaviour {

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

    public void ActivateElevator() {

        isActive = true;
        cylinder.SetActive(true);
    }
}
