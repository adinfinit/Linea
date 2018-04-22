using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class PickTextController : MonoBehaviour {

	public GameObject mercyText, danceText;
	public Transform arrow;
	public NPCBase target;

	private bool waitingForChoice = false;

	// Use this for initialization
	void Start () {
		target.SetMovementEnabled(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(waitingForChoice){
			float phase = Mathf.Sin(Mathf.PI * Time.time);
			arrow.localScale = new Vector3(Mathf.Sign(phase) * (Mathf.Abs(phase/2)+0.5f), Mathf.Abs(phase/2)+0.5f, 1);
			mercyText.transform.localScale = Mathf.Lerp(0.5f, 1, (phase+1)/2) * Vector3.one;
			danceText.transform.localScale = Mathf.Lerp(0.5f, 1, (-phase+1)/2) * Vector3.one;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			PlayerController pc = other.gameObject.GetComponent<PlayerController>();
			pc.enabled = false;
			other.GetComponentInChildren<Animator>().SetFloat("Speed", 0);

			mercyText.SetActive(true);
			danceText.SetActive(true);
			arrow.gameObject.SetActive(true);
		}
	}
}
