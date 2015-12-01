using UnityEngine;
using System.Collections;

public class LevelManagerManager : MonoBehaviour {

    [SerializeField]
    GameObject levelManager;

	// Use this for initialization
	void Start () {

        if (GameObject.Find("LevelManager") == null) {
            
            Instantiate(levelManager);            
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
