using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public Text currentLevel;
    public Text info;
    public int maxlevel = 2;

	// Use this for initialization
	void Start () {

        SetInfo();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if (LevelManager.Instance.lvl > maxlevel) {
            LevelManager.Instance.lvl = maxlevel;
        
        }
    }

    void SetInfo() {

        LevelManager.Instance.UpdateValues();

        string t = "";
        t += "Space Station size      \t " + LevelManager.Instance.spaceStationSize + "\n";

        t += "Enemy Orbs      \t "+LevelManager.Instance.orb+"%\n";
        t += "Enemy Turrets   \t " + LevelManager.Instance.turret + "%\n";
        t += "Aid Items       \t " + LevelManager.Instance.help + "%\n";
        t += "Weapon packages \t " + LevelManager.Instance.weapon + "%\n";
        
       

        if (LevelManager.Instance.lvl == maxlevel)
        {
            t = "No info available...";
        }

        info.text = t;


    
    
    }
	
	// Update is called once per frame
	void Update () {

        currentLevel.text = LevelManager.Instance.lvl.ToString()+"/"+maxlevel.ToString();
	    if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LevelManager.Instance.LoadBoss();
        }
	}

    public void OnStartButtonClick() {

        if (LevelManager.Instance.lvl < maxlevel){
        LevelManager.Instance.StartGame();
        }
            
        else {
            LevelManager.Instance.LoadBoss();

        }
    
    }
}
