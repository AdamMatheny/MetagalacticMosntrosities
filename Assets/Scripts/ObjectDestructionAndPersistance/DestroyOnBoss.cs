using UnityEngine;
using System.Collections;

public class DestroyOnBoss : MonoBehaviour {

	void Update () {
	
		if (Application.loadedLevel == 26) {

			Destroy(gameObject);
		}
	}
}
