﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class LifeManager_Player : LifeManager {

    [SerializeField]
    int _maxHP;
    public int dmgReduction;

	// Use this for initialization
	void Start () {
        maxHP = _maxHP;
        currentHP = maxHP;
        GUIManager.SetHP(currentHP);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.M)) {
            ReceiveDamage(10, Vector3.zero);
            
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Heal(10);
            
        }
	
	}

    public override void ReceiveDamage(int dmg, Vector3 hitPos)
    {
        base.ReceiveDamage(dmg*(100-dmgReduction)/100, hitPos);
        GUIManager.SetHP(currentHP);
        GUIManager.Instance.uiFlash.Flash(Color.red);        
    }

    public override void Heal(int dmg)
    {
        base.Heal(dmg);
        GUIManager.SetHP(currentHP);
        GUIManager.Instance.uiFlash.Flash(Color.green);
        GetComponent<FirstPersonController>().remainingStamina = 20; //hardcode

    }
}
