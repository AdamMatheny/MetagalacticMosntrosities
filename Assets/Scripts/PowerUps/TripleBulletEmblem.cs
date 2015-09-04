using UnityEngine;
using System.Collections;

public class TripleBulletEmblem : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(new Vector3(0f,-5f*Time.deltaTime,0f));
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "SecondShip") {
			
			other.GetComponentInParent<PlayerShipController> ().mThreeBullet = true;
			other.GetComponentInParent<PlayerShipController> ().mThreeBulletTimer = 30f;
			Destroy(this.gameObject);
		}

		if(other.GetComponent<PlayerShipController>() != null)
		{
			other.GetComponent<PlayerShipController>().mThreeBullet = true;
			other.GetComponent<PlayerShipController>().mThreeBulletTimer = 30f; 
			Destroy(this.gameObject);
		}
		if(other.GetComponent<PlayerTwoShipController>() != null)
		{
			FindObjectOfType<PlayerTwoShipController>().mThreeBullet = true;
			FindObjectOfType<PlayerTwoShipController>().mThreeBulletTimer = 30f;
			Destroy(this.gameObject);
		}
	}
}
