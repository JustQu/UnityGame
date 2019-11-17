using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Player")){
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			if (enemies.Length == 0){
				GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
				DungeonGenerator dungeonGenerator = dungeon.GetComponent<DungeonGenerator>();
				dungeonGenerator.ResetDungeon();
				SceneManager.LoadScene("Demo");
			}
		}
	}
}
