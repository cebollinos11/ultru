using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public List<Transform> doorways;
    //                   Theirs, Mine
	public Dictionary<Transform, Transform> connections = new Dictionary<Transform, Transform>();
	// Use this for initialization

    public Texture floorTexture;
    public Texture floorNormalMap;

    public Texture wallTexture;
    public Texture wallNormalMap;

    

    void Start()
    {

        //PaintRoom(transform);

    }

    void PaintRoom(Transform room) {

        Transform floor = room.FindChild("Floor");
        ApplyTextureTo(floor, floorTexture, floorNormalMap, 2);

        Transform ceiling = room.FindChild("Ceiling");
        ApplyTextureTo(ceiling, floorTexture, floorNormalMap, 2);

        //walls
        Transform wallPack = room.FindChild("Walls");
        foreach (Transform child in wallPack)
        {
            //child is your child transform
            ApplyTextureTo(child.FindChild("Plane"), wallTexture, wallNormalMap, 1);
        }
    
    }

    void ApplyTextureTo(Transform who, Texture texture, Texture normalMap, int multiplier)
    {
        Material mat = who.GetComponent<MeshRenderer>().material;

        mat.mainTexture = texture;
        mat.color = Color.white;
        mat.SetTexture("_BumpMap", normalMap);

        //tiling
        mat.mainTextureScale = new Vector2(who.transform.lossyScale.x, who.transform.lossyScale.z) * multiplier;

          
    
    }

	// Update is called once per frame
	void Update () {
	}

}
