using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpawnPoint : MonoBehaviour {
    public Character Spawn(Character character)
    {
        Character characterToSpawn = Instantiate<Character>(character, this.transform); // passes in the input parameter character, with the parent SpawnPoint game object
        return characterToSpawn;
    }
	
}
