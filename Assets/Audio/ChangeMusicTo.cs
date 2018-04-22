using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicTo : MonoBehaviour
{

	public AudioClip clip;
	private BackgroundMusic backgroundMusic;

	void Start ()
	{
		GameObject audioManager = GameObject.FindWithTag ("AudioManager");
		if (audioManager != null)
			backgroundMusic = audioManager.GetComponent<BackgroundMusic> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.transform.tag != "Player")
			return;
		if (backgroundMusic != null)
			backgroundMusic.ChangeTo (clip);
	}

}
