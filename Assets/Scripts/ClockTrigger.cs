using UnityEngine;

public class ClockTrigger : MonoBehaviour {

	public GameObject tube;
	public GameObject hand;
	public GameObject clockModel;
	public Animator anim;

	private int shakeHash = Animator.StringToHash("shake");

	private const int MinHour = 1;
	private const int MaxHour = 12;
	private const int CorrectHour = 6;

	private bool collectedTube = false;
	private Vector3 tubePos;
	private InspectionTrigger inspectionTrigger;
	private int currentHour = 12;

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

	float RotationForClockHour(int hour) {
		return ((360 / 12) * hour);
	}

	void MoveHand(int hour) {
		Vector3 rot = new Vector3 (hand.transform.rotation.x, hand.transform.rotation.y, RotationForClockHour(hour));
		Quaternion handRot = Quaternion.Euler (rot);
		hand.transform.rotation = Quaternion.Lerp (hand.transform.rotation, handRot, Time.deltaTime * 4.0f);
	}

	// Inspection Trigger

	void OnStartViewTarget () {

	}

	void OnEndViewTarget () {

	}

	void OnUpdateViewingTarget () {

		// Update hour
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			currentHour--;
			if (currentHour < MinHour) {
				currentHour = MaxHour;
			} 
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			currentHour++;
			if (currentHour > MaxHour) {
				currentHour = MinHour;
			}
		}
			
		// Move hand position
		MoveHand(currentHour);

		// Try and get the key
		if (Input.GetKeyDown (KeyCode.Return)) {
			// Shake
			anim.SetTrigger(shakeHash);

			if (currentHour == CorrectHour) {
				
			}

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
