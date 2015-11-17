using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {

    protected int maxHP;
    protected int currentHP;



	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void ReceiveDamage(int dmg) { 
    
        //play some hit particle system + sounds
        currentHP -= dmg;

    }

    public virtual void Heal(int dmg) {

        currentHP += dmg;
        if (currentHP > maxHP) {
            currentHP = maxHP;
        }
    }

}
