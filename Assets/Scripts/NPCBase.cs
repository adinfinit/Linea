using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCBase : AnimationEventTarget
{
	public bool MovementEnabled = true;

	private bool animating = false;
	private float animationTime = 0.0f;
	private Vector3 animationStart;
	private Vector3 animationFinish;

	public virtual void SetMovementEnabled (bool enabled)
	{
		MovementEnabled = enabled;	
	}

	public virtual bool GetMovementEnabled ()
	{
		if (animating) {
			animationTime += Time.deltaTime;
			transform.position = Vector3.Lerp (animationStart, animationFinish, animationTime);
			if (animationTime > 1.0f) {
				animating = false;
				return MovementEnabled;
			}
			return false;
		}
		return MovementEnabled;
	}

	public virtual void MoveTo (Vector3 target)
	{
		animationStart = transform.position;
		animationFinish = target;
		animating = true;
		animationTime = 0.0f;
	}
}
