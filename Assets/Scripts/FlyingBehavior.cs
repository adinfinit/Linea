using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBehavior : MonoBehaviour
{
	private Vector3 center;
	public Vector3 size;

	public float speed = 0.1f;

	public GameObject LightningPrefab;
	private bool started = false;

	void Start ()
	{
		center = transform.position;
		started = true;
	}

	void Update ()
	{
		float x = size.x * Mathf.Sin (Time.time * 2.300f * speed);
		float y = size.y * Mathf.Sin (Time.time * 3.415f * speed);

		transform.position = center + new Vector3 (x, y, 0);
	}

	void OnDrawGizmos ()
	{
		if (started)
			Gizmos.DrawWireCube (center, size);
		else
			Gizmos.DrawWireCube (transform.position, size);
	}
}
