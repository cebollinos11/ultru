using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {

    protected int maxHP;
    protected int currentHP;

    public virtual void Init(int maxHP) {
        this.maxHP = maxHP;
        currentHP = maxHP;
    }
    
    public virtual void ReceiveDamage(int dmg) { 
    
        //play some hit particle system + sounds
        currentHP -= dmg;
        if (currentHP <= 0) {
            Destroy(gameObject);
        }
    }

    public virtual void Heal(int dmg) {

        currentHP += dmg;
        if (currentHP > maxHP) {
            currentHP = maxHP;
        }
    }

}
