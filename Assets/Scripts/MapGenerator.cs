using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	[SerializeField] int maxRooms;
	[SerializeField] GameObject spawnRoom;
	[SerializeField] int maxSpawnAttempts = 10;
	Object[] roomsDB;
	Object[] hallwaysDB;
	Dictionary<int, GameObject> locations;
	List<Transform> unusedDoorways;
    List<Transform> closedDoorways;
	int roomCounter = 0;


	//Testing Variables - TO DELETE
	float counter = 0;
	[SerializeField] GameObject tested;


	// Use this for initialization
	void Start () {
		locations = new Dictionary<int, GameObject>();
		unusedDoorways = new List<Transform>();
        closedDoorways = new List<Transform>();
		roomsDB = Resources.LoadAll("mapgen/rooms");
		hallwaysDB = Resources.LoadAll("mapgen/hallways");
		GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
            CloseUnusedConnections();
		}
	}
	
	void GenerateMap() {
        Debug.Log("Generating spawn room");
		locations = new Dictionary<int, GameObject>();
		unusedDoorways = new List<Transform>();
		roomCounter = 0;
		GameObject spawnedRoom = (GameObject)Instantiate(spawnRoom, Vector3.zero, Quaternion.identity);
		Room room = spawnedRoom.GetComponent<Room>();
		locations.Add(roomCounter, spawnedRoom);
		unusedDoorways.AddRange(room.doorways);
		roomCounter++;

        Debug.Log("Spawning in random rooms");
		for (int i = roomCounter; i < maxRooms; i++) {
			AttemptSpawnRoom();
		}
        CloseUnusedConnections();
	}

	void AttemptSpawnRoom() {
		Transform newExit = GetUnusedExit();
		GameObject newRoom = SpawnRoom(newExit.parent.transform, newExit, (GameObject)roomsDB[Random.Range(0, roomsDB.Length)], roomCounter);
		int tryCounter = 0;
		while (newRoom == null && tryCounter < maxSpawnAttempts) {
			GameObject.Destroy(newRoom);
			newRoom = SpawnRoom(newExit.parent.transform, newExit, (GameObject)roomsDB[Random.Range(0, roomsDB.Length)], roomCounter);
			tryCounter++;
		}
		if (tryCounter >= maxSpawnAttempts) {
			//newExit.GetComponent<MeshRenderer>().enabled = false;
			if (unusedDoorways.Count > 0 ) {
				AttemptSpawnRoom();
				return;
			}
			foreach (KeyValuePair<int, GameObject> k in locations) {
				GameObject.Destroy(k.Value);
			}
			GenerateMap();
		}
		else {
			locations.Add(roomCounter, newRoom);
			roomCounter++;
		}
	}
	
	Transform GetUnusedExit() {
		Transform returnValue;
		int i = Random.Range(0, unusedDoorways.Count-1);
		returnValue = unusedDoorways[i];
		unusedDoorways.RemoveAt(i);
		return returnValue;
	}

	GameObject SpawnRoom(Transform oldRoom, Transform entrance, GameObject selectedRoom, int key) {
 		selectedRoom = (GameObject)Instantiate(selectedRoom, Vector3.zero, Quaternion.identity);
		Room room = selectedRoom.GetComponent<Room>();
		List<Transform> temp = room.doorways;
		int i = Random.Range(0, temp.Count-1);
		Transform selectedDoorway = temp[i];
		temp.RemoveAt(i);

		ConnectRooms(oldRoom, entrance, selectedRoom.transform, selectedDoorway);
		entrance.gameObject.name = key.ToString();
		selectedDoorway.gameObject.name = key.ToString();

		Bounds newBounds = selectedRoom.GetComponent<BoxCollider>().bounds;
		bool doesIntersect = false;
		foreach (KeyValuePair<int, GameObject> k in locations) {
			Bounds otherBounds = k.Value.GetComponent<BoxCollider>().bounds;
			doesIntersect = newBounds.Intersects(otherBounds);
			if (doesIntersect) break;
		}

		if (doesIntersect) {
			GameObject.Destroy(selectedRoom);
			return null;
		}
		
		//entrance.GetComponent<MeshRenderer>().enabled = false;
		//selectedDoorway.GetComponent<MeshRenderer>().enabled = false;
		room.connections.Add(entrance, selectedDoorway);
		entrance.parent.GetComponent<Room>().connections.Add(selectedDoorway, entrance);
		unusedDoorways.AddRange(temp);
		return selectedRoom;
	}

	void ConnectRooms(Transform room1, Transform doorway1, Transform room2, Transform doorway2) {
		Vector3 newrot = doorway1.rotation.eulerAngles - doorway2.rotation.eulerAngles +new Vector3(0, 180, 0);
		room2.rotation = Quaternion.Euler(0, newrot.y+room2.rotation.eulerAngles.y, 0);
		room2.position = doorway1.position;
		room2.position -= doorway2.position-room2.position;
	}

	void CloseUnusedConnections() {
        Debug.Log("Closing unused connections!");
        
        foreach (KeyValuePair<int, GameObject> k in locations) {
            Room room = k.Value.GetComponent<Room>();
            if (room == null) continue;

            foreach (Transform t in room.doorways) {
                if (!room.connections.ContainsValue(t)) {
                    closedDoorways.Add(t);
                }
            }
        }
        //Place closed doors at closeddoorways positions.
        Debug.Log("Closed " + closedDoorways.Count + " doorways");
        
	}

}
