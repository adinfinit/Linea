using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

public class SFXManager : MonoBehaviour
{
	public AudioMixerGroup mixer;

	[Range (1, 50)] public int numSFXPlayers = 10;

	private AudioSource[] sfxPlayers;
	private int sfxIndex = 0;

	// Use this for initialization
	void Start ()
	{
		sfxPlayers = new AudioSource[numSFXPlayers];

		for (int i = 0; i < this.numSFXPlayers; i++){
			sfxPlayers [i] = gameObject.AddComponent<AudioSource> ();
			sfxPlayers [i].outputAudioMixerGroup = mixer;
		}
	}


	public void PlaySFX (AudioClip c)
	{
		sfxIndex = (sfxIndex + 1) % numSFXPlayers;
		AudioSource player = sfxPlayers [sfxIndex];

		player.clip = c; // set the clip to secondary audio player
		player.time = 0f;
		player.Play ();
	}
}