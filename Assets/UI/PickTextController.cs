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

	public DialogData beginDialog;
	public DialogData killDialog;
	public DialogData hugDialog;

	public GameObject dialogText;

	private bool waitingForChoice = false;

	// Use this for initialization
	void Start () {
		if(target != null)
			target.SetMovementEnabled(false);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			PlayerController pc = other.gameObject.GetComponent<PlayerController>();
			pc.enabled = false;
			other.GetComponentInChildren<Animator>().SetFloat("Speed", 0);

			

			if(beginDialog != null){
				StartCoroutine(DialogCoroutine());
			}
		}
	}

	public IEnumerator DialogCoroutine(){

		prompt.SetActive(true);
		prompt.GetComponent<TextMeshPro>().text = "!";
		GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
		// Do beginning dialog
		if(beginDialog != null){
			int index = 0;

			yield return new WaitForSeconds(1.0f);

			prompt.GetComponent<TextMeshPro>().text = "[SPACE]";
			dialogText.SetActive(true);
			while(index < beginDialog.speech.Length){
				Vector3 newPos = dialogText.transform.position;
				if((target == null) || ((index % 2 == 0) == !beginDialog.playerBegins)){
					newPos.x = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x;
				}else{
					newPos.x = target.transform.position.x;
				}
				dialogText.transform.position = newPos;

				dialogText.GetComponent<TextMeshPro>().text = beginDialog.speech[index];
				while(!Input.GetButtonDown("Jump")) yield return null;
				index++;
				yield return null;
			}
		}

		// Do choice
		if(target != null){
			killText.SetActive(true);
			danceText.SetActive(true);
			arrow.gameObject.SetActive(true);

			waitingForChoice = true;

			bool choseKill = false;
			while(waitingForChoice){
				float phase = Mathf.Sin(Mathf.PI * Time.time);
				arrow.localScale = new Vector3(Mathf.Sign(phase) * (Mathf.Abs(phase/2)+0.5f), Mathf.Abs(phase/2)+0.5f, 1);
				arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Mathf.PI * Time.time + Mathf.PI/3) * -7.0f);
				killText.transform.localScale = Mathf.Lerp(0.5f, 1, (phase+1)/2) * Vector3.one;
				killText.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Mathf.PI * Time.time + 2*Mathf.PI/3) * 7.0f);
				danceText.transform.localScale = Mathf.Lerp(0.5f, 1, (-phase+1)/2) * Vector3.one;
				danceText.transform.rotation = Quaternion.Euler(0, 0, phase * 7.0f);
				if(Input.GetButtonDown("Jump")){
					Debug.Log(phase > 0 ? "Kill" : "Join");
					arrow.GetComponent<SpriteRenderer>().color = selectionColor;
					if(phase > 0){
						killText.GetComponent<TextMeshPro>().faceColor = selectionColor;
						choseKill = true;
					}else{
						danceText.GetComponent<TextMeshPro>().faceColor = selectionColor;
					}
					waitingForChoice = false;
				}
				yield return null;
			}
			yield return new WaitForSeconds(1.0f);
			killText.SetActive(false);
			danceText.SetActive(false);
			arrow.gameObject.SetActive(false);

			// Do end dialog
			DialogData choiceDialog = choseKill ? killDialog : hugDialog;
			if(choiceDialog != null){
				int index = 0;
				dialogText.SetActive(true);
				while(index < choiceDialog.speech.Length){
					Vector3 newPos = dialogText.transform.position;
					if((target == null) || ((index % 2 == 0) == !choiceDialog.playerBegins)){
						newPos.x = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x;
					}else{
						newPos.x = target.transform.position.x;
					}
					dialogText.transform.position = newPos;

					dialogText.GetComponent<TextMeshPro>().text = choiceDialog.speech[index];
					while(!Input.GetButtonDown("Jump")) yield return null;
					index++;
					yield return null;
				}
			}
			prompt.SetActive(false);
			dialogText.SetActive(false);

			if(choseKill){
				target.SetMovementEnabled(true);
				player.GetComponent<PlayerController>().enabled = true;
				gameObject.SetActive(false);
			}else{
				target.GetComponent<Collider2D>().enabled = false;
				target.GetComponent<Rigidbody2D>().isKinematic = true;
				StartCoroutine(MoveToCoroutine());
				player.GetComponent<PlayerController>().minions.Add(target);
			}
		}else{
			player.GetComponent<PlayerController>().enabled = true;
			gameObject.SetActive(false);
		}
	}

	IEnumerator MoveToCoroutine(){
		GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
		target.MoveTo(player.transform.position + new Vector3(0, 1, 0));
		while((target.transform.position - player.transform.position).magnitude > 1.0f)
			yield return null;
		target.transform.parent = player.transform;
		player.GetComponent<PlayerController>().enabled = true;
		gameObject.SetActive(false);
	}
}
