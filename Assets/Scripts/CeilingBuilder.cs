using UnityEngine;
using System.Collections;

public class CeilingBuilder : MonoBehaviour {

    public float height;
    private float lightStep = 10f;

	// Use this for initialization
	void Awake () {

        GameObject instance = Instantiate(Resources.Load("mapgen/roomParts/Ceiling", typeof(GameObject))) as GameObject;
        instance.transform.position = transform.position+new Vector3(0,height,0);
        instance.transform.localScale = transform.localScale;
        instance.transform.rotation = transform.rotation;
        instance.transform.Rotate(0, 0, 180, Space.Self);

        instance.transform.parent = transform.parent;
        instance.transform.name = "Ceiling";

        //place ligths
        //PlaceLight(instance.transform.position, instance.transform.parent);
        //Debug.Log(instance.transform.lossyScale.x);
        //float sx = (int)instance.transform.lossyScale.x;
        //float sz = (int)instance.transform.lossyScale.z;
        //int i = 0;
        //int j = 0;
        //for (j = 1; j < sz-1; j++)
        //    for (i = 1; i < sx-1;i++ )
        //        PlaceLight(instance.transform.position+new Vector3(i-sx/2,0,j-sz/2)*10, instance.transform.parent);
        PlaceLight(instance.transform.position, instance.transform.parent);
	    
	}

    void PlaceLight(Vector3 where,Transform parent)
    {
        GameObject light = Instantiate(Resources.Load("Prefabs/BasicLight", typeof(GameObject))) as GameObject;
        light.transform.position = where ;
        light.transform.parent = parent;
    }
	
	
}
