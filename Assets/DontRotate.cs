using UnityEngine;
using System.Collections;

public class DontRotate : MonoBehaviour {

    Quaternion originalRot;
	// Use this for initialization
	void Start () {
        originalRot = transform.rotation;
	}
	
	// Update is called once per frame
    void Update()
    {

        transform.rotation = originalRot;
	
	}
}
