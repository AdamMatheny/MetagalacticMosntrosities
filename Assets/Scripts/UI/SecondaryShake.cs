using UnityEngine;
using System.Collections;

public class SecondaryShake : MonoBehaviour {


	public bool Purple;
	public bool Red;
	public bool Green;
	public bool Teal;

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
