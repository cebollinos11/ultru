using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Decorator : MonoBehaviour {

    MapGenerator mG;
    Object[] wallDecorationsDB;
    [SerializeField]
    int WallDecorationProb;
	// Use this for initialization


    public void InitializeDecorator() {

        mG = transform.GetComponent<MapGenerator>();
        wallDecorationsDB = Resources.LoadAll("mapgen/walldecorations");
    
    }


    public void DecorateLocations() {

       


        Debug.Log("mg locations is " + mG.ToString());

        foreach (KeyValuePair<int, GameObject> k in mG.locations)
        {
            //PaintRoom(k.Value.transform);
            Transform wallPack = k.Value.transform.FindChild("Walls");

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
        if (wall.lossyScale.x < 1)
            return;

        float horizontalOffset = Random.Range( -wall.lossyScale.x*10/3,wall.lossyScale.x*10/3);       
        
        
        GameObject deco = Instantiate(wallDecorationsDB[Random.Range(0,wallDecorationsDB.Length)], wall.position,wall.rotation) as GameObject;
        deco.transform.Translate(horizontalOffset, 0f, 0f);
        deco.transform.position = new Vector3(deco.transform.position.x, 2.5f, deco.transform.position.z);
        deco.transform.parent = wall.parent;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
