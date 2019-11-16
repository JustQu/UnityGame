using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Player")){
			GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
			DungeonGenerator dungeonGenerator = dungeon.GetComponent<DungeonGenerator>();
			Room room = dungeonGenerator.CurrentRoom();
			dungeonGenerator.MoveToRoom(room.Neighbor((this.name[5]).ToString()));
			SceneManager.LoadScene("Demo");
		}
	}
}
