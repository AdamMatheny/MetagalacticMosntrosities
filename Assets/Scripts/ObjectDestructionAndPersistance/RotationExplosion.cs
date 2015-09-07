using UnityEngine;
using System.Collections;

public class RotationExplosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		transform.rotation = Quaternion.AngleAxis(90, Vector3.left);
	}
}
