using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAnimation : MonoBehaviour {

	public Animator anim;

	// Use this for initialization
	void Start () {
		
	}

	void ShakeFinished(){
		Debug.Log ("Shake Finished");
		anim.SetTrigger ("idle");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
