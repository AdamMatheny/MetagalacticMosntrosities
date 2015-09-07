using UnityEngine;
using System.Collections;

public class InvincibleBox : MonoBehaviour 
{
	ScoreManager mScoreManager;
	PlayerShipController mPlayerAvatar;

	bool mPlayerInside = false;
	// Use this for initialization
	void Start () 
	{
		mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		mPlayerAvatar = FindObjectOfType<PlayerShipController>() as PlayerShipController;
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Make sure we always have a ScoreManager ~Adam
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		}
		//Make sure we always have a Ship ~Adam
		if(mPlayerAvatar == null)
		{
			mPlayerAvatar = FindObjectOfType<PlayerShipController>() as PlayerShipController;
		}
	}//END of Update()

	void LateUpdate()
	{
		if(mPlayerInside)
		{
			mPlayerAvatar.GetComponent<PlayerShipController>().mMainShip.GetComponent<Renderer>().material.color = Color.white;
			mPlayerAvatar.GetComponent<PlayerShipController>().mSecondShip.GetComponent<Renderer>().material.color = Color.white;
		}
	}//END of LateUpdate()

	void OnTriggerStay(Collider other)
	{
		if(other.GetComponent<PlayerShipController>() != null)
		{
			mScoreManager.mPlayerSafeTime =0.2f;
			mPlayerInside = true;
		}
	}//END of OnTriggerStay()
	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<PlayerShipController>() != null)
		{
			mPlayerInside = false;
		}
	}//END of OnTriggerExit()
}
