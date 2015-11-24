using UnityEngine;
using System.Collections;

public class TextureTiler : MonoBehaviour {

    public    float speed;
	// Use this for initialization

    MeshRenderer mr;

	void Start () {
        mr = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 currentOffset = mr.material.mainTextureOffset;
        mr.material.mainTextureOffset = currentOffset +Vector2.up*speed*Time.deltaTime;
	
	}
}
