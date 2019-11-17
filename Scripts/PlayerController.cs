using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
	public int health = 100;

    private Vector2 movement;
	static private Vector2 playerPosition;
	private int maxhealth = 100;

	private void Awake()
	{
		
	}

    // Update is called once per frame
    void Update()
    {
        movement.x =  Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        animator.SetFloat("Speed", movement.sqrMagnitude);
        if (movement.magnitude > 1)
            movement /= movement.magnitude;

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

	public void Damage(int amount)
	{
		health -= amount;
		UIController.instance.ChangePlayerHealth((float)health / maxhealth);

	}
}
