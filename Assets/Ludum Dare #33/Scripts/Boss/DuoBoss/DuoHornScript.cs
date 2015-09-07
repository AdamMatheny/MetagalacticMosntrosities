using UnityEngine;
using System.Collections;
using InControl;
using XInputDotNetPure;

public class DuoHornScript : BossWeakPoint {
	
	public int health;
	
	public enum HornList{
		//Left horn is blue head and right horn is tail
		LeftHorn,
		RightHorn
	}
	
	public HornList hornNumber;
	
	//For Flashing when hit ~Adam
	public SpriteRenderer mHornSprite;
	
	public GameObject mDeathEffect;
	public Transform mExplosionPoint;


	//For turning off attacks when overheated
	public GameObject mBeam;


	//For putting a charge attack on the tail (copy pasted from the BossEye script for the skull boss)
	public float timer;
	float timerTemp;
	bool mShooting = false;
	public GameObject BuildUp;
	public GameObject mTarget;
	public GameObject bullet;


	public override void Start ()
	{
		mBossCentral.mTotalHealth += health;
		mBossCentral.mCurrentHealth += health;
	}

	public override void Update()
	{
		//For flashing when hit ~Adam
		if(mHornSprite != null)
		{
			mHornSprite.color = Color.Lerp (mHornSprite.color, Color.white,0.1f);
		}

		if(mBossCentral.mOverheated && mBeam != null)
		{
			mBeam.SetActive (false);
		}
		else if(mBeam != null)
		{
			mBeam.SetActive (true);
		}

		//For doing a Charge attack from the tail
		if(hornNumber == HornList.RightHorn)
		{
			if(mTarget == null)
			{
				mTarget = GameObject.FindGameObjectWithTag ("Player");
			}
			//Fire the eye beam on button press ~Adam
			if((Input.GetButtonDown ("FireGun") || InputManager.ActiveDevice.Action1.WasPressed)&& mBossCentral.mChargeReady)
			{
				mShooting = true;
				timerTemp = 1.1f;
				mBossCentral.mChargeReady = false;
				mBossCentral.mCurrentCharge = 0;
				mBossCentral.mOverheated = true;
			}
			
			if (timerTemp < 1) 
			{
				
				BuildUp.SetActive (true);
			} 
			else 
			{
				
				BuildUp.SetActive(false);
			}
			
			if (mShooting && timerTemp > 0) 
			{
				
				timerTemp -= Time.deltaTime;
			} 
			else if(mShooting)
			{
				
				timerTemp = timer;
				mShooting = false;
				Instantiate(bullet, transform.position, Quaternion.identity);
				Debug.Log("SHOOT!");
			}
		}

	}
	
	
	public override void TakeDamage(){
		
		health --;
		base.TakeDamage ();

		//For flashing when hit ~Adam
		if(mHornSprite != null)
		{
			mHornSprite.color = Color.Lerp (mHornSprite.color, Color.red,1f);
		}
		
		if (health <= 0) {
			
			BlowUpHorn();
		}
	}
	
	public void BlowUpHorn(){
		
		if (hornNumber == HornList.LeftHorn) {
			
			GetComponentInParent<DuoBossReak> ().leftHornAlive = false;
		} else {
			
			GetComponentInParent<DuoBossReak> ().rightHornAlive = false;
		}
		if(mDeathEffect != null)
		{
			if(mExplosionPoint !=null)
			{
				Instantiate(mDeathEffect, mExplosionPoint.position, Quaternion.identity);
			}
			else
			{
				Instantiate(mDeathEffect, transform.position, Quaternion.identity);
			}
		}
		Destroy (gameObject);
	}
}
