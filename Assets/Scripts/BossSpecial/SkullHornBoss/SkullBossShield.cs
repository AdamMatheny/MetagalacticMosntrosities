using UnityEngine;
using System.Collections;

public class SkullBossShield : MonoBehaviour 
{
	public bool mShieldDown = false;
	ScoreManager mScoreManager;
	// Use this for initialization
	void Start () 
	{
		mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;

	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Make sure we always have a ScoreManager ~Adam
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		}

		//GetComponent<Collider>().isTrigger = mShieldDown;
	}//END of Update()

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.GetComponent<PlayerBulletController>() != null)
		{
			other.rigidbody.velocity =new Vector3(Random.Range(-100f,100f),Random.Range(-100f,100f), 0f);
		}

		if(other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			mScoreManager.LoseALife();

		}
	}//END of OnCollsionEnter()

	void OnCollisionStay(Collision other)
	{
		if(other.gameObject.GetComponent<PlayerBulletController>() != null)
		{
			other.rigidbody.velocity =new Vector3(Random.Range(-100f,100f),Random.Range(-100f,100f), 0f);
		}
		
		if(other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			mScoreManager.LoseALife();
			
		}
	}//END of OnCollsionEnter()
//	void OnTriggerEnter(Collider other)
//	{
//		if(other.GetComponent<PlayerShipController>() != null)
//		{
//			mScoreManager.LoseALife();
//		}
//	}//END of OnTriggerEnter()
//	void OnTriggerStay(Collider other)
//	{
//		if(other.GetComponent<PlayerShipController>() != null)
//		{
//			mScoreManager.LoseALife();
//		}
//	}//END of OnTriggerStay)
}
