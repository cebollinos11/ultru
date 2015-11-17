using UnityEngine;
using System.Collections;

public class LifeManager_Player : LifeManager {

	// Use this for initialization
	void Start () {
        currentHP = maxHP;
        GUIManager.SetHP(currentHP);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.M)) {
            ReceiveDamage(10);
            Debug.Log("life stats "+currentHP+"/"+maxHP);
        }
	
	}

    public override void ReceiveDamage(int dmg)
    {
        base.ReceiveDamage(dmg);
        GUIManager.SetHP(currentHP);

        
    }
}
