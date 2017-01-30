using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTrigger : MonoBehaviour {

	public GameObject tube;

	private bool collectedTube = false;
	private Vector3 tubePos;
	private InspectionTrigger inspectionTrigger;

	void Start () {
		Vector3 pos = tube.transform.position;
		tubePos = new Vector3 (pos.x, pos.y, pos.z - 0.2f);
	}

	void OnEnable () {
		inspectionTrigger = gameObject.GetComponent<InspectionTrigger> ();

		inspectionTrigger.OnStartViewTarget += OnStartViewTarget;
		inspectionTrigger.OnEndViewTarget += OnEndViewTarget;
		inspectionTrigger.OnUpdateViewingTarget += OnUpdateViewingTarget;
	}

	void OnDisable() {
		inspectionTrigger.OnStartViewTarget -= OnStartViewTarget;
		inspectionTrigger.OnEndViewTarget -= OnEndViewTarget;
		inspectionTrigger.OnUpdateViewingTarget -= OnUpdateViewingTarget;
	}

	void OnStartViewTarget () {

	}

	void OnEndViewTarget () {

	}

	void OnUpdateViewingTarget () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (collectedTube) {
				inspectionTrigger.EndViewingTarget ();
			}
			collectedTube = true;
		}

		if (collectedTube) {
			tube.transform.position = Vector3.Lerp (tube.transform.position, tubePos, Time.deltaTime * 3.0f);
		}
	}
}
