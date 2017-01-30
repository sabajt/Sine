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
	public event StartViewTarget OnStartViewTarget;

	public delegate void EndViewTarget();
	public event EndViewTarget OnEndViewTarget;

	public delegate void UpdateViewingTarget();
	public event UpdateViewingTarget OnUpdateViewingTarget;

	void Start () {
		canvas.SetActive (false);
		canvasScript = canvas.GetComponent<EventCanvasScript> ();
		canvasScript.hideDetailText ();
		cameraScript = GameObject.Find ("EventCamera").GetComponent<CameraScript> ();
	}
	
	void Update () {
		if (playerInsideTarget == false) {
			return;
		}
			
		if (Input.GetKeyDown (KeyCode.E)) {
			if (isViewingTarget == false) {
				BeginViewingTarget ();
			} else {
				EndViewingTarget ();	
			}
		}

		if (isViewingTarget) {
			cameraScript.LerpTarget (anchor.transform.position, anchor.transform.rotation, 3.2f);
			if (OnUpdateViewingTarget != null) {
				OnUpdateViewingTarget ();
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

	public void BeginViewingTarget() {
		isViewingTarget = true;
		cameraScript.FocusTarget ();
		canvasScript.showDetailText ();
		if (OnStartViewTarget != null) {
			OnStartViewTarget ();
		}
	}

	public void EndViewingTarget() {
		isViewingTarget = false;
		cameraScript.UnfocusTarget ();
		canvasScript.hideDetailText ();
		if (OnEndViewTarget != null) {
			OnEndViewTarget ();
		}
	}
}
