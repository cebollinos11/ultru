using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public List<Transform> doorways;
    //                   Theirs, Mine
	public Dictionary<Transform, Transform> connections = new Dictionary<Transform, Transform>();

    public bool isRoom;
	// Use this for initialization



    

    void Start()
    {

        //PaintRoom(transform);

    }



	// Update is called once per frame
	void Update () {
	}

}
