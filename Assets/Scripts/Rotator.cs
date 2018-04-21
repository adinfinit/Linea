using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

	public float AngularSpeed = 60f;

	void Update ()
	{
		var angles = transform.localRotation.eulerAngles;
		angles.z += AngularSpeed * Time.deltaTime;
		transform.localRotation = Quaternion.Euler (angles);
	}
}
