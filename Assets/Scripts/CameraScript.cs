using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public Camera eventCamera;
	public AudioListener eventCameraAudioListener;

	private GameObject fpsCharacter;
	private GameObject fpsController;
	private Camera fpsCamera;

	// Use this for initialization
	void Start () {
		fpsCharacter = GameObject.Find ("FirstPersonCharacter");
		fpsController = GameObject.Find ("FPSController");
		fpsCamera = fpsCharacter.GetComponent<Camera> ();

		eventCamera.enabled = false;
		eventCameraAudioListener.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FocusTarget() {
		transform.position = fpsCharacter.transform.position;
		transform.rotation = fpsCharacter.transform.rotation;

		eventCamera.enabled = true;
		eventCameraAudioListener.enabled = true;

		fpsController.SetActive (false);
		fpsCharacter.SetActive (false);
		fpsCharacter.GetComponent<Camera> ().enabled = false;
	}

	public void UnfocusTarget() {
		fpsCamera.enabled = true;
		fpsController.SetActive (true);
		fpsCharacter.SetActive (true);

		eventCamera.enabled = false;
		eventCameraAudioListener.enabled = false;
	}

	public void LerpTarget(Vector3 toPosition, Quaternion toRotation, float speed) {
		transform.position = Vector3.Lerp (transform.position, toPosition, Time.deltaTime * speed);
		transform.rotation = Quaternion.Lerp (transform.rotation, toRotation, Time.deltaTime * speed);
	}
}
