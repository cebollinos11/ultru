using UnityEngine;
using System.Collections;

public class CeilingBuilder : MonoBehaviour {

    public float height;

	// Use this for initialization
	void Start () {

        GameObject instance = Instantiate(Resources.Load("mapgen/roomParts/Ceiling", typeof(GameObject))) as GameObject;
        instance.transform.position = transform.position+new Vector3(0,height,0);
        instance.transform.localScale = transform.localScale;
        instance.transform.rotation = transform.rotation;
        instance.transform.Rotate(0, 0, 180, Space.Self);    
        
	    
	}
	
	
}
