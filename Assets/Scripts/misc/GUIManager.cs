using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager> {

    public Text interactableLabelText;
    public Text Hp;
    public UIFlash uiFlash;

	// Use this for initialization
	void Start () {

        
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void SetHP(int str)
    {
        Instance.Hp.text = str.ToString();
        if (str < 31)
        {
            Instance.Hp.color = Color.red;
        }
        else {
            Instance.Hp.color = Color.green;
        }

        
        
    }
}
