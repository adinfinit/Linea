﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
	public static BackgroundMusic MainInstance = null;

	public AudioClip primaryTrack;

	public AnimationCurve crossfadeCurve;
	public float crossfadeDuration = 3f;
	[Range (0f, 1f)] private float crossfade = 0.0f;

	[Range (0f, 1f)] public float volume = 1f;

	private AudioSource primary;
	private AudioSource secondary;

	void Start ()
	{
		if (MainInstance != null) {
			Destroy (gameObject);
			return;
		}
		MainInstance = this;
		DontDestroyOnLoad (gameObject);

		primary = gameObject.AddComponent<AudioSource> ();
		primary.loop = true;

		secondary = gameObject.AddComponent<AudioSource> ();
		secondary.loop = true;

		primary.clip = primaryTrack;
	}

	public void ChangeTo (AudioClip song)
	{
		if (primary.clip == song)
			return;
		if (secondary.clip == song)
			return;
		InternalChangeTo (song);
	}

	void InternalChangeTo (AudioClip song)
	{
		secondary.clip = song;
		secondary.volume = 0.0f;
		secondary.time = 0.0f;
		secondary.Play ();

		crossfade = 0.0f;
	}

	void Update ()
	{
		if (secondary.clip == null) {
			primary.volume = volume;
			return;
		}
		crossfade += Time.deltaTime / crossfadeDuration;

		float p = crossfadeCurve.Evaluate (crossfade);
		p = Mathf.Clamp01 (p);

		primary.volume = (1.0f - p) * volume;
		secondary.volume = (p) * volume;

		if (p >= 1.0f) {
			AudioSource tmp = secondary;
			secondary = primary;
			primary = tmp;

			secondary.Stop ();
			secondary.time = 0.0f;
			secondary.volume = 0.0f;
			secondary.clip = null;
		}
	}
}
