using UnityEngine;
using System.Collections;
using InControl;
using XInputDotNetPure;

//For spawning "minions" that chase the player as a charge attack

public class BlobEnemySpawner : MonoBehaviour 
{
	public BossGenericScript mBossBody;
	

	public GameObject mTarget;
	
	public GameObject bullet;
	

	public float timer = 0.5f;
	float timerTemp;
	

	bool mShooting = false;
	int mShotsFired = 0;
	public int mShots = 25;


	public void Start()
	{
		
		mTarget = GameObject.FindGameObjectWithTag ("Player");
		
		timerTemp = timer;	
		

	}
	
	public void Update()
	{
		
		if(mTarget == null)
		{
			mTarget = GameObject.FindGameObjectWithTag ("Player");
		}
		//Fire the eye beam on button press ~Adam
		if((Input.GetButtonDown ("FireGun") || InputManager.ActiveDevice.Action1.WasPressed)&& mBossBody.mChargeReady)
		{
			mShooting = true;
			timerTemp = 0.5f;
			mBossBody.mChargeReady = false;
			mBossBody.mCurrentCharge = 0;
			mBossBody.mOverheated = true;
		}
		

		
		if (mShooting && timerTemp > 0) 
		{
			
			timerTemp -= Time.deltaTime;
		} 
		else if(mShooting)
		{
			
			timerTemp = timer;
			mShotsFired ++;
			Instantiate(bullet, transform.position, Quaternion.identity);
			if(mShotsFired >= mShots)
			{
				timerTemp = 10f;
				mShooting = false;
				mShotsFired = 0;
			}
		}
		

	}


}
