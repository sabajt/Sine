using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTrigger : MonoBehaviour {

	// Generic
	public GameObject anchor;
	public GameObject canvas;

	private CameraScript cameraScript;
	private EventCanvasScript canvasScript;
	private bool playerInsideTarget = false;
	private bool isViewingTarget = false;
	private bool hasSelectedBook = false;

	// Event
	public GameObject bookshelf;
	public AudioClip bookSlide;
	public AudioClip bookReveal;
	public AudioClip correctReveal;
	public AudioClip bookshelfSlide;

	private List<GameObject> books = new List<GameObject>();
	private List<Vector3> bookDefPositions = new List<Vector3> ();
	private List<Quaternion> bookDefRotations = new List<Quaternion>();
	private List<Vector3> bookSelPositions = new List<Vector3>();
	private List<Vector3> bookPullPositions = new List<Vector3>();
	private List<Quaternion> bookPullRotations = new List<Quaternion>();

	private int bookIndex = 0;
	const int BookIndexMax = 8;
	const int correctIndex = 3;

	private Vector3 origShelfPos; 
	private Vector3 unlockShelfPos;
	private bool hasUnlocked = false;

	void Start () {
		// Generic
		canvas.SetActive (false);
		canvasScript = canvas.GetComponent<EventCanvasScript> ();
		canvasScript.hideDetailText ();
		cameraScript = GameObject.Find ("CameraController").GetComponent<CameraScript> ();
			
		// Event
		setupBooks ();

		origShelfPos = bookshelf.transform.position;
		unlockShelfPos = new Vector3 (origShelfPos.x, origShelfPos.y, origShelfPos.z + 2);
	}

	void setupBooks () {
		for (int i = 0; i <= BookIndexMax; i++) {
			GameObject book = GameObject.Find ("book" + i);
			books.Add (book);

			// positions
			Vector3 pos = book.transform.position;
			bookDefPositions.Add (pos);
			bookSelPositions.Add (new Vector3(pos.x - 0.2f, pos.y, pos.z));
			bookPullPositions.Add (new Vector3(pos.x - 0.7f, pos.y, pos.z));

			// rotations
			bookDefRotations.Add (book.transform.rotation);
			Vector3 rot = book.transform.rotation.eulerAngles;
			rot = new Vector3 (rot.x, rot.y + 90, rot.z);
			bookPullRotations.Add (Quaternion.Euler (rot));
		}
	}

	void resetBooks () {
		hasSelectedBook = false;
		int i = 0;
		foreach (GameObject book in books) {
			book.transform.position = bookDefPositions [i];
			book.transform.rotation = bookDefRotations [i];
			i++;
		}
	}

	void zoomOut() {
		resetBooks ();
		isViewingTarget = false;
		cameraScript.unfocusTarget ();
		canvasScript.hideDetailText ();
	}

	void moveBookshelf() {
		bookshelf.transform.position = Vector3.Lerp (bookshelf.transform.position, unlockShelfPos, Time.deltaTime * 3.0f);
	}

	void Update () {

		if (hasUnlocked) {
			moveBookshelf ();
		}

		if (playerInsideTarget == true) {

			// Generic 
			if (Input.GetKeyDown (KeyCode.E)) {
				if (isViewingTarget == false) {
					isViewingTarget = true;

					cameraScript.focusTarget();
					canvasScript.showDetailText ();

				} else {
					zoomOut ();
				}
			}

			// Event
			if (isViewingTarget) {
				cameraScript.lerpTarget (anchor.transform.position, anchor.transform.rotation, 3.2f);

				// Book selection
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					bookIndex--;
					if (bookIndex < 0) {
						bookIndex = 0;
					} else {
						resetBooks ();
					}
				}
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					bookIndex++;
					if (bookIndex > BookIndexMax) {
						bookIndex = BookIndexMax;
					} else {
						resetBooks ();
					}
				}

				GameObject book = books[bookIndex];

				// select book
				if (Input.GetKeyDown (KeyCode.Return)) {
					if (!hasSelectedBook) {
						hasSelectedBook = true;
						if (bookIndex == correctIndex) {
							StartCoroutine(UnlockAfterDelay(1.3f));
						}
					}
				}
				if (hasSelectedBook) {
					// pull out / rotate book from shelf
					book.transform.position = Vector3.Lerp (book.transform.position, bookPullPositions[bookIndex], Time.deltaTime * 3.0f);
					book.transform.rotation = book.transform.rotation = Quaternion.Lerp (book.transform.rotation, bookPullRotations[bookIndex], Time.deltaTime * 3.0f);
				}

				// scroll thru / highlight book
				book.transform.position = Vector3.Lerp (book.transform.position, bookSelPositions[bookIndex], Time.deltaTime * 3.6f);
			}
		}
	}

	IEnumerator UnlockAfterDelay(float time) {
		yield return new WaitForSeconds(time);
		zoomOut ();
		hasUnlocked = true;
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
