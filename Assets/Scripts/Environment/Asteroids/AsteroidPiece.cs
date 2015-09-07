using UnityEngine;
using System.Collections;

public class AsteroidPiece : MonoBehaviour {

	float timer = 10;
	float scaleSize;

	public Rigidbody2D rb;

	void Start(){

//		scaleSize = Random.Range (6f, 8f);
//		transform.localScale = new Vector3 (scaleSize, scaleSize, 1f);

		timer -= Time.timeScale;

		if (timer <= 0) {

			Destroy(gameObject);
		}

		rb = GetComponent<Rigidbody2D>();

		rb.velocity = new Vector2 (Random.Range(-20,20), Random.Range(-20,20));
		rb.angularVelocity = Random.Range (-200, 200);
	}
}
