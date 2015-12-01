using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public Text currentLevel;
    public Text info;

	// Use this for initialization
	void Start () {

        SetInfo();
	
	}

    void SetInfo() {

        LevelManager.Instance.UpdateValues();

        string t = "";
        t += "Space Station size      \t " + LevelManager.Instance.spaceStationSize + "\n";

        t += "Enemy Orbs      \t "+LevelManager.Instance.orb+"%\n";
        t += "Enemy Turrets   \t " + LevelManager.Instance.turret + "%\n";
        t += "Aid Items       \t " + LevelManager.Instance.help + "%\n";
        t += "Weapon packages \t " + LevelManager.Instance.weapon + "%\n";
        
        info.text = t;


    
    
    }
	
	// Update is called once per frame
	void Update () {

        currentLevel.text = LevelManager.Instance.lvl.ToString();
	
	}

    public void OnStartButtonClick() {

        LevelManager.Instance.StartGame();
    
    }
}
