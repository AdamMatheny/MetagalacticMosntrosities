using UnityEngine;
using System.Collections;

public class HeroShipAI : MonoBehaviour 
{
	public BossGenericScript mBoss;
	public Transform mTarget;
	public Vector3 mTargetPoint;
	public int mHitsRemaining = 10;
	public int mMaxHits = 10;
	
	public GameObject mHeroBullet;
	public GameObject mDodgeObject;
	public Vector3 mDodgePoint;
	public Vector3 mDodgeDir;


	public float mSpeed = 16f;
	public Vector3 mMoveDir = Vector3.zero;
	float mDefaultSpeed;

	public float mShootTimerDefault = 0.1f;
	public float mShootTimer = 2f;
	public Transform mBulletSpawnPoint;
	
	public float mDodgeTimer = 0f;
	
	[SerializeField] private GameObject mShipSprite;
	[SerializeField] private ParticleSystem mThrusters;
	
	public float mInvincibleTimer = 0f;
	[SerializeField] private ParticleSystem mHitEffect;
	
	public GameObject mDeathEffect;
	public GameObject mNextHeroShip;
	
	public bool mHasEntered = false;

	[SerializeField] private bool mGoForCenter = false;
	Vector3 mLastPos = Vector3.zero;
	[SerializeField] private float mStuckTimer = 1f;
	[SerializeField] private float mDodgeStuckTimer = 1f;
	[SerializeField] private float mStuckDefault = 1f;


	[SerializeField] private GameObject mSuperWeapon;
	[SerializeField] private bool mHasFiredSuper = false;
	[SerializeField] private bool mFiringSuper = false;

	float mHoverTimer = 0.5f;

	// Use this for initialization
	void Start () 
	{
		mMaxHits = mHitsRemaining;
		mDefaultSpeed = mSpeed;
		//Find the Boss ~Adam
		if(mTarget == null || mBoss == null)
		{
			if(FindObjectOfType<BossGenericScript>() != null)
			{
				mBoss = FindObjectOfType<BossGenericScript>();
				mTarget = mBoss.transform;
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(Random.Range (-5,5), Random.Range (-2,2),0);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Find the Boss ~Adam
		if(mTarget == null || mBoss == null)
		{
			if(FindObjectOfType<BossGenericScript>() != null)
			{
				mBoss = FindObjectOfType<BossGenericScript>();
				mTarget = mBoss.transform;
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(Random.Range (-3,3), Random.Range (-2,2),0);
			}
		}

		if(mTarget != null && Vector3.Distance (transform.position,mTargetPoint) < 2f)
		{
			//mHoverTimer -= Time.deltaTime;.
			if(Mathf.Abs (transform.position.x - mTarget.position.x) < 2f)
			{
				mSpeed = Mathf.Lerp (mSpeed, 2f, 0.3f);
			}
			else
			{
				mSpeed = Mathf.Lerp (mSpeed, mDefaultSpeed, 0.1f);
			}
			if(Vector3.Distance (transform.position,mTargetPoint) < 0.5f)
			{
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(Random.Range (-3,3), Random.Range (-2,2),0);
				mGoForCenter = false;
				mStuckTimer = mStuckDefault;
			}

		}
		else
		{
			if(mSpeed <= mDefaultSpeed-0.5f)
			{
				if(mTarget != null && Vector3.Distance (transform.position,mTargetPoint) < 3f)
				{
					mSpeed = Mathf.Lerp (mSpeed, mDefaultSpeed/2f, 0.1f);
				}
				else
				{
					mSpeed = mDefaultSpeed;
				}
			}
		}
		if(mDodgeTimer > 0f || mGoForCenter)
		{
			mSpeed = mDefaultSpeed;
		}

		if(mDodgeStuckTimer > mStuckDefault*-1f)
		{
			mDodgeStuckTimer-=Time.deltaTime;
		}

		if(!mHasFiredSuper && mHitsRemaining < 5)
		{
			mHasFiredSuper = true;
			mFiringSuper = true;
			mSuperWeapon.SetActive (true);
		}
		if(mFiringSuper && !mSuperWeapon.activeInHierarchy)
		{
			mFiringSuper = false;
		}

		//Toggle hit effect sparks ~Adam
		if(mInvincibleTimer >= 0f)
		{
			mInvincibleTimer -= Time.deltaTime;
			mDodgeTimer = 0f;
			mDodgeStuckTimer = mStuckDefault;
			mGoForCenter = false;
			mHitEffect.gameObject.SetActive (true);
			if(mHitEffect.isStopped)
			{
				mHitEffect.Play();
			}
		}
		else if(mHitEffect.isPlaying)
		{
			mHitEffect.gameObject.SetActive (false);
			mHitEffect.Stop();
		}

		//Check for if it's been stuck in place ~Adam
		if(mLastPos == transform.position && !mFiringSuper)
		{
			mStuckTimer -= Time.deltaTime;
			if(mStuckTimer <= 0f)
			{
				mGoForCenter = true;
			}
		}
		else
		{
			mLastPos = transform.position;
		}

		//Break out of what it's doing and fly to the center of the boss if stuck ~Adam
		if(mGoForCenter && mTarget != null)
		{
			mTargetPoint = mTarget.transform.position +(Vector3.up*5f);
			mMoveDir = Vector3.Normalize (mTargetPoint-transform.position);

			//mTargetPoint = mTarget.transform.position;
		}

		//Try to get under the target point ~Adam
		if(mDodgeTimer <= 0f)
		{
			mMoveDir = Vector3.Normalize (mTargetPoint-transform.position);

		}
		//Dodge away ~Adam
		else
		{
			//mMoveDir = Vector3.Normalize (transform.position-mDodgePoint);
			mMoveDir = mDodgeDir;
			mDodgeTimer -= Time.deltaTime;
		}

		//Shoot ~Adam
		if(mShootTimer <= 0f)
		{
			FireHeroBullet ();
			mShootTimer = mShootTimerDefault;
		}

		//Adjust for speed and don't move on the Z axis ~Adam
		mMoveDir *= mSpeed * 0.01f;
		if(mDodgeTimer > 0f)
		{
			mMoveDir*=1.5f;
		}
		mMoveDir = new Vector3(mMoveDir.x, mMoveDir.y, 0f);




		//Don't let the ship shoot or get hit when a new boss or ship is coming in ~Adam
		if(!mHasEntered || mTarget == null || mBoss == null 
		   || (mBoss != null && (mBoss.mEntryTime>0f||mBoss.mDying) ) )
		{
			mInvincibleTimer = 1.5f;
			mShootTimer = 2f;
			if(transform.position.y > -33f)
			{
				mHasEntered = true;
			}
		}
		#region Don't let the ship leave the bounds of the screen ~Adamelse
		//Count down the shoot timer ~Adam
		mShootTimer -= Time.deltaTime;

		//Keep ship within screen bounds
		if(transform.position.x <= -20f)
		{
			transform.position = new Vector3(-20f, transform.position.y, transform.position.z);
			mMoveDir*=-1f;
			mDodgeTimer = 0f;
			if(mTarget != null)
			{
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(Random.Range (0,3), 0,0);
				mTargetPoint = new Vector3(-19f, mTargetPoint.y, mTargetPoint.z);
			}
		}
		if (transform.position.x >= 20f)
		{
			transform.position = new Vector3(20f, transform.position.y, transform.position.z);
			mMoveDir*=-1f;
			mDodgeTimer = 0f;
			if(mTarget != null)
			{
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(Random.Range (-3,0), 0,0);
				mTargetPoint = new Vector3(19f, mTargetPoint.y, mTargetPoint.z);
			}
		}

		if(transform.position.y <= -33f)
		{
			transform.position = new Vector3(transform.position.x, -33f, transform.position.z);
			mMoveDir*=-1f;
			mDodgeTimer = 0f;
			if(mTarget != null)
			{
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(0, Random.Range (0,2),0);
				mTargetPoint = new Vector3(mTargetPoint.x, -32f, mTargetPoint.z);
			}
		}
		if (transform.position.y >= 23f)
		{
			transform.position = new Vector3(transform.position.x, 23, transform.position.z);
			mMoveDir*=-1f;
			mDodgeTimer = 0f;
			if(mTarget != null)
			{
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(0, Random.Range (-2,0),0);
				mTargetPoint = new Vector3(mTargetPoint.x, 22f, mTargetPoint.z);
			}
		}
		#endregion

		//Don't try to go out of bounds due to the boss being too close to the bottom of the screen ~Adam
		
		if(mMoveDir.y < 0f && transform.position.y <-32f)
		{
			if(!mGoForCenter)
			{
				mMoveDir = new Vector3(mMoveDir.x,0f,mMoveDir.z);
			}
		}

		//Move the ship ~Adam
		if(!mFiringSuper && mBoss != null && mTarget != null)
		{
			transform.Translate(mMoveDir);
		}

		//Animate the ship ~Adam
		if(mShipSprite.GetComponent<Animator>() != null)
		{
			//Always firing ~Adam
			mShipSprite.GetComponent<Animator>().SetBool ("IsFiring", true);
			//Ship flying left ~Adam
			if(mMoveDir.x <= -0.02f)
			{
				mShipSprite.GetComponent<Animator>().SetInteger ("Direction", -1);
			}
			//Ship flying right ~Adam
			else if(mMoveDir.x >= 0.02f)
			{
				mShipSprite.GetComponent<Animator>().SetInteger ("Direction", 1);
			}
			//Ship flying straight/hovering
			else
			{
				mShipSprite.GetComponent<Animator>().SetInteger ("Direction", 0);
			}
		}

		//Toggle thrusters ~Adam
		if(mThrusters != null)
		{
			if(mMoveDir.y >= -0.02f && mThrusters.isStopped)
			{
				mThrusters.Play ();
			}
			else if(mMoveDir.y < -0.02f)
			{
				mThrusters.Stop();
			}
		}


		//For debug testing hero ship damage
		if(Input.GetKeyDown(KeyCode.K))
		{
			HitHeroShip(1);
		}


		//Keep target point within screen bounds
		if(mTarget != null)
		{
			if(mTargetPoint.x <= -20f)
			{
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(Random.Range (0,5), 0,0);
				mTargetPoint = new Vector3(-19f, mTargetPoint.y, mTargetPoint.z);

			}
			if (mTargetPoint.x >= 20f)
			{
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(Random.Range (-5,0), 0,0);
				mTargetPoint = new Vector3(19f, mTargetPoint.y, mTargetPoint.z);
			}
			
			if(mTargetPoint.y <= -33f)
			{
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(0, Random.Range (0,2),0);
				mTargetPoint = new Vector3(mTargetPoint.x, -32f, mTargetPoint.z);
			}
			if (mTargetPoint.y >= 23f)
			{
				mTargetPoint = mTarget.transform.position +(Vector3.down*20f) +  new Vector3(0, Random.Range (-2,0),0);
				mTargetPoint = new Vector3(mTargetPoint.x, 22f, mTargetPoint.z);
			}
		}
	}//END of Update()

	void OnTriggerEnter(Collider other)
	{

		if(other.gameObject != this.gameObject && other.tag != "Player Bullet")// && mInvincibleTimer <= 1f)
		{
			if(mDodgeStuckTimer > 0 || mDodgeStuckTimer < mStuckDefault*-1)
			{
				if(mDodgeStuckTimer < mStuckDefault*-1)
				{
					mDodgeStuckTimer = mStuckDefault;
				}

				//Debug.Log ("enter "+other.gameObject.name);
				mDodgeObject = other.gameObject;
				//mDodgePoint = transform.position+Vector3.Normalize (mDodgeObject.transform.position-transform.position)*0.1f;
				//mMoveDir = Vector3.Normalize (transform.position-mDodgePoint);
				if(mDodgeTimer < 0f)
				{
					mDodgePoint = transform.position+Vector3.Normalize (mDodgeObject.transform.position-transform.position)*0.1f;
					mDodgeDir = Vector3.Normalize (transform.position-mDodgePoint);
					if(!other.GetComponent<LDBossBeam>() || Random.value <0.5f)
					{
						if(other.transform.position.x<transform.position.x)
						{
							mDodgeDir = Quaternion.Euler (0f,0f,-90f) * mDodgeDir;
						}
						else
						{
							mDodgeDir = Quaternion.Euler (0f,0f,90f) * mDodgeDir;
						}
					}
				}
				if(mInvincibleTimer <= 0f)
				{
					mDodgeTimer = 0.3f;
				}
			}
			else
			{
				mGoForCenter = true;
			}

		}
	}//END of OnTriggerEnter()

	void OnTriggerStay(Collider other)
	{

		//else
		if(other.gameObject != this.gameObject && other.tag != "Player Bullet" && !other.name.Contains("Asteroid"))// && mInvincibleTimer <= 1f)
		{
			//Debug.Log ("Stay "+other.gameObject.name);

			mDodgeObject = other.gameObject;
			//mDodgePoint = transform.position+Vector3.Normalize (mDodgeObject.transform.position-transform.position)*0.1f;
			//mMoveDir = Vector3.Normalize (transform.position-mDodgePoint);
			if(mDodgeTimer < 0.2f)
			{
				mDodgePoint = transform.position+Vector3.Normalize (mDodgeObject.transform.position-transform.position)*0.1f;
				mDodgeDir = Vector3.Normalize (transform.position-mDodgePoint);
				if(!other.GetComponent<LDBossBeam>() || Random.value <0.5f)
				{
					if(other.transform.position.x<transform.position.x)
					{
						mDodgeDir = Quaternion.Euler (0f,0f,-90f) * mDodgeDir;
					}
					else
					{
						mDodgeDir = Quaternion.Euler (0f,0f,90f) * mDodgeDir;
					}
				}
			}
			mDodgeTimer =0.5f;

		}
	}//END of OnTriggerStay()






	void FireHeroBullet()
	{
		Instantiate (mHeroBullet, mBulletSpawnPoint.position, mBulletSpawnPoint.rotation* Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f)));
	}//END of FireHeroBullet()

	public void HitHeroShip(int damage)
	{
		if(mInvincibleTimer <= 0f)
		{
			Camera.main.GetComponent<CameraShaker>().ShakeCameraGreen();
			mGoForCenter = false;
			mInvincibleTimer = 1.5f;
			mHitsRemaining -= damage;
			if(GetComponent<AudioSource>() != null)
			{
				GetComponent<AudioSource>().Play();
			}

			//If this was the last hit, destroy self and spawn next ship
			if(mHitsRemaining <= 0)
			{
				if(mDeathEffect != null)
				{
					Instantiate (mDeathEffect, transform.position, Quaternion.identity);
				}
				if(mNextHeroShip != null)
				{
					Instantiate (mNextHeroShip, new Vector3(0f,-40f, -2f), Quaternion.identity);
				}
				Camera.main.GetComponent<CameraShaker>().ShakeCameraPurple();
				Destroy(this.gameObject);
			}
		}
	}//END of HitHeroShip()
}
