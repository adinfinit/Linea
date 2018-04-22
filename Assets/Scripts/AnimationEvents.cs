using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
	public AnimationEventTarget Target;
	private SFXManager sfxmanager;

	void Awake ()
	{
		Target = transform.parent.gameObject.GetComponent<AnimationEventTarget> ();
		if (Target == null) {
			Debug.LogError ("AnimationEventTarget missing");
		}

		GameObject audioManager = GameObject.FindWithTag ("AudioManager");
		if (audioManager != null)
			sfxmanager = audioManager.GetComponent<SFXManager> ();
	}

	public void Attack ()
	{
		Target.Attack ();
	}

	public void Die ()
	{
		Target.Die ();
	}

	public void PlayClip (AudioClip sound)
	{
		sfxmanager.PlaySFX (sound);
	}
}
