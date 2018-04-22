using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class PickTextController : MonoBehaviour {

	public GameObject killText, danceText, prompt;
	public Transform arrow;
	public NPCBase target;
	public Color selectionColor;

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
			arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Mathf.PI * Time.time + Mathf.PI/3) * -7.0f);
			killText.transform.localScale = Mathf.Lerp(0.5f, 1, (phase+1)/2) * Vector3.one;
			killText.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Mathf.PI * Time.time + 2*Mathf.PI/3) * 7.0f);
			danceText.transform.localScale = Mathf.Lerp(0.5f, 1, (-phase+1)/2) * Vector3.one;
			danceText.transform.rotation = Quaternion.Euler(0, 0, phase * 7.0f);
			if(Input.GetButton("Jump")){
				Debug.Log(phase > 0 ? "Kill" : "Join");
				arrow.GetComponent<SpriteRenderer>().color = selectionColor;
				if(phase > 0){
					killText.GetComponent<TextMeshPro>().faceColor = selectionColor;
				}else{
					danceText.GetComponent<TextMeshPro>().faceColor = selectionColor;
				}
				waitingForChoice = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			PlayerController pc = other.gameObject.GetComponent<PlayerController>();
			pc.enabled = false;
			other.GetComponentInChildren<Animator>().SetFloat("Speed", 0);

			killText.SetActive(true);
			danceText.SetActive(true);
			prompt.SetActive(true);
			arrow.gameObject.SetActive(true);
			waitingForChoice = true;
		}
	}
}
