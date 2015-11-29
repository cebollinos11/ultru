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
	public Dictionary<int, GameObject> locations;
	List<Transform> unusedDoorways;
    List<Transform> closedDoorways;
    List<Transform> rooms;
	Dictionary<Transform, Transform> roomConnections;
	int roomCounter = 0;

    [SerializeField]
    GameObject hackTerminal;
    [SerializeField]
    GameObject Enemy;
		
	protected float ROOMDISTANCEOFFSET = 0.5f;

    Decorator decorator;

    Transform CurrentSpaceStation;





	// Use this for initialization
	void Start () {

        Random.seed =  Random.Range(0, 1000);
        Debug.Log("Seed used: "+ Random.seed.ToString());

        //Random.seed = 14;

		GenerateMap();
        AudioManager.PlayBgSong(0);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void GenerateMap() {
        Debug.Log("*** Starting Generate Map ***");

        var go = new GameObject();
        go.name = "Space Station"+Random.Range(0,100).ToString();
        CurrentSpaceStation = go.transform;
        //Instantiate(go, Vector3.zero, Quaternion.identity);




        //foreach (KeyValuePair<int, GameObject> k in locations)
        //{
        //    GameObject.Destroy(k.Value);
        //}


        locations = new Dictionary<int, GameObject>();
        roomConnections = new Dictionary<Transform, Transform>();
        unusedDoorways = new List<Transform>();
        closedDoorways = new List<Transform>();
        roomsDB = Resources.LoadAll("mapgen/rooms");
        rooms = new List<Transform>();
        hallwaysDB = Resources.LoadAll("mapgen/hallways");
        decorator = GetComponent<Decorator>();        
		roomCounter = 0;


		GameObject spawnedRoom = (GameObject)Instantiate(spawnRoomPrefab, Vector3.zero, Quaternion.identity);
        spawnedRoom.transform.parent = CurrentSpaceStation;
		Room room = spawnedRoom.GetComponent<Room>();
		locations.Add(roomCounter, spawnedRoom);

        

		unusedDoorways.AddRange(room.doorways);
		roomCounter++;

        Debug.Log("Spawning in random rooms");
		for (int i = roomCounter; i < maxRooms; i++) {
			AttemptSpawnRoom(i);
		}
		PlaceExit();
		PlaceDoors();
        CloseUnusedConnections();
        transform.GetComponent<Decorator>().InitializeDecorator();
        PaintAllRooms();
        
        transform.GetComponent<Decorator>().DecorateLocations();
        PopulateRooms();


	}

    void PopulateRooms() {


        Object[] interactableItems =  Resources.LoadAll("mapgen/interactableItems");
        
        //first of all add hackable servers
        int indexOfTerminal = Random.Range(0,rooms.Count-1);
        int i;

        //Debug.Log("indexOfTerminal:" + indexOfTerminal.ToString() + " -> " + (rooms.Count - 1).ToString());
        for (i = 0; i < rooms.Count ; i++)
        {
           
            if (i == indexOfTerminal)
            {                
                    SpawnWhatWhere(rooms[i], hackTerminal);
            }
            else
            {
                SpawnWhatWhere(rooms[i], Enemy);
            }
            
            SpawnWhatWhere(rooms[i], (GameObject)interactableItems[Random.Range(0, interactableItems.Length)]);
            


        }    


    }


    void SpawnWhatWhere(Transform room, GameObject itemToSpawn) {

        Vector3 spawnPosition;
        spawnPosition = room.position;

        Vector3 floorScale = Vector3.one;

        Transform floorTransform = room.transform;

        foreach (Transform t in room)
        {
            if (t.name == "Floor")
            {                
                floorTransform = t;
            }
        }

        spawnPosition = floorTransform.position;

       

        //spawnPosition += new Vector3(Random.Range( -floorScale.x,floorScale.x), 0f, Random.Range(-floorScale.z,floorScale.z));
        //spawnPosition += new Vector3( floorScale.x, 0f, floorScale.z);

        GameObject go = Instantiate(itemToSpawn, spawnPosition, room.rotation) as GameObject;
        go.transform.parent = floorTransform;

        int iIR = room.gameObject.GetComponent<Room>().itemsInRoom;

        if (iIR % 2 == 0) {
            iIR *= -1;    
        }
        go.transform.Translate(0f, 0f, floorTransform.lossyScale.z * 4 * (iIR) / 4);
        room.gameObject.GetComponent<Room>().itemsInRoom++;

    }
    

    void PaintAllRooms() {

        foreach (KeyValuePair<int, GameObject> k in locations) {
            decorator.PaintRoom(k.Value.transform);
        }
        
    }

	void AttemptSpawnRoom(int i) {
		Transform newExit = GetUnusedExit();
		GameObject newRoom = SpawnRoom(newExit.parent.transform, newExit, (GameObject)roomsDB[Random.Range(0, roomsDB.Length)], roomCounter);
        //Debug.Log(newRoom+ "***");
		int tryCounter = 0;
		while (newRoom == null && tryCounter < maxSpawnAttempts) {
			GameObject.Destroy(newRoom);
			newRoom = SpawnRoom(newExit.parent.transform, newExit, (GameObject)roomsDB[Random.Range(0, roomsDB.Length)], roomCounter);
            //Debug.Log(newRoom + "*X*");
            if(newRoom==null)
			    tryCounter++;
		}
		if (tryCounter >= maxSpawnAttempts) {
			//newExit.GetComponent<MeshRenderer>().enabled = false;
			if (unusedDoorways.Count > 0 ) {
				AttemptSpawnRoom(i);
				return;
			}
			foreach (KeyValuePair<int, GameObject> k in locations) {
                //Debug.Log("SOME ROOM WAS DESTROYED -> " + k.Value.ToString());
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
            if (unusedDoorways.Count > 0)
            {
                Debug.Log("FORGETTING ABOUT THE EXIT");
                //AttemptSpawnRoom();
                //return;
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

        Debug.Log("Created -> "+newRoom.name);
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
        foreach (KeyValuePair<int, GameObject> k in locations)
        {
            Bounds otherBounds = k.Value.GetComponent<BoxCollider>().bounds;
            doesIntersect = newBounds.Intersects(otherBounds);
            if (doesIntersect) break;
        }

        //Debug.Log("Spawning -> " + selectedRoom.name + " " + selectedRoom.transform.position.ToString() + " does intersect:" + doesIntersect + " connects to " + selectedDoorway.transform.position.ToString());

        if (doesIntersect)
        {
            GameObject.Destroy(selectedRoom);
            return null;
        }	
		
		//entrance.GetComponent<MeshRenderer>().enabled = false;
		//selectedDoorway.GetComponent<MeshRenderer>().enabled = false;
		roomConnections.Add(entrance, selectedDoorway);
		room.connections.Add(entrance, selectedDoorway);
		entrance.parent.GetComponent<Room>().connections.Add(selectedDoorway, entrance);
		unusedDoorways.AddRange(temp);
        selectedRoom.transform.parent = CurrentSpaceStation;
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
		//Debug.Log("Placing doors in open doorways");
		foreach (KeyValuePair<Transform, Transform> k in roomConnections) {
			GameObject door = (GameObject) GameObject.Instantiate(doorPrefab, k.Key.position - ((k.Value.position - k.Key.position) / 2), k.Key.rotation);
			door.transform.position += door.transform.position - door.GetComponentInChildren<Door>().zero.position;
            door.GetComponentInChildren<Door>().SetDoorVisibility(!(noDoorProbabilityPercent >= Random.Range(1, 100)));
            door.transform.parent = k.Key.transform.parent;
		}
	}


    //void ApplyTextureTo(Transform who, Texture texture, Texture normalMap, int multiplier)
    //{
    //    //Debug.Log(who);
    //    if (who == null) { return; }
    //    Material mat = who.GetComponent<MeshRenderer>().material;

    //    mat.mainTexture = texture;
    //    mat.color = Color.white;
    //    mat.SetTexture("_BumpMap", normalMap);

    //    //tiling
    //    mat.mainTextureScale = new Vector2(who.transform.lossyScale.x, who.transform.lossyScale.z) * multiplier;



    //}

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
                    blockedDoor.transform.parent = k.Value.transform;
                }
            }
        }
        //Place closed doors at closeddoorways positions.
        Debug.Log("Closed " + closedDoorways.Count + " doorways");
        
	}

}
