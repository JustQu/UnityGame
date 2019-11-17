using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
	public Animator animator;
	public float moveSpeed;
	public float attackCooldown;
	public GameObject projectilePrefab;
	public int  health = 420;

	private float attackTimer;
	private Rigidbody2D r2d;
	private bool coolDown;
	private int maxhealth;

	private void Start()
	{
		maxhealth = health;
		UIController.instance.ChangeBossHealth((float)health/maxhealth);
	}


	public void Damage(int amount)
	{
		health -= amount;
		UIController.instance.ChangeBossHealth((float)health / maxhealth);
	}
}
