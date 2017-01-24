using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCanvasScript : MonoBehaviour {

	public GameObject waypointText;
	public GameObject detailText;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void showDetailText () {
		detailText.SetActive (true);
	}

	public void hideDetailText () {
		detailText.SetActive (false);
	}


}
