using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {

	public int duration = 30;

	float speed;
	float scaleSize;
	float zedPos;

	public Rigidbody2D rb;

	// Use this for initialization
	void Start () 
	{
		//This line was to create an  Asteroid counter in the player prefs
//		PlayerPrefs.SetInt("AsteroidCount", 0);
//		PlayerPrefs.SetInt("AsteroidRequiredCount", 100);
//		PlayerPrefs.SetInt("SpawnLaserFist",0);

		speed = Random.Range (-1, -10);

		zedPos = Random.Range (-3.3f, 1.68f);
		transform.position = new Vector3 (transform.position.x, transform.position.y, zedPos);

		//From when we were experimenting with being able to hide under asteroids -Adam
//		if(transform.position.z > -2.63f)
//		{
//			transform.GetComponent<Renderer>().material.color = Color.gray;
//		}


		scaleSize = Random.Range (4f, 8f);
		transform.localScale = new Vector3 (scaleSize, scaleSize, 1f);
	
		rb = GetComponent<Rigidbody2D>();

		rb.velocity = new Vector2 (rb.velocity.x, speed);
		rb.angularVelocity = Random.Range (-200, 150);
		Destroy (gameObject, duration);
	}
}