using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public static GameController Instance {
        get {
            if (instance == null) {
                instance = new GameController();
            }
            return instance;
        }
        private set {

        }
    }
    static GameController instance;

    public bool exitHacked { get; private set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ExitHacked() {
        if (exitHacked) return;
        Debug.Log("EXIT IS NOW HACKED");
        exitHacked = true;
    }
}
