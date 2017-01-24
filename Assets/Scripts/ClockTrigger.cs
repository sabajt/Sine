using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTrigger : MonoBehaviour {

	// Generic
	public GameObject anchor;
	public GameObject canvas;

	private CameraScript cameraScript;
	private EventCanvasScript canvasScript;
	private bool playerInsideTarget = false;
	private bool isViewingTarget = false;

	// Clock
	public GameObject tube;

	private bool collectedTube = false;
	private Vector3 tubePos;

	// Use this for initialization
	void Start () {
		// Generic
		canvas.SetActive (false);
		canvasScript = canvas.GetComponent<EventCanvasScript> ();
		canvasScript.hideDetailText ();
		cameraScript = GameObject.Find ("CameraController").GetComponent<CameraScript> ();

		// clock
		Vector3 pos = tube.transform.position;
		tubePos = new Vector3 (pos.x, pos.y, pos.z - 0.2f);
	}

	void zoomOut() {
		isViewingTarget = false;
		cameraScript.unfocusTarget ();
		canvasScript.hideDetailText ();
	}

	// Update is called once per frame
	void Update () {

		if (playerInsideTarget == true) {

			// Generic Event Code
			if (Input.GetKeyDown (KeyCode.E)) {
				if (isViewingTarget == false) {
					isViewingTarget = true;

					cameraScript.focusTarget();
					canvasScript.showDetailText ();

				} else {
					isViewingTarget = false;
					cameraScript.unfocusTarget ();
					canvasScript.hideDetailText ();
				}
			}

			if (isViewingTarget) {
				cameraScript.lerpTarget (anchor.transform.position, anchor.transform.rotation, 3.2f);

				// Clock
				if (Input.GetKeyDown (KeyCode.Return)) {
					if (collectedTube) {
						zoomOut ();
					}
					collectedTube = true;
				}

				if (collectedTube) {
					tube.transform.position = Vector3.Lerp (tube.transform.position, tubePos, Time.deltaTime * 3.0f);
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
