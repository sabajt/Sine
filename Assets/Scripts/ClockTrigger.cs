using UnityEngine;
using System.Collections;

public class ClockTrigger : MonoBehaviour {

	public GameObject tube;
	public GameObject hand;
	public GameObject clockModel;
	public Animator anim;
	public InspectionTrigger inspectionTrigger;

	private int shakeHash = Animator.StringToHash("shake");

	private const int MinHour = 1;
	private const int MaxHour = 12;
	private const int CorrectHour = 6;

	private bool collectedTube = false;
	private Vector3 tubePos;
	private int currentHour = 12;

	void Start () {
		Vector3 pos = tube.transform.position;
		tubePos = new Vector3 (pos.x, pos.y, pos.z - 0.2f);
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

	float RotationForClockHour(int hour) {
		return ((360 / 12) * hour);
	}

	void LerpHand(int hour) {
		Vector3 rot = new Vector3 (hand.transform.rotation.x, hand.transform.rotation.y, RotationForClockHour(hour));
		Quaternion handRot = Quaternion.Euler (rot);
		hand.transform.rotation = Quaternion.Lerp (hand.transform.rotation, handRot, Time.deltaTime * 4.0f);
	}

	void StepHand (bool forward) {
		int step = forward ? 1 : -1;
		currentHour += step;

		if (currentHour < MinHour) {
			currentHour = MaxHour;
		} else if (currentHour > MaxHour) {
			currentHour = MinHour;
		}

		if (currentHour == CorrectHour) {
			StartCoroutine(ShakeAfterDelay(0.33f));
		}
	}

	// Inspection Trigger
	void OnStartViewTarget () {

	}

	void OnEndViewTarget () {

	}
		
	void OnUpdateViewingTarget () {
		// Update hour
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			StepHand (false);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			StepHand (true);
		}
			
		// Move hand position
		LerpHand(currentHour);

//			if (collectedTube) {
//				inspectionTrigger.EndViewingTarget ();
//			}
//			collectedTube = true;
//
//			
//		if (collectedTube) {
//			tube.transform.position = Vector3.Lerp (tube.transform.position, tubePos, Time.deltaTime * 3.0f);
//		}
	}

	IEnumerator ShakeAfterDelay(float time) {
		yield return new WaitForSeconds(time);
		anim.SetTrigger(shakeHash);
	}
}
