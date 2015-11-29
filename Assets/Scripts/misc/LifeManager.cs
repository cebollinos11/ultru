using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {

    public delegate void OnDamage(Vector3 hitPos);
    public delegate void OnDeath();
    public OnDamage onDamage;
    public OnDeath onDeath;

    [SerializeField] protected int maxHP;
    public int currentHP;

    public virtual void Init(int maxHP) {
        this.maxHP = maxHP;
        currentHP = maxHP;
    }
    
    public virtual void ReceiveDamage(int dmg, Vector3 hitPos) {

        //play some hit particle system + sounds
        currentHP -= dmg;
        if (currentHP <= 0) {
            if (onDeath != null) {
                onDeath();
            }
        }
        else {
            if (onDamage != null) {
                onDamage(hitPos);
            }
        }
        
    }

    public virtual void Heal(int dmg) {
        currentHP += dmg;
        if (currentHP > maxHP) {
            currentHP = maxHP;
        }
    }

}
