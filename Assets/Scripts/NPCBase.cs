using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCBase : AnimationEventTarget
{
	public bool MovementEnabled = true;

	public virtual void SetMovementEnabled (bool enabled)
	{
		MovementEnabled = enabled;	
	}

	public virtual bool GetMovementEnabled ()
	{
		return MovementEnabled;
	}

	public virtual void MoveTo(Vector3 target){
		transform.position = target;
	}
}
