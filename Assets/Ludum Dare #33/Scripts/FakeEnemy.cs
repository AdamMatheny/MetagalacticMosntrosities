using UnityEngine;
using System.Collections;

public class FakeEnemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player Bullet")
		{
			Destroy(this.gameObject);
		}
	}
}
