using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	private GameObject fpsCharacter;
	private GameObject fpsController;
	private GameObject eventCamera;

	// Use this for initialization
	void Start () {
		fpsCharacter = GameObject.Find ("FirstPersonCharacter");
		fpsController = GameObject.Find ("FPSController");
		eventCamera = GameObject.Find ("EventCamera");

		eventCamera.GetComponent<Camera> ().enabled = false;
		eventCamera.GetComponent<AudioListener> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void focusTarget() {
		eventCamera.transform.position = fpsCharacter.transform.position;
		eventCamera.transform.rotation = fpsCharacter.transform.rotation;

		eventCamera.GetComponent<Camera> ().enabled = true;
		eventCamera.GetComponent<AudioListener> ().enabled = true;

		fpsController.SetActive (false);
		fpsCharacter.SetActive (false);
		fpsCharacter.GetComponent<Camera> ().enabled = false;
	}

	public void unfocusTarget() {
		fpsCharacter.GetComponent<Camera> ().enabled = true;
		fpsController.SetActive (true);
		fpsCharacter.SetActive (true);

		eventCamera.GetComponent<Camera> ().enabled = false;
		eventCamera.GetComponent<AudioListener> ().enabled = false;
	}

	public void lerpTarget(Vector3 toPosition, Quaternion toRotation, float speed) {
		eventCamera.transform.position = Vector3.Lerp (eventCamera.transform.position, toPosition, Time.deltaTime * speed);
		eventCamera.transform.rotation = eventCamera.transform.rotation = Quaternion.Lerp (eventCamera.transform.rotation, toRotation, Time.deltaTime * speed);
	}
}
