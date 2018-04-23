using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
	public Color[] colors;
	public ColorizeEffect colorizer;

	private BackgroundMusic backgroundMusic;

	public float bpm = 120.0f;

	// Use this for initialization
	void Start ()
	{

		GameObject audioManager = GameObject.FindWithTag ("AudioManager");
		if (audioManager != null) {
			backgroundMusic = audioManager.GetComponent<BackgroundMusic> ();
		}
	}

	float musicTime ()
	{
		if (backgroundMusic != null) {
			return backgroundMusic.getTime ();
		} else {
			return Time.time;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		int index = (int)(bpm * musicTime () / 60.0f) % colors.Length;
		colorizer.background = Color.Lerp (colorizer.background, colors [index], 0.5f);
	}
}
