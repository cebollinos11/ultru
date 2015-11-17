using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {

    public int maxHP = 100;
    public int currentHP;



	// Use this for initialization
	void Start () {
        currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void ReceiveDamage(int dmg) { 
    
        //play some hit particle system + sounds
        currentHP -= dmg;

    }

}
