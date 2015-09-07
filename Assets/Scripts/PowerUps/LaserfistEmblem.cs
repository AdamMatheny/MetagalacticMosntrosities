using UnityEngine;
using System.Collections;

public class LaserfistEmblem : MonoBehaviour 
{
	
	// Use this for initialization
	void Start () 
	{

		Camera.main.GetComponent<CameraShaker> ().RumbleController(.5f, .5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(new Vector3(0f,-5f*Time.deltaTime,0f));
	}
	
	void OnTriggerEnter(Collider other)
	{
		//Auto-fire on mobile ~Adam
		if(Application.isMobilePlatform)
		{
			if(other.GetComponent<PlayerShipController>() != null)
			{
				//other.GetComponent<PlayerShipController>().mHaveLaserFist = true;
				other.GetComponent<PlayerShipController>().mLaserFist.SetActive(true);
				Destroy(this.gameObject);
			}
			if(other.GetComponent<PlayerTwoShipController>() != null)
			{
				//FindObjectOfType<PlayerShipController>().mHaveLaserFist = true;
				other.GetComponent<PlayerTwoShipController>().mLaserFist.SetActive(true);
				Destroy(this.gameObject);
			}
		}
		//Store super weapon on non-mobile ~Aadm
		else
		{
			if(other.GetComponent<PlayerShipController>() != null)
			{
				other.GetComponent<PlayerShipController>().mHaveLaserFist = true;
				Destroy(this.gameObject);
			}
			if(other.GetComponent<PlayerTwoShipController>() != null)
			{
				FindObjectOfType<PlayerTwoShipController>().mHaveLaserFist = true;
				Destroy(this.gameObject);
			}
		}
	}
}