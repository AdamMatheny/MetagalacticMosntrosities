using UnityEngine;
using System.Collections;

public class LDPussControls : MonoBehaviour {

	public int speed;
	
	void Update () {
	
		float horizontal = Input.GetAxis ("RightAnalogHorizontal");
		float vertical = Input.GetAxis ("RightAnalogVertical");
		
		GetComponent<Rigidbody> ().velocity = new Vector3 (horizontal * speed, vertical * speed);
	}
}
