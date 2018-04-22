using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class PlayerController : AnimationEventTarget
{
	public float MaxSpeed = 20f;
	public float MaxFallSpeed = 40f;
	public float MinJumpVelocity = 5.0f;
	public float MaxJumpVelocity = 30.0f;
	public float MaxHangTime = 0.4f;
	public float AirborneControlForce = 100.0f;

	public float Gravity = 10.0f;

	public float GroundRadius = 0.3f;
	public float GroundSpread = 0.5f;
	public LayerMask GroundLayer;

	private Rigidbody2D body;
	private BoxCollider2D bodyCollider;

	private bool grounded = false;
	private bool hitHead = false;

	private Vector2 direction;
	private float jumpFirstTime = 0.0f;
	private float jumpStickyTime = 0.0f;
	private float jumpLastTime = 0.0f;
	private int jumpCount = 0;

	private float lastAnimationSpeed = 0.0f;
	Animator animator;

	void Awake ()
	{
		body = GetComponent<Rigidbody2D> ();
		bodyCollider = GetComponent<BoxCollider2D> ();
		animator = GetComponent<Animator> ();
		if (animator == null)
			animator = GetComponentInChildren<Animator> ();
		if (attackCollider == null)
			attackCollider = transform.Find ("AttackCollider").gameObject.GetComponent<BoxCollider2D> ();
	}

	Vector3 GroundCheck1 ()
	{
		var bounds = bodyCollider.bounds;
		Vector3 center = new Vector3 (bounds.center.x + GroundSpread, bounds.min.y - GroundRadius);
		return center;
	}

	Vector3 GroundCheck2 ()
	{
		var bounds = bodyCollider.bounds;
		Vector3 center = new Vector3 (bounds.center.x - GroundSpread, bounds.min.y - GroundRadius);
		return center;
	}

	Vector3 HeadCheck1 ()
	{
		var bounds = bodyCollider.bounds;
		Vector3 center = new Vector3 (bounds.center.x + GroundSpread, bounds.max.y + GroundRadius);
		return center;
	}

	Vector3 HeadCheck2 ()
	{
		var bounds = bodyCollider.bounds;
		Vector3 center = new Vector3 (bounds.center.x - GroundSpread, bounds.max.y + GroundRadius);
		return center;
	}

	bool hitsCircle (Vector2 pos, float radius)
	{
		return Physics2D.OverlapCircle (pos, radius, GroundLayer);
	}

	void Update ()
	{
		direction.x = Input.GetAxisRaw ("Horizontal");
		direction.y = Input.GetAxisRaw ("Vertical");

		grounded = hitsCircle (GroundCheck1 (), GroundRadius) ||
		hitsCircle (GroundCheck2 (), GroundRadius);

		hitHead = hitsCircle (HeadCheck1 (), GroundRadius) ||
		hitsCircle (HeadCheck2 (), GroundRadius);

		bool jumpJustPressedSticky = Time.fixedTime - jumpStickyTime < 0.1f;
		bool jumpJustPressed = Input.GetButtonDown ("Jump");
		if (!grounded && jumpJustPressed && !jumpJustPressedSticky) {
			jumpStickyTime = Time.fixedTime;
		}

		bool jumpPressed = Input.GetButton ("Jump") || jumpJustPressed;
		
		if (grounded && (Time.fixedTime - jumpFirstTime > 0.1f)) {
			jumpCount = 0;
			jumpFirstTime = 0;
			jumpLastTime = 0;
			jumpStickyTime = 0;
		}

		if (hitHead) {
			jumpLastTime = 0;
			jumpStickyTime = 0;
		}
		
		if (jumpJustPressed) {
			if (jumpCount == 0) {
				jumpFirstTime = Time.fixedTime;
			}
			if ((jumpCount == 0) && !grounded) {
				jumpCount++;
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

		const float JustTime = 0.1f;

		bool justJumpedFromGround = (jumpCount == 1 && (jumpTime < JustTime));
		if (grounded || justJumpedFromGround) {
			if (Mathf.Abs (direction.x) > 0.1f) {
				if (justJumpedFromGround) {
					velocity.x = Mathf.Max (velocity.x, direction.x * MaxSpeed * jumpTime / JustTime);
				} else {
					velocity.x = direction.x * MaxSpeed;
				}
			} else {
				if (!justJumpedFromGround) {
					velocity.x *= 0.8f;
				}
			}
		}
		if (!grounded) {
			var force = new Vector2 (direction.x * AirborneControlForce, 0f);
			body.AddForce (force);
		}
		 
		velocity.x = Mathf.Clamp (velocity.x, -MaxSpeed, MaxSpeed);
		velocity.y = Mathf.Clamp (velocity.y, -MaxFallSpeed, MaxFallSpeed * 2f);
		float newAnimationSpeed = Mathf.Abs (body.velocity.x) / MaxSpeed;
		body.velocity = velocity;

		lastAnimationSpeed = Mathf.Clamp (newAnimationSpeed, lastAnimationSpeed - 0.2f, lastAnimationSpeed + 0.2f);
		animator.SetFloat ("Speed", lastAnimationSpeed);

		if (Input.GetButtonDown ("Fire1")) {
			StartAttack ();
		}

		if (direction.x > 0.1f) {
			if (transform.localScale.x < 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		} else if (direction.x < -0.1f) {
			if (transform.localScale.x > 0) {
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
		}
	}

	private BoxCollider2D attackCollider;
	private bool attacked = false;

	void StartAttack ()
	{
		attacked = false;
		animator.Play ("Attack", -1, 0);
	}

	public override void Attack ()
	{
		Debug.Log ("ATTACK ATTACK ATTACK");
		if (attacked)
			return;

		Collider2D[] hits = new Collider2D[1];
		ContactFilter2D filter = new ContactFilter2D ();
		filter.SetLayerMask (1 << LayerMask.NameToLayer ("Enemy"));
		Physics2D.OverlapCollider (attackCollider, filter, hits);
		Collider2D hit = hits [0];
		if (hit == null)
			return;
		
		WalkingController enemy = hit.GetComponent<WalkingController> ();
		if (enemy == null)
			return;

		attacked = true;
		enemy.HitByPlayer ();
	}

	public void HitByEnemy ()
	{
		// TODO:
	}

	void Die ()
	{
		Destroy (gameObject);
	}

	#region Debug

	void OnDrawGizmos ()
	{
		if (bodyCollider == null) {
			bodyCollider = GetComponent<BoxCollider2D> ();
		}
		Gizmos.color = grounded ? Color.green : Color.blue;
		Gizmos.DrawSphere (GroundCheck1 (), GroundRadius);
		Gizmos.DrawSphere (GroundCheck2 (), GroundRadius);

		Gizmos.color = hitHead ? Color.green : Color.blue;
		Gizmos.DrawSphere (HeadCheck1 (), GroundRadius);
		Gizmos.DrawSphere (HeadCheck2 (), GroundRadius);
	}

	#endregion
}
