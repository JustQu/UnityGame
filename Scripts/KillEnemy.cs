using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    /* тут надо будет все поменять */
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.CompareTag("Player")){
			Destroy(this.gameObject);
		}
	}
}
