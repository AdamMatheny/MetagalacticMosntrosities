using UnityEngine;
using System.Collections;

public class PermanentGameController : MonoBehaviour {

	//I'm using this script to set things that I want to be permanent throughout the game. Feel free to move elsewhere. ~ Jonathan

	// Use this for initialization
	void Start(){

		Screen.lockCursor = true; //Can't move cursor
	}

	void Awake () {
	
		DontDestroyOnLoad(transform.gameObject); //Don't Destroy
		Application.targetFrameRate = 60; //Attempt to set the Framerate
	}
}
