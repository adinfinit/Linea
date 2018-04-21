using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float MaxSpeed = 10f;
	public float JumpForce = 400f;

	Animator animator;
	bool facingRight = true;

	private Rigidbody2D body;
	bool grounded = false;

	public Vector3 groundCheck;
	public float groundRadius = 0.3f;

	void Start ()
	{
		body = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
	}

	void FixedUpdate ()
	{
		float move = Input.GetAxis ("Horizontal");
		body.velocity = new Vector2 (move * MaxSpeed, body.velocity.y);
		if (Input.GetButtonDown ("Jump")) {
			body.AddForce (new Vector2 (0f, JumpForce));
		}

		if (Mathf.Abs (body.velocity.x) < 0.1f) {
			animator.Play ("stand");
		} else {
			animator.Play ("run");
		}

		if (body.velocity.x > 0.1f) {
			facingRight = true;
			if (transform.localScale.x < 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		} else if (body.velocity.x < -0.1f) {
			facingRight = false;
			if (transform.localScale.x > 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere (transform.position + groundCheck, groundRadius);
	}

	void Update ()
	{

	}
}
