using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillRegion : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.transform.tag != "Player")
			return;

		PlayerController controller = other.gameObject.GetComponent<PlayerController> ();
		if (controller == null)
			return;

		controller.Kill ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
