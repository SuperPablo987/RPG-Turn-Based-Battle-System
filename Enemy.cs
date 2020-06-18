using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
    public void Act()
    {
        int dieRoll = Random.Range(0, 2);
        Character target = BattleController.Instance.GetRandomPlayer();   // finds random target
        switch (dieRoll)
        {
            case 0:
                Defend();
                break;
            case 1:
                Spell spellToCast = GetRandomSpell();// spell
                if (spellToCast.spellType == Spell.SpellType.Heal)
                {
                    target = BattleController.Instance.GetWeakestEnemy();// get friendly weak target
                }
                if (!CastSpell(spellToCast, null))
                {
                    BattleController.Instance.DoAttack(this, target);  // attack random player selected above
                }
                break;
            case 2:
                BattleController.Instance.DoAttack(this, target);  // attack
                break;
        }
    }

    Spell GetRandomSpell()
    {
        return spells[Random.Range(0, spells.Count - 1)];
    }

    public override void Die()
    {
        base.Die();
        BattleController.Instance.characters[1].Remove(this);
    }

}
