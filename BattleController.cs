using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {
    public static BattleController Instance { get; set; } // allows you to access anything that is public

    public Dictionary<int, List<Character>> characters = new Dictionary<int, List<Character>>();    //      players     enemies
    public int characterTurnIndex;                                                                  //      0           1
    public Spell playerSelectedSpell;                                                               //      0 1 2       0 1 2
    public bool playerIsAttack;

    [SerializeField]private BattleSpawnPoint[] spawnPoints;

    [SerializeField]private BattleUIController uIController;

    private int actTurn;    
                                                                                                    
    private void Start()                                                                            
    {
        if(Instance != null && Instance != this) // to protect from having two instances of BattleController which would confuse our references
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        characters.Add(0, new List<Character>());
        characters.Add(1, new List<Character>());
        FindObjectOfType<BattleLauncher>().Launch();
    }

    public Character GetRandomPlayer()
    {
        return characters[0][Random.Range(0, characters[0].Count - 1)];     // returns a random character from character dictionary
    }

    public Character GetWeakestEnemy()
    {
        Character weakestEnemy = characters[1][0];
        foreach(Character character in characters[1])
        {
            if(character.health < weakestEnemy.health)
            {
                weakestEnemy = character;
            }
        }
        return weakestEnemy;
    }

    private void NextTurn()
    {
        actTurn = actTurn == 0 ? 1 : 0; // inline turnery (sp?) operator what to do based on the condition
    }

    public void NextAct()
    {
        
        if (characters[0].Count > 0 && characters[1].Count > 0)
        {
            //Debug.Log(characters[0].Count + characters[1].Count +"count");
            if (characterTurnIndex < characters[actTurn].Count - 1)
            {
                //Debug.Log(characterTurnIndex +"ti");
                characterTurnIndex++;
            }
            else
            {
                NextTurn();
                characterTurnIndex = 0;
                Debug.Log("turn: " + actTurn);
            }

            switch (actTurn)
            {
                
                case 0:
                    
                    uIController.ToggleActionState(true); // do ui stuff; turns on our buttons so we can act
                    uIController.BuildSpellList(GetCurrentCharacter().spells);
                    break;
                case 1:
                    StartCoroutine(PerformAct());       // special execution method for coroutine
                    uIController.ToggleActionState(false); // do ui stuff and act ; turns off our buttons so we can't act during enemy team
                    break;
            }
        }
        else
        {
            Debug.Log("Battle over!");
        }
    }

    IEnumerator PerformAct()        // to use coroutine
    {
        yield return new WaitForSeconds(.75f);      
        if (GetCurrentCharacter().health> 0)            // check to see if you're alive
        {
            GetCurrentCharacter().GetComponent<Enemy>().Act();      // if you're alive then act
        }
        uIController.UpdateCharacterUI();
        yield return new WaitForSeconds(1f);
        NextAct(); //Lesson 10 says we don't need this but it was in the tutorial when we set it up in Lesson 6 @ 10:45
    }

    public void SelectCharacter(Character character) // passing in the target character
    {
        if (playerIsAttack)
        {
            DoAttack(GetCurrentCharacter(), character);
            uIController.UpdateCharacterUI();
            NextAct();
        }
        else if (playerSelectedSpell != null)
        {
            if(GetCurrentCharacter().CastSpell(playerSelectedSpell, character))  // spell is cast on target character
            {
                uIController.UpdateCharacterUI();
                NextAct();
            }
            else
            {
                Debug.LogWarning("Not enough mana to cast that spell!");
            }
        }
    }

    public void DoAttack(Character attacker, Character target)
    {
        Debug.Log("Do Attack");
        target.Hurt(attacker.attackPower);
        NextAct();
    }

    public void StartBattle(List<Character> players, List<Character> enemies) // pass in a list of characters through BattleSpawnPoint
    {
        Debug.Log("Setup Battle!");
        for (int i =0; i < players.Count; i++)
        {
            characters[0].Add(spawnPoints[i+3].Spawn(players[i])); // accessing the character dictionary and adding the spawn
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            characters[1].Add(spawnPoints[i].Spawn(enemies[i]));
            
        }
    }

    public Character GetCurrentCharacter()
    {
        return characters[actTurn][characterTurnIndex];
        
    }
}
