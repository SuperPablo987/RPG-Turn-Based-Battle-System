using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {
    public string spellName;
    public int power;
    public int manaCost;
    public enum SpellType { Attack, Heal }
    public SpellType spellType;

    private Vector3 targetPosition;

    private void Update()
    {
        if(targetPosition != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, .15f); // visually moving the spell across the screen
            if (Vector3.Distance(transform.position, targetPosition) < 0.25f)
            {
                Destroy(this.gameObject, 1);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Cast(Character target) // initially set up as GameObject but changed after we define Character class
    {
        Debug.Log(target.name);
        targetPosition = target.transform.position;
        Debug.Log(spellName + " was cast on " + target.name + "!");
        if(spellType == SpellType.Attack)
        {
            target.Hurt(power); // hurt character
        }
        else if(spellType == SpellType.Heal)
        {
            target.Heal(power); // heal
        }
    }
}
