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
	public float AirborneControlForce = 100.0f;

	public float Gravity = 10.0f;

	public Vector3 GroundCheck = new Vector3 (0, -2f, 0);
	public float GroundRadius = 0.3f;
	public float GroundSpread = 0.5f;
	public LayerMask GroundLayer;

	[Header ("State")]
	private Rigidbody2D body;
	private BoxCollider2D boxCollider;
	private bool grounded = false;
	private bool hitHead = false;

	private Vector2 direction;
	private float jumpFirstTime = 0.0f;
	private float jumpStickyTime = 0.0f;
	private float jumpLastTime = 0.0f;
	private int jumpCount = 0;

	private float lastAnimationSpeed = 0.0f;

	Animator animator;
	bool facingRight = true;

	void Awake ()
	{
		body = GetComponent<Rigidbody2D> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		animator = GetComponent<Animator> ();
	}

	Vector3 GroundCheck1(){
		Vector3 center = transform.position + GroundCheck;
		center.x += GroundSpread;
		return center;
	}

	Vector3 GroundCheck2(){
		Vector3 center = transform.position + GroundCheck;
		center.x -= GroundSpread;
		return center;
	}
	
	Vector3 HeadCheck1(){
		Vector3 head = GroundCheck;
		head.y = -head.y;
		Vector3 center = transform.position + head;
		center.x += GroundSpread;
		return center;
	}

	Vector3 HeadCheck2(){
		Vector3 head = GroundCheck;
		head.y = -head.y;
		Vector3 center = transform.position + head;
		center.x -= GroundSpread;
		return center;
	}
	
	void Update ()
	{
		direction.x = Input.GetAxisRaw ("Horizontal");
		direction.y = Input.GetAxisRaw ("Vertical");

		grounded = Physics2D.OverlapCircle(GroundCheck1(), GroundRadius, GroundLayer) || 
			Physics2D.OverlapCircle(GroundCheck2(), GroundRadius, GroundLayer);

		hitHead = Physics2D.OverlapCircle(HeadCheck1(), GroundRadius, GroundLayer) || 
			Physics2D.OverlapCircle(HeadCheck2(), GroundRadius, GroundLayer);

		bool jumpJustPressedSticky = Time.fixedTime - jumpStickyTime < 0.1f;
		bool jumpJustPressed = Input.GetButtonDown ("Jump");
		if(!grounded && jumpJustPressed && !jumpJustPressedSticky){
			jumpStickyTime = Time.fixedTime;
		}

		bool jumpPressed = Input.GetButton ("Jump") || jumpJustPressed;
		
		if (grounded && (Time.fixedTime - jumpFirstTime > 0.1f)) {
			jumpCount = 0;
			jumpFirstTime = 0;
			jumpLastTime = 0;
			jumpStickyTime = 0;
		}

		if(hitHead){
			jumpLastTime = 0;
			jumpStickyTime = 0;
		}
		
		if (jumpJustPressed) {
			if(jumpCount == 0){
				jumpFirstTime = Time.fixedTime;
			}
			jumpCount++;
			jumpLastTime = Time.fixedTime;
		}

		float jumpTime = Time.fixedTime - jumpLastTime;
		float jumpMultiplier = 0f;
		bool jumpActive = false;
		if ((jumpCount <= 2) && jumpPressed) {
			jumpActive = MaxHangTime - jumpTime > 0;
			jumpMultiplier = (MaxHangTime - jumpTime) / MaxHangTime;
		}
		
		Vector2 velocity = body.velocity;
		// update jumping info
		if (jumpActive) {
			velocity.y = jumpMultiplier * (MaxJumpVelocity - MinJumpVelocity) + MinJumpVelocity;
			body.gravityScale = 0.1f;
		} else {
			if (velocity.y > 0.0f) {
				velocity.y *= 0.9f;
			}
			body.gravityScale = 12.0f;
		}

		const float JustTime = 0.2f;

		bool justJumpedFromGround = (jumpCount == 1 && (jumpTime < JustTime));
		if (grounded || justJumpedFromGround) {
			if(Mathf.Abs(direction.x) > 0.1f){
				if(justJumpedFromGround) {
					velocity.x = Mathf.Max(velocity.x, direction.x * MaxSpeed * jumpTime / JustTime);
				} else {
					velocity.x = direction.x * MaxSpeed;
				}
			} else {
				if(!justJumpedFromGround){
					velocity.x *= 0.8f;
				}
			}
		}
		if(!grounded) {
			var force = new Vector2(direction.x * AirborneControlForce, 0f);
			body.AddForce(force);
		}

		velocity.x = Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed);

		float newAnimationSpeed = Mathf.Abs(body.velocity.x) / MaxSpeed;
		lastAnimationSpeed = Mathf.Clamp(newAnimationSpeed, lastAnimationSpeed -0.2f,  lastAnimationSpeed +0.2f);
		animator.SetFloat("Speed", lastAnimationSpeed);

		body.velocity = velocity;

		if (direction.x > 0.1f) {
			facingRight = true;
			if (transform.localScale.x < 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		} else if (direction.x < -0.1f) {
			facingRight = false;
			if (transform.localScale.x > 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = grounded ? Color.green : Color.blue;
		Gizmos.DrawSphere (GroundCheck1(), GroundRadius);
		Gizmos.DrawSphere (GroundCheck2(), GroundRadius);

		Gizmos.color = hitHead ? Color.green : Color.blue;
		Gizmos.DrawSphere (HeadCheck1(), GroundRadius);
		Gizmos.DrawSphere (HeadCheck2(), GroundRadius);
	}
}
