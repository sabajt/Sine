using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCanvasScript : MonoBehaviour {

	public GameObject waypointText;
	public GameObject detailText;

	public void showDetailText () {
		detailText.SetActive (true);
	}

	public void hideDetailText () {
		detailText.SetActive (false);
	}
}
