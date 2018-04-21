using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class NPCController : MonoBehaviour
{

	public float groundCheckRadius = 1f;
	public float groundCheckDistance = 0.1f;
	public float movementSpeed = 1;
	public float walkCheckDistance = 0.5f;
	public float attackDistance = 1.0f;
	public int lives = 1;

	public Animator anim;

	private bool isMovingRight;

	Vector3 velocity = new Vector3 ();

	// Use this for initialization
	void Start ()
	{
		
	}

	private bool attacked = false;

	public void StartAttack ()
	{
		attacked = false;
		anim.Play ("attack");
	}

	public void Attack ()
	{
		if (attacked)
			return;
		attacked = true;
	}

	public void HitByPlayer ()
	{
		lives--;
		if (lives <= 0) {
			// anim.Play ("death");
			Die ();
		}
	}

	public void Die ()
	{
		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update ()
	{
		if (lives <= 0)
			return;

		LayerMask levelMask = 1 << LayerMask.NameToLayer ("Default");
		LayerMask playerMask = 1 << LayerMask.NameToLayer ("Player");
		LayerMask enemyMask = 1 << LayerMask.NameToLayer ("Enemy");

		RaycastHit2D groundCheck = Physics2D.CircleCast (transform.position, groundCheckRadius, Vector2.down, groundCheckDistance, levelMask);
		if (groundCheck != null && groundCheck.distance < groundCheckDistance) {
			velocity.y *= 0.9f;
			velocity.x = movementSpeed * (isMovingRight ? 1 : -1) * (Mathf.Sin (Time.time * Mathf.PI * 2) + 2) / 2;
			
			RaycastHit2D[] walkChecks = Physics2D.CircleCastAll (
				                            transform.position + Vector3.up * 0.5f,
				                            groundCheckRadius - 0.1f,
				                            new Vector2 (isMovingRight ? 1 : -1, 0),
				                            Mathf.Max (walkCheckDistance, attackDistance),
				                            levelMask | playerMask | enemyMask
			                            );

			foreach (var check in walkChecks) {
				if (check.collider.gameObject == this.gameObject)
					continue;
				
				if (check.distance < attackDistance && check.collider.gameObject.tag == "Player") {
					StartAttack ();
					break;
				} else if (check.distance < walkCheckDistance) {
					isMovingRight = !isMovingRight;
					float xScale = Mathf.Abs (transform.localScale.x) * (isMovingRight ? -1 : 1);
					transform.localScale = new Vector3 (xScale, transform.localScale.y, transform.localScale.z);
				}
			}
		} else {
			velocity += new Vector3 (0, -1, 0);
		}

		Vector3 fallCheckOffset = Vector3.down * groundCheckRadius * 1.5f;
		fallCheckOffset += (isMovingRight ? 1 : -1) * Vector3.right * groundCheckRadius;
		var fallCheck = Physics2D.OverlapCircle (transform.position + fallCheckOffset, groundCheckDistance, levelMask);
		if (fallCheck == null) {
			isMovingRight = !isMovingRight;
			float xScale = Mathf.Abs (transform.localScale.x) * (isMovingRight ? -1 : 1);
			transform.localScale = new Vector3 (xScale, transform.localScale.y, transform.localScale.z);
		}

		Vector3 deltaPos = velocity * Time.deltaTime;
		if (groundCheck != null) {
			deltaPos.y = Mathf.Min (groundCheck.distance, Mathf.Abs (deltaPos.y)) * Mathf.Sign (deltaPos.y);
		}
		transform.position += deltaPos;
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;
		Vector3 fallCheckOffset = Vector3.down * groundCheckRadius * 1.5f;
		fallCheckOffset += (isMovingRight ? 1 : -1) * Vector3.right * groundCheckRadius;

		Gizmos.DrawSphere (transform.position + fallCheckOffset, groundCheckDistance);
	}
}
