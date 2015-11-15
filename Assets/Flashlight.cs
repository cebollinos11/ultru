using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {

    
    private Light lightObject;

	// Use this for initialization
	void Start () {

        lightObject = GetComponent<Light>();

        TurnOff();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.F))
        { Toggle(); }
	
	}

    public void TurnOff()
    {

        lightObject.enabled = false;
        
    }

    public void TurnOn()
    {
        lightObject.enabled = true;
    }

    public void Toggle() {
        lightObject.enabled = !lightObject.enabled;
    }

}
