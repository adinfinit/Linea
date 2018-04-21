using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour {

	public Color[] colors;
	public ColorizeEffect colorizer;

	public float bpm = 120.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		colorizer.background = colors[(int)(bpm * Time.time / 60) % colors.Length];
	}
}
