using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NPCController : MonoBehaviour {

	public float groundCheckRadius = 1f;
	public float groundCheckDistance = 0.1f;
	public float movementSpeed = 1;
	public float walkCheckDistance = 0.5f;
	public float attackDistance = 1.0f;

	public Animator anim;

	private bool isMovingRight;

	Vector3 velocity = new Vector3();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D[] groundCheck = Physics2D.CircleCastAll(transform.position, groundCheckRadius, Vector2.down);
		if(groundCheck.Length > 1 && groundCheck[1].distance <= groundCheckDistance){
			velocity.y = 0;
			velocity.x = movementSpeed * (isMovingRight ? 1 : -1);
			RaycastHit2D[] walkCheck = Physics2D.CircleCastAll(transform.position, groundCheckRadius-0.1f, new Vector2(isMovingRight ? 1 : -1, 0));

			if(walkCheck.Length > 1){
				if(walkCheck[1].distance < attackDistance && walkCheck[1].collider.gameObject.tag == "Player"){
					anim.Play("attack");
				}else if(walkCheck[1].distance <= walkCheckDistance){
					Debug.Log(walkCheck[1].collider.gameObject);
					isMovingRight = !isMovingRight;
					float xScale = Mathf.Abs(transform.localScale.x) * (isMovingRight ? -1 : 1);
					transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
				}
			}
		}else{
			//Debug.Log(groundCheck[1].distance);
			velocity += new Vector3(0, -1, 0);
		}

		Vector3 deltaPos = velocity * Time.deltaTime;
		if(groundCheck.Length > 1){
			deltaPos.y = Mathf.Min(groundCheck[1].distance, Mathf.Abs(deltaPos.y))*Mathf.Sign(deltaPos.y);
		}
		transform.position += deltaPos;
	}
}
