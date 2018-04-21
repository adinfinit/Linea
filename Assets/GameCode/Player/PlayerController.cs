using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
	public float MaxSpeed = 20f;
	public float MinJumpVelocity = 5.0f;
	public float MaxJumpVelocity = 30.0f;
	public float MaxHangTime = 0.4f;

	public Vector3 GroundCheck = new Vector3 (0, -2f, 0);
	public float GroundRadius = 0.3f;
	public LayerMask GroundLayer;

	[Header ("State")]
	public Rigidbody2D body;
	public BoxCollider2D boxCollider;
	public bool grounded = false;

	public  Vector2 direction;
	public  float jumpStartTime = 0.0f;
	public  int jumpCount = 0;

	Animator animator;
	bool facingRight = true;

	void Awake ()
	{
		body = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		animator = GetComponent<Animator> ();
	}

	void FixedUpdate ()
	{
		direction.x = Input.GetAxis ("Horizontal");
		direction.y = Input.GetAxis ("Vertical");

		grounded = Physics2D.OverlapCircle (
			GroundCheck + transform.position, 
			GroundRadius, GroundLayer);

		bool jumpJustPressed = Input.GetButtonDown ("Jump");
		bool jumpPressed = Input.GetButton ("Jump");
		if (jumpJustPressed) {
			jumpCount++;
			jumpStartTime = Time.fixedTime;
		}

		if (!jumpPressed && grounded) {
			jumpCount = 0;
			jumpStartTime = 0;
		}
		
		float jumpTime = 0f;
		float jumpMultiplier = 0f;
		bool jumpActive = false;
		if ((jumpCount <= 2) && jumpPressed) {
			jumpTime = MaxHangTime - (Time.fixedTime - jumpStartTime);
			jumpActive = jumpTime > 0;
			jumpMultiplier = jumpTime / MaxHangTime;
		}
		
		Vector2 velocity = new Vector2 (direction.x * MaxSpeed, body.velocity.y);
		if (jumpActive) {
			velocity.y = jumpMultiplier * (MaxJumpVelocity - MinJumpVelocity) + MinJumpVelocity;
			body.gravityScale = 0.1f;
		} else {
			if (velocity.y > 0.0f) {
				velocity.y *= 0.9f;
			}
			body.gravityScale = 12.0f;
		}

		body.velocity = velocity;

		if (Mathf.Abs (velocity.x) < 0.1f) {
			animator.Play ("stand");
		} else {
			animator.Play ("run");
		}

		if (velocity.x > 0.1f) {
			facingRight = true;
			if (transform.localScale.x < 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		} else if (velocity.x < -0.1f) {
			facingRight = false;
			if (transform.localScale.x > 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = grounded ? Color.green : Color.blue;
		Gizmos.DrawSphere (transform.position + GroundCheck, GroundRadius);
	}

	void Update ()
	{

	}
}
