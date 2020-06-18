using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public string characterName;
    public int health;
    public int maxHealth;
    public int attackPower;
    public int defencePower;
    public int manaPoints; // public int maxMana; missing?
    public List<Spell> spells;

    public void Hurt(int amount)
    {
        int damageAmount = amount; // Random.Range(0,1) * (amount - defencePower); original tutuorial didn't work?
        health = Mathf.Max(health - damageAmount, 0); // make health equal to remaining health after taking damage or 0 whichever is higher
        if (health == 0)
        {
            Die(); // die
        }
    }

    public void Heal(int amount)
    {
        int healAmount = amount; //Random.Range(0, 1) * (int)(amount + (maxHealth*.33f));
        health = Mathf.Min(health + healAmount, maxHealth); // make health equal to max health or current health or whichever is lower 
        
    }
    public void Defend()
    {
        defencePower += (int)(defencePower * 0.33f) ;
        Debug.Log("Def Increased.");
        BattleController.Instance.NextAct();
    }

    public bool CastSpell(Spell spell, Character targetCharacter)
    {
        bool successful = manaPoints >= spell.manaCost; // comparison to return true or false depending on whether or not our current manaPoints are greater than manaCost

        if (successful)
        {
            Spell spellToCast = Instantiate<Spell>(spell, transform.position, Quaternion.identity); // Quaternion gives the default rotation of the object
            manaPoints -= spell.manaCost;
            spellToCast.Cast(targetCharacter);
            Debug.Log("Spell success");
        }            

        return successful;
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
        Debug.LogFormat("{0} has died!", characterName);
    }
}
