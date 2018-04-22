using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
	private Animator animator;

	void Start ()
	{
		animator = GetComponentInChildren<Animator> ();
		animator.SetFloat ("Random", Random.Range (0.0f, 1.0f));
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
