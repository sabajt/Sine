using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioTrigger : MonoBehaviour {

	public InspectionTrigger inspectionTrigger;
	public List<AudioClip> audioClips = new List<AudioClip>();

	const int radioIndexMax = 3;

	private int radioIndex = 0;

	void OnEnable () {
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
		// Radio station selection
		if (Input.GetKeyDown (KeyCode.Return)) {
			radioIndex++;
			if (radioIndex > radioIndexMax) {
				radioIndex = 0;
			}

			AudioSource audio = GetComponent<AudioSource>();
			audio.clip = audioClips [radioIndex];
			audio.Play();
		}
	}
}
