using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

    private float delayTurnOn = 10f; //2 sec delay

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.L))
            TurnOff();

        if (Input.GetKeyDown(KeyCode.K))
            TurnOn();

	
	}

    public void TurnOff() {

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    
    }

    public void TurnOn() {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void DelayTurnOn() {

        Invoke("TurnOn", delayTurnOn);
    
    }


}
