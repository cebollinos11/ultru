using UnityEngine;
using System.Collections;

public class Presetter : MonoBehaviour {

    Object[] prefabsDB;
    Transform chosenPreset;

	// Use this for initialization
	void Start () {

        prefabsDB = Resources.LoadAll("mapgen/bigroomDecorations");

        int nPresets = transform.childCount;
        int chosenIndex = Random.Range(0, nPresets);

        

        foreach (Transform spawner in transform.GetChild(chosenIndex)) {

            GameObject go = (GameObject)Instantiate(prefabsDB[Random.Range(0, prefabsDB.Length)], spawner.position, spawner.rotation);
            go.transform.parent = transform.parent;
        }

        //foreach (Transform t in listOfPresets) {
        //    Debug.Log("FOUND " + t.name);
        //}
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
