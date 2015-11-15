using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController> {

    bool isShutdown = false;

    public void ShutDown() {

        //shuts down the space station, all ambient lights are off, and then after X time, the emergency lights start

        if (isShutdown) { return;
        }

        isShutdown = true;
        RenderSettings.ambientIntensity = 0f;

        //find all light bulbs and turn them off
        foreach (GameObject lightBulb in GameObject.FindGameObjectsWithTag("LightBulb"))
        {
            lightBulb.GetComponent<LightController>().TurnOff();
            lightBulb.GetComponent<LightController>().DelayTurnOn();

        }

        //find player SpotLight and turn it on
        //Debug.Log("turning on " + GameObject.Find("Flashlight"));
        //GameObject.Find("Flashlight").GetComponent<Flashlight>().TurnOn();
        


    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Q)) {
            ShutDown();
        }
    }

}
