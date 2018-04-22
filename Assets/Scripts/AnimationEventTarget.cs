using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTarget : MonoBehaviour
{
	public virtual void Attack ()
	{
	}

	public virtual void Die ()
	{
		Destroy (gameObject);
	}
}
