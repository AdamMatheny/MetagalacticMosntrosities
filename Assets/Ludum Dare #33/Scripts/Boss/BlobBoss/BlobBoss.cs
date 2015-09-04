using UnityEngine;
using System.Collections;

public class BlobBoss : BossGenericScript 
{
	
	public int mhealth = 120;
	
	public SpriteRenderer spriter;
	public Animator mAnimator;

	public RuntimeAnimatorController[] mAnimationStages;

	public GameObject mBlobBossBarrage;

	public Transform[] mBarrageSpawnPoints;

	public float mBarrageTimer = 0f;

	//Set starting health and find the body to flash when hit ~Adam
	public override void Start ()
	{
		mTotalHealth = mhealth;
		mCurrentHealth = mhealth;

		spriter = GetComponent<SpriteRenderer> ();
		base.Start ();
	}
	
	public override void Update ()
	{

		//Change number of teeth based on health ~Adam
		//8 teeth
		if(mhealth >= 108)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[0];
		}
		//6 teeth ~Adam
		else if(mhealth >= 72)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[1];
		}
		//4 teeth ~Adam
		else if(mhealth >= 38)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[2];
		}
		//2 teeth ~Adam
		else if(mhealth >= 12)
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[3];
		}
		//No teeth ~Adam
		else
		{
			mAnimator.runtimeAnimatorController = mAnimationStages[4];
		}

		if(!mDying)
		{
			if(!mOverheated)
			{
				mBarrageTimer -= Time.deltaTime;
				if(mBarrageTimer <= 0f)
				{
					BlobBarrage ();
				}
			}
			if(spriter!= null)
			{
				spriter.color = Color.Lerp (spriter.color, Color.white,0.1f);
				
			}
		}
		if(mhealth <= 0f)
		{
			mDying = true;
			//For flashing when hit ~Adam

		}

		base.Update ();
	}

	void BlobBarrage()
	{
		mBarrageTimer = 2.5f;

		if(mBlobBossBarrage != null)
		{
			for(int i = 0; i< mBarrageSpawnPoints.Length; i++)
			{
				GameObject newBullet = Instantiate (mBlobBossBarrage, mBarrageSpawnPoints[i].position, Quaternion.identity) as GameObject;
				newBullet.GetComponent<LDBulletScript>().mPlayer = mHero.gameObject;
			}
		}
	}

	private IEnumerator Hindrance(){
		Debug.Log ("Help");
		
		yield return new WaitForSeconds (1);
		
		Debug.Log ("Help Passed");
		
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShader> ().shader1.enabled = false;
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShader> ().shader2.enabled = false;
	}

}
