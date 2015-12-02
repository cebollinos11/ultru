using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController> {

    public enum Teams {
        Player,
        Enemy
    }

    [SerializeField] public GameObject[] weaponPrefabs;

    bool isShutdown;
    bool shutDownComplete;
    bool maxLightReached;

    public void ShutDown() {

        foreach (GameObject lightBulb in GameObject.FindGameObjectsWithTag("LightBulb"))
        {
            lightBulb.GetComponent<LightController>().TurnOff();
            lightBulb.GetComponent<LightController>().DelayTurnOn();
        }

        //ElevatorManager.ActivateElevator();
        GameObject.Find("Elevator").GetComponent<ElevatorManager>().ActivateElevator();
        AudioManager.AudioShutdown();
        StartCoroutine(doShutdown());
        

        ////shuts down the space station, all ambient lights are off, and then after X time, the emergency lights start

        //if (isShutdown) { return;
        //}

        //isShutdown = true;
        //RenderSettings.ambientIntensity = 0f;


        ////find all light bulbs and turn them off
        //foreach (GameObject lightBulb in GameObject.FindGameObjectsWithTag("LightBulb"))
        //{
        //    lightBulb.GetComponent<LightController>().TurnOff();
        //    lightBulb.GetComponent<LightController>().DelayTurnOn();
        //}

        ////find player SpotLight and turn it on
        ////Debug.Log("turning on " + GameObject.Find("Flashlight"));
        ////GameObject.Find("Flashlight").GetComponent<Flashlight>().TurnOn();
        


    }

    IEnumerator doShutdown() {

        if (isShutdown  && shutDownComplete)
        {           

            ////find player SpotLight and turn it on
            ////Debug.Log("turning on " + GameObject.Find("Flashlight"));
            ////GameObject.Find("Flashlight").GetComponent<Flashlight>().TurnOn();

            

            yield break;
        }

        while (!isShutdown && !shutDownComplete) 
        {
            if (!maxLightReached && RenderSettings.ambientIntensity < 8.0f)
            {
                RenderSettings.ambientIntensity = RenderSettings.ambientIntensity + 4.0f;
            }
            else if (RenderSettings.ambientIntensity >= 8.0f)
            {
                maxLightReached = true;
            }


            if (maxLightReached && RenderSettings.ambientIntensity > 0.0f)
            {
                RenderSettings.ambientIntensity -= 2.0f;
            }
            else if (RenderSettings.ambientIntensity <= 0.0f)
            {
                shutDownComplete = true;
                isShutdown = true;
            }

            yield return new WaitForSeconds(0.05f);
        }
      


    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Q)) {
            ShutDown();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            LevelManager.Instance.lvl++;
            LevelManager.Instance.GoToMainMenu();
        }
    }

}
