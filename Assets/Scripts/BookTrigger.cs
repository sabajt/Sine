using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTrigger : MonoBehaviour {

	public InspectionTrigger inspectionTrigger;
	public GameObject bookshelf;
	public AudioClip bookSlide;
	public AudioClip bookReveal;
	public AudioClip correctReveal;
	public AudioClip bookshelfSlide;

	const int BookIndexMax = 8;
	const int CorrectIndex = 3;

	private List<GameObject> books = new List<GameObject>();
	private List<Vector3> bookDefPositions = new List<Vector3> ();
	private List<Quaternion> bookDefRotations = new List<Quaternion>();
	private List<Vector3> bookSelPositions = new List<Vector3>();
	private List<Vector3> bookPullPositions = new List<Vector3>();
	private List<Quaternion> bookPullRotations = new List<Quaternion>();
	private bool hasSelectedBook = false;
	private bool hasUnlocked = false;
	private int bookIndex = 0;
	private Vector3 origShelfPos; 
	private Vector3 unlockShelfPos;

	void Start () {
		SetupBooks ();
		origShelfPos = bookshelf.transform.position;
		unlockShelfPos = new Vector3 (origShelfPos.x, origShelfPos.y, origShelfPos.z + 2);
	}

	void Update () {
		if (hasUnlocked) {
			moveBookshelf ();
		}
	}
		
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
		ResetBooks ();
	}

	void OnUpdateViewingTarget () {

		// Book selection
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			bookIndex--;
			if (bookIndex < 0) {
				bookIndex = 0;
			} else {
				ResetBooks ();
			}
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			bookIndex++;
			if (bookIndex > BookIndexMax) {
				bookIndex = BookIndexMax;
			} else {
				ResetBooks ();
			}
		}

		GameObject book = books[bookIndex];

		// select book
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (!hasSelectedBook) {
				hasSelectedBook = true;
				if (bookIndex == CorrectIndex) {
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

	void SetupBooks () {
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

	void ResetBooks () {
		hasSelectedBook = false;
		int i = 0;
		foreach (GameObject book in books) {
			book.transform.position = bookDefPositions [i];
			book.transform.rotation = bookDefRotations [i];
			i++;
		}
	}

	void moveBookshelf() {
		bookshelf.transform.position = Vector3.Lerp (bookshelf.transform.position, unlockShelfPos, Time.deltaTime * 3.0f);
	}

	IEnumerator UnlockAfterDelay(float time) {
		yield return new WaitForSeconds(time);
		ResetBooks ();
		hasUnlocked = true;
		inspectionTrigger.EndViewingTarget ();
	}
}
