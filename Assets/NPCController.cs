using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NPCController : MonoBehaviour {

	public float groundCheckRadius = 2;
	public float groundCheckDistance = 0.1f;

	Vector3 velocity = new Vector3();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D groundCheck = Physics2D.CircleCast(transform.position, groundCheckRadius, Vector2.down);
		if(groundCheck != null && groundCheck.distance < groundCheckDistance){
			Debug.Log(groundCheck.distance);
			velocity += Physics.gravity;
		}else{
			velocity.y = 0;
		}

		Vector3 deltaPos = velocity * Time.deltaTime;
		if(groundCheck != null){
			deltaPos.y = Mathf.Min(groundCheck.distance, deltaPos.y);
		}
		transform.position += deltaPos;
	}
}
