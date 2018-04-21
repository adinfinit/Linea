using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickTextController : MonoBehaviour {

	public GameObject mercyText, danceText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float phase = Mathf.Sin(Mathf.PI * Time.time);
		//transform.localScale = new Vector3(Mathf.Sign(phase), 1, 1);
		mercyText.SetActive(phase > 0);
		danceText.SetActive(phase < 0);
	}
}
