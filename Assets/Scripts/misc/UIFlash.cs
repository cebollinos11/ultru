using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFlash : MonoBehaviour {

    [SerializeField]
    RawImage rI;

    
    [SerializeField]float flashTime;

	// Use this for initialization
	void Start () {

       
	    
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (rI.color.a > 0f)
        {
            rI.color = new Color(rI.color.r,rI.color.g,rI.color.b,rI.color.a-Time.deltaTime/flashTime);
        }
	}

    public void Flash(Color c)
    {
        rI.color = c;
    }
}
