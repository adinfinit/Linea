using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
	public AnimationEventTarget Target;

	void Awake ()
	{
		Target = transform.parent.gameObject.GetComponent<AnimationEventTarget> ();
		if (Target == null) {
			Debug.LogError ("AnimationEventTarget missing");
		}
	}

	public void Attack ()
	{
		Target.Attack ();
	}

	public void Die ()
	{
		Target.Die ();
	}
}
