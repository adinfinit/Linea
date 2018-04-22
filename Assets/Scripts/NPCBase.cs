using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCBase : MonoBehaviour {

	public abstract void SetMovementEnabled(bool enabled);
	public abstract bool GetMovementEnabled();

}
