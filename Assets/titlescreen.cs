using UnityEngine;
using System.Collections;

public class titlescreen : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if (LevelManager.Instance.lvl != 1)
        {
            gameObject.SetActive(false);
        }

        else {
            Invoke("TurnOff", 5f);
        
        }
	
	}

    void TurnOff() {
       
        gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            TurnOff();
        }
    }
	
	
}
