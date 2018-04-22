using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	Vector3 initialScale;
	Animator animator;

	void Start ()
	{
		animator = GetComponentInChildren<Animator> ();
		initialScale = animator.transform.localScale;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.transform.tag != "Player")
			return;

		PlayerController controller = other.gameObject.GetComponent<PlayerController> ();
		if (controller == null)
			return;

		controller.SetCheckpoint (this);
		animator.Play ("active");
	}

	public void PlayerFoundNew ()
	{
		animator.Play ("inactive");
	}
}
