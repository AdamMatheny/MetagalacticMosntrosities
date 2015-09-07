using UnityEngine;
using System.Collections;

public class CoOpShooting : MonoBehaviour {

	public GameObject mPlayerTwo;

	void Start()
	{
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			mPlayerTwo = FindObjectOfType<PlayerTwoShipController>().gameObject;
			GetComponent<EnemyShipAI> ().mShooter = true;
			GetComponent<EnemyShipAI> ().mAutoShoot = true;
		} 
		else 
		{
			
			GetComponent<EnemyShipAI> ().mShooter = false;
			GetComponent<EnemyShipAI> ().mAutoShoot = false;
		}
	}

//	void Update()
//	{
//
//		if (mPlayerTwo != null) 
//		{ //If in CoOp mode
//			
//			GetComponent<EnemyShipAI> ().mShooter = true;
//			GetComponent<EnemyShipAI> ().mAutoShoot = true;
//		} else 
//		{
//
//			GetComponent<EnemyShipAI> ().mShooter = false;
//			GetComponent<EnemyShipAI> ().mAutoShoot = false;
//		}
//	}
}
