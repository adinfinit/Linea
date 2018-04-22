using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBehavior : AnimationEventTarget
{
	Collider2D attackCollider;
	Animator animator;

	void Start ()
	{
		if (attackCollider == null)
			attackCollider = transform.Find ("AttackCollider").gameObject.GetComponent<Collider2D> ();

		animator = GetComponentInChildren<Animator> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.transform.tag != "Player")
			return;
		animator.Play ("attack");
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.transform.tag != "Player")
			return;
		animator.Play ("idle");
	}

	override public void Attack ()
	{
		if (attackCollider == null)
			return;
		
		Collider2D[] hits = new Collider2D[1];
		ContactFilter2D filter = new ContactFilter2D ();
		filter.SetLayerMask (1 << LayerMask.NameToLayer ("Player"));
		Physics2D.OverlapCollider (attackCollider, filter, hits);
		Collider2D hit = hits [0];
		if (hit == null)
			return;

		PlayerController player = hit.GetComponent<PlayerController> ();
		if (player == null)
			return;
		
		player.HitByEnemy ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
