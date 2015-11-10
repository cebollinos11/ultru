using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	[SerializeField] int maxRooms;
	[SerializeField] GameObject spawnRoomPrefab;
	[SerializeField] GameObject exitRoomPrefab;
	[SerializeField] GameObject doorPrefab;
    [SerializeField] GameObject blockedDoorPrefab;
    [SerializeField] int maxSpawnAttempts = 10;
    [SerializeField] float noDoorProbabilityPercent = 35;
	Object[] roomsDB;
	Object[] hallwaysDB;
	Dictionary<int, GameObject> locations;
	List<Transform> unusedDoorways;
    List<Transform> closedDoorways;
    List<Transform> rooms;
	Dictionary<Transform, Transform> roomConnections;
	int roomCounter = 0;

    [SerializeField]
    GameObject hackTerminal;
		
	protected float ROOMDISTANCEOFFSET = 0.5f;


    [SerializeField] Texture floorTexture;
    [SerializeField]
    Texture floorNormalMap;

    [SerializeField]
    Texture wallTexture;
    [SerializeField]
    Texture wallNormalMap;


	// Use this for initialization
	void Start () {
		locations = new Dictionary<int, GameObject>();
		roomConnections = new Dictionary<Transform, Transform>();
		unusedDoorways = new List<Transform>();
        closedDoorways = new List<Transform>();
		roomsDB = Resources.LoadAll("mapgen/rooms");
		hallwaysDB = Resources.LoadAll("mapgen/hallways");
        rooms = new List<Transform>();
		GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void GenerateMap() {
        Debug.Log("Generating spawn room");
		locations = new Dictionary<int, GameObject>();
		unusedDoorways = new List<Transform>();
		roomCounter = 0;
		GameObject spawnedRoom = (GameObject)Instantiate(spawnRoomPrefab, Vector3.zero, Quaternion.identity);
		Room room = spawnedRoom.GetComponent<Room>();
		locations.Add(roomCounter, spawnedRoom);

        

		unusedDoorways.AddRange(room.doorways);
		roomCounter++;

        Debug.Log("Spawning in random rooms");
		for (int i = roomCounter; i < maxRooms; i++) {
			AttemptSpawnRoom();
		}
		PlaceExit();
		PlaceDoors();
        CloseUnusedConnections();
        PaintAllRooms();
        PopulateRooms();


	}

    void PopulateRooms() { 
        
        //first of all add hackable servers
        SpawnWhatWhere(rooms[Random.Range(0,rooms.Count-1)], hackTerminal);
        


    }


    void SpawnWhatWhere(Transform room, GameObject itemToSpawn) {
        
        GameObject go = Instantiate(itemToSpawn, room.position, room.rotation) as GameObject;
        go.transform.parent = room.transform;
    }
    

    void PaintAllRooms() {

        foreach (KeyValuePair<int, GameObject> k in locations) {
            PaintRoom(k.Value.transform);
        }
        
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

            //if this is a room (to spawn objects), add it to the room list
            
            if (newRoom.GetComponent<Room>().isRoom)
            {
                rooms.Add(newRoom.transform);
                
            }
		}
	}

	void AttemptSpawnRoom(Transform fromRoom, Transform exitDoor, GameObject newRoomPrefab, int roomId) {
		GameObject newRoom = SpawnRoom(fromRoom, exitDoor, newRoomPrefab, roomId);
		int tryCounter = 0;
		while (newRoom == null && tryCounter < maxSpawnAttempts) {
			GameObject.Destroy(newRoom);
			newRoom = SpawnRoom(fromRoom, exitDoor, newRoomPrefab, roomId);
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
		roomConnections.Add(entrance, selectedDoorway);
		room.connections.Add(entrance, selectedDoorway);
		entrance.parent.GetComponent<Room>().connections.Add(selectedDoorway, entrance);
		unusedDoorways.AddRange(temp);
		return selectedRoom;
	}

	void ConnectRooms(Transform room1, Transform doorway1, Transform room2, Transform doorway2) {
		Vector3 newrot = doorway1.rotation.eulerAngles - doorway2.rotation.eulerAngles +new Vector3(0, 180, 0);
		room2.rotation = Quaternion.Euler(0, newrot.y+room2.rotation.eulerAngles.y, 0);
		room2.position = doorway1.position;
		Vector3 temp = doorway2.position - room2.position;
		room2.position -= temp - doorway1.forward * ROOMDISTANCEOFFSET;
	}

	void PlaceExit() {
		Debug.Log("Placing exit room"); 
		Transform newExit = GetUnusedExit();
		AttemptSpawnRoom(newExit.parent.transform, newExit, exitRoomPrefab, roomCounter);
	}

	void PlaceDoors() {
		Debug.Log("Placing doors in open doorways");
		foreach (KeyValuePair<Transform, Transform> k in roomConnections) {
			GameObject door = (GameObject) GameObject.Instantiate(doorPrefab, k.Key.position - ((k.Value.position - k.Key.position) / 2), k.Key.rotation);
			door.transform.position += door.transform.position - door.GetComponentInChildren<Door>().zero.position;
            door.GetComponentInChildren<Door>().SetDoorVisibility(!(noDoorProbabilityPercent >= Random.Range(1, 100)));
            door.transform.parent = k.Key.transform.parent;
		}
	}

    void PaintRoom(Transform room)
    {

        foreach (Transform t in room)
         {
             if (t.name == "Floor")
                 ApplyTextureTo(t, floorTexture, floorNormalMap, 2);

            if(t.name == "Ceiling")
                ApplyTextureTo(t, floorTexture, floorNormalMap, 2);            
         }

       // Transform floor = room.FindChild("Floor");
        //ApplyTextureTo(floor, floorTexture, floorNormalMap, 2);

        //Transform ceiling = room.FindChild("Ceiling");
        //if (ceiling == null) { Debug.Log("NO CEILING FOUND");
        //Debug.Log(room.name);
        //}
        //ApplyTextureTo(ceiling, floorTexture, floorNormalMap, 2);

        //walls
        Transform wallPack = room.FindChild("Walls");
        
        if (wallPack == null) { return; }
        foreach (Transform child in wallPack)
        {
            //child is your child transform
            ApplyTextureTo(child.FindChild("Plane"), wallTexture, wallNormalMap, 1);
        }

    }

    void ApplyTextureTo(Transform who, Texture texture, Texture normalMap, int multiplier)
    {
        //Debug.Log(who);
        if (who == null) { return; }
        Material mat = who.GetComponent<MeshRenderer>().material;

        mat.mainTexture = texture;
        mat.color = Color.white;
        mat.SetTexture("_BumpMap", normalMap);

        //tiling
        mat.mainTextureScale = new Vector2(who.transform.lossyScale.x, who.transform.lossyScale.z) * multiplier;



    }

	void CloseUnusedConnections() {
        Debug.Log("Closing unused connections!");
        
        foreach (KeyValuePair<int, GameObject> k in locations) {
            Room room = k.Value.GetComponent<Room>();
            if (room == null) continue;

            foreach (Transform t in room.doorways) {
                if (!room.connections.ContainsValue(t)) {
                    closedDoorways.Add(t);
                    GameObject blockedDoor = (GameObject)GameObject.Instantiate(blockedDoorPrefab, t.position, t.rotation);
                    Transform doorZero = blockedDoor.transform.FindChild("Zero");
                    blockedDoor.transform.position += blockedDoor.transform.position - doorZero.position;
                }
            }
        }
        //Place closed doors at closeddoorways positions.
        Debug.Log("Closed " + closedDoorways.Count + " doorways");
        
	}

}
