using UnityEngine;
using System.Collections;

public class LDBossHorn : BossWeakPoint 
{

	public int health;

	public enum HornList
	{

		LeftHorn,
		RightHorn,
		FinalBoss
	}

	public HornList hornNumber;

	//For Flashing when hit ~Adam
	public SpriteRenderer mHornSprite;

	public GameObject mDeathEffect;
	public Transform mExplosionPoint;


	public GameObject mHornBeam;

	public override void Start()
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

		if(mHornBeam != null)
		{
			if(mBossCentral.mOverheated)
			{
				mHornBeam.SetActive (false);
			}
			else
			{
				mHornBeam.SetActive (true);
			}
		}
	}


	public override void TakeDamage()
	{

		health --;

		//For flashing when hit ~Adam
		if(mHornSprite != null)
		{
			mHornSprite.color = Color.Lerp (mHornSprite.color, Color.red,1f);
		}

		base.TakeDamage ();

		if (health <= 0) {

			BlowUpHorn();
		}


	}

	public void BlowUpHorn(){

		if (hornNumber != HornList.FinalBoss) {

			if (hornNumber == HornList.LeftHorn) {
				
				GetComponentInParent<Boss1> ().leftHornAlive = false;
			} else {
				
				GetComponentInParent<Boss1> ().rightHornAlive = false;
			}
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
