using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	private Vector3  target;

	internal void Initialize(Vector3 target)
	{
		this.target = target;
	}

	private void Update()
	{
		//потом доделать
		if (target != null){
			transform.position = Vector3.MoveTowards(transform.position, target, -1);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Enemy")){
			//
		}
	}
}