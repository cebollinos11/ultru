using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Decorator : MonoBehaviour {

    MapGenerator mG;
    Object[] wallDecorationsDB;
    [SerializeField]
    int WallDecorationProb;
	// Use this for initialization

    
    Material floorMaterial;    
    Material wallMaterial;   
    Material CeilingMaterial;



    public void InitializeDecorator() {

        mG = transform.GetComponent<MapGenerator>();
        wallDecorationsDB = Resources.LoadAll("mapgen/walldecorations");
        floorMaterial = (Material)Resources.Load("mapgen/roomParts/MaterialsPack/Materials/floorTex");
        wallMaterial = (Material)Resources.Load("mapgen/roomParts/MaterialsPack/Materials/wallTex");
        CeilingMaterial = (Material)Resources.Load("mapgen/roomParts/MaterialsPack/Materials/ceilingTex");
        
    
    }


    public void DecorateLocations() {

       


        foreach (KeyValuePair<int, GameObject> k in mG.locations)
        {
            
            //PaintRoom(k.Value.transform);
            Transform wallPack = k.Value.transform.FindChild("Walls");
            Debug.Log("decorating " + k.Value.name+ " "+wallPack);
            if (wallPack == null) { return; }
            foreach (Transform child in wallPack)
            {
                //child is your child transform
                if(Random.Range(0,100)<WallDecorationProb)
                {
                    PlaceWallDecoration(child.FindChild("Plane"));
                    //PlaceWallDecoration(child.FindChild("Plane"));
                }
                    
            }
        }
    
    }

    void PlaceWallDecoration(Transform wall)
    {
        
        //dont decorate small walls
        if (wall.lossyScale.x < 0.9)
        {
            
            return;
        }
        float horizontalOffset = Random.Range( -wall.lossyScale.x*10/3,wall.lossyScale.x*10/3);       
        
        
        GameObject deco = Instantiate(wallDecorationsDB[Random.Range(0,wallDecorationsDB.Length)], wall.position,wall.rotation) as GameObject;
        deco.transform.Translate(horizontalOffset, 0f, 0f);
        deco.transform.position = new Vector3(deco.transform.position.x, 2.5f, deco.transform.position.z);
        deco.transform.parent = wall.parent;
    }

    public void PaintRoom(Transform room)
    {
        
        foreach (Transform t in room)
        {
            if (t.name == "Floor")
                ApplyMaterialTo(t, floorMaterial, 4);

            if (t.name == "Ceiling")
                ApplyMaterialTo(t, CeilingMaterial, 4);
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
            ApplyMaterialTo(child.FindChild("Plane"), wallMaterial, 2);
        }

    }
    void ApplyMaterialTo(Transform who, Material material, int multiplier)
    {

        who.GetComponent<MeshRenderer>().material = material;
        Vector2 scale = new Vector2(who.transform.lossyScale.x, who.transform.lossyScale.z) * multiplier;
        who.GetComponent<MeshRenderer>().material.mainTextureScale = scale;
        who.GetComponent<MeshRenderer>().material.SetTextureScale("_BumpMap", scale);
        

    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
