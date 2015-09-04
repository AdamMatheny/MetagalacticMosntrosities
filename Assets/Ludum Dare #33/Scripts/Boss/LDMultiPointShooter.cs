using UnityEngine;
using System.Collections;
using InControl;
using XInputDotNetPure;

public class LDMultiPointShooter : MonoBehaviour 
{
	public BossGenericScript mBossBody;
	
	
	public GameObject mTarget;
	
	public GameObject bullet;
	
	
	public float timer = 0.5f;
	float timerTemp;
	
	
	bool mShooting = false;

	public Transform[] mBulletSpawnPoints;
	
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
			timerTemp = 1f;
			mBossBody.mChargeReady = false;
			mBossBody.mCurrentCharge = 0;
			mBossBody.mOverheated = true;
		}
		
		
		
		if (mShooting && timerTemp > 0) 
		{
			foreach(Transform spawnPoint in mBulletSpawnPoints)
			{
				if(spawnPoint.GetChild (0)!=null)
				{
					spawnPoint.GetChild (0).gameObject.SetActive (true);
				}
			}
			timerTemp -= Time.deltaTime;
		} 
		else if(mShooting)
		{
			
			timerTemp = timer;
			foreach(Transform spawnPoint in mBulletSpawnPoints)
			{
				Instantiate(bullet, spawnPoint.position, Quaternion.identity);
			}
			timerTemp = 10f;
			mShooting = false;

		}
		else
		{
			foreach(Transform spawnPoint in mBulletSpawnPoints)
			{
				if(spawnPoint.GetChild (0)!=null)
				{
					spawnPoint.GetChild (0).gameObject.SetActive (false);
				}
			}

		}

	}
	
	
}
