using UnityEngine;
using System.Collections;

public class ShieldEmblem : MonoBehaviour 
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

			other.GetComponentInParent<PlayerShipController> ().mShielded = true;
			other.GetComponentInParent<PlayerShipController> ().mShieldTimer = 30f;
			Destroy(this.gameObject);
		}

		if(other.GetComponent<PlayerShipController>() != null)
		{
			other.GetComponent<PlayerShipController>().mShielded = true;
			other.GetComponent<PlayerShipController>().mShieldTimer = 30f; 
			Destroy(this.gameObject);
		}
		if(other.GetComponent<PlayerTwoShipController>() != null)
		{
			FindObjectOfType<PlayerTwoShipController>().mShielded = true;
			FindObjectOfType<PlayerTwoShipController>().mShieldTimer = 30f;
			Destroy(this.gameObject);
		}
	}
}