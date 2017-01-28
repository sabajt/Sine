using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionTrigger : MonoBehaviour {

	public GameObject anchor;
	public GameObject canvas;

	private CameraScript cameraScript;
	private EventCanvasScript canvasScript;
	private bool playerInsideTarget = false;
	private bool isViewingTarget = false;

	// Delegates
	public delegate void StartViewTarget();
	public static event StartViewTarget OnStartViewTarget;

	public delegate void EndViewTarget();
	public static event EndViewTarget OnEndViewTarget;

	public delegate void UpdateViewingTarget();
	public static event UpdateViewingTarget OnUpdateViewingTarget;

	void Start () {
		canvas.SetActive (false);
		canvasScript = canvas.GetComponent<EventCanvasScript> ();
		canvasScript.hideDetailText ();
		cameraScript = GameObject.Find ("CameraController").GetComponent<CameraScript> ();
	}
	
	void Update () {
		if (playerInsideTarget == true) {
			if (Input.GetKeyDown (KeyCode.E)) {
				if (isViewingTarget == false) {
					cameraScript.focusTarget ();
					canvasScript.showDetailText ();
					if (OnStartViewTarget != null) {
						OnStartViewTarget ();
					}
				} else {
					cameraScript.unfocusTarget ();
					canvasScript.hideDetailText ();
					if (OnEndViewTarget != null) {
						OnEndViewTarget ();
					}
				}
				isViewingTarget = !isViewingTarget;
			}

			if (isViewingTarget) {
				cameraScript.lerpTarget (anchor.transform.position, anchor.transform.rotation, 3.2f);

				if (OnUpdateViewingTarget != null) {
					OnUpdateViewingTarget ();
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
