using UnityEngine;
using System.Collections;

public class SecondaryShake : MonoBehaviour {


	public bool Purple; //Boss weak point explosion and Hero Ship death ~Adam
	public bool Red; //Boss Death effect ~Adam
	public bool Green; //Hero Ship getting hit ~Adam
	public bool Teal;  //Hornet boss minion death ~Adam

	void Start(){

		if (Purple) {

			Camera.main.GetComponent<CameraShaker>().ShakeCameraPurple();
		}
		if (Teal) {
			
			Camera.main.GetComponent<CameraShaker>().ShakeCameraTeal();
		}
		if (Red) {
			
			Camera.main.GetComponent<CameraShaker>().ShakeCameraRed();
		}
		if (Green) {
			
			Camera.main.GetComponent<CameraShaker>().ShakeCameraGreen();
		}
	}

	void Awake(){

		if (Purple) {
			
			Camera.main.GetComponent<CameraShaker>().ShakeCameraPurple();
		}
		if (Teal) {
			
			Camera.main.GetComponent<CameraShaker>().ShakeCameraTeal();
		}
		if (Red) {
			
			Camera.main.GetComponent<CameraShaker>().ShakeCameraRed();
		}
		if (Green) {
			
			Camera.main.GetComponent<CameraShaker>().ShakeCameraGreen();
		}
	}



}
