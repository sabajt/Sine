using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioTrigger : MonoBehaviour {

	// Generic
	public GameObject anchor;
	public GameObject canvas;

	private CameraScript cameraScript;
	private EventCanvasScript canvasScript;
	private bool playerInsideTarget = false;
	private bool isViewingTarget = false;

	// Radio
	public List<AudioClip> audioClips = new List<AudioClip>();
	private int radioIndex = 0;
	const int radioIndexMax = 3;

	// Use this for initialization
	void Start () {
		// Generic
		canvas.SetActive (false);
		canvasScript = canvas.GetComponent<EventCanvasScript> ();
		canvasScript.hideDetailText ();
		cameraScript = GameObject.Find ("CameraController").GetComponent<CameraScript> ();
	}

	// Update is called once per frame
	void Update () {

		if (playerInsideTarget) {

			// Generic Event Code
			if (Input.GetKeyDown (KeyCode.E)) {
				if (isViewingTarget == false) {
					isViewingTarget = true;

					cameraScript.FocusTarget();
					canvasScript.showDetailText ();

				} else {
					isViewingTarget = false;
					cameraScript.UnfocusTarget ();
					canvasScript.hideDetailText ();
				}
			}

			if (isViewingTarget) {
				cameraScript.LerpTarget (anchor.transform.position, anchor.transform.rotation, 3.2f);

				// Radio station selection
				if (Input.GetKeyDown (KeyCode.Return)) {
					radioIndex++;
					if (radioIndex > radioIndexMax) {
						radioIndex = 0;
					}

					AudioSource audio = GetComponent<AudioSource>();
					audio.clip = audioClips [radioIndex];
					audio.Play();
					Debug.Log ("playing audio: " + audio.clip.name);
				}
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		playerInsideTarget = other.gameObject.name == "FPSController";
		canvas.SetActive(playerInsideTarget);
	}

	void OnTriggerExit( Collider other ){
		playerInsideTarget = other.gameObject.name != "FPSController";
		canvas.SetActive(playerInsideTarget);
	}
}
