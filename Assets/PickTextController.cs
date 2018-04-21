using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickTextController : MonoBehaviour {

	public GameObject mercyText, danceText;
	public Transform arrow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float phase = Mathf.Sin(Mathf.PI * Time.time);
		arrow.localScale = new Vector3(Mathf.Sign(phase) * (Mathf.Abs(phase/2)+0.5f), Mathf.Abs(phase/2)+0.5f, 1);
		//mercyText.SetActive(phase > 0);
		//danceText.SetActive(phase < 0);
		mercyText.transform.localScale = Mathf.Lerp(0.5f, 1, (phase+1)/2) * Vector3.one;
		danceText.transform.localScale = Mathf.Lerp(0.5f, 1, (-phase+1)/2) * Vector3.one;
	}
}
