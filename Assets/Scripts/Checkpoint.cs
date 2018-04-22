using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	Vector3 initialScale;
	GameObject animator;

	void Start ()
	{
		animator = transform.Find ("AnimatorStub").gameObject;
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

		animator.transform.localScale = initialScale * 2;
	}

	public void PlayerFoundNew ()
	{
		animator.transform.localScale = initialScale;
	}
}
