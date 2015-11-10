using UnityEngine;
using System.Collections;

public class CeilingBuilder : MonoBehaviour {

    public float height;

	// Use this for initialization
	void Awake () {

        GameObject instance = Instantiate(Resources.Load("mapgen/roomParts/Ceiling", typeof(GameObject))) as GameObject;
        instance.transform.position = transform.position+new Vector3(0,height,0);
        instance.transform.localScale = transform.localScale;
        instance.transform.rotation = transform.rotation;
        instance.transform.Rotate(0, 0, 180, Space.Self);

        instance.transform.parent = transform.parent;
        instance.transform.name = "Ceiling";

        //place ligths
        GameObject light = Instantiate(Resources.Load("Prefabs/BasicLight", typeof(GameObject))) as GameObject;
        light.transform.position = instance.transform.position;
        light.transform.parent = instance.transform;
        
	    
	}
	
	
}
