using UnityEngine;
using System.Collections;

public class HeroShipAI : MonoBehaviour 
{
	public BossGenericScript mBoss;
	public Transform mTarget;
	public int mHitsRemaining = 10;
	public int mMaxHits = 10;

	public GameObject mHeroBullet;
	public GameObject mDodgeObject;
	Vector3 mDodgePoint;

	public float mSpeed = 16f;
	public Vector3 mMoveDir = Vector3.zero;

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

	// Use this for initialization
	void Start () 
	{
		mMaxHits = mHitsRemaining;
		//Find the Boss ~Adam
		if(mTarget == null || mBoss == null)
		{
			if(FindObjectOfType<BossGenericScript>() != null)
			{
				mBoss = FindObjectOfType<BossGenericScript>();
				mTarget = mBoss.transform;
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
			}
		}

		//Toggle hit effect sparks ~Adam
		if(mInvincibleTimer >= 0f)
		{
			mInvincibleTimer -= Time.deltaTime;
			if(mHitEffect.isStopped)
			{
				mHitEffect.Play();
			}
		}
		else if(mHitEffect.isPlaying)
		{
			mHitEffect.Stop();
		}


		//Try to get under the target point ~Adam
		if(mDodgeTimer <= 0f)
		{
			mMoveDir = Vector3.Normalize ((mTarget.position+(Vector3.down*20f))-transform.position);

		}
		//Dodge away ~Adam
		else
		{
			mMoveDir = Vector3.Normalize (transform.position-mDodgePoint);
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
			mMoveDir*=1.2f;
		}
		mMoveDir = new Vector3(mMoveDir.x, mMoveDir.y, 0f);




		//Don't let the ship shoot or get hit when a new boss or ship is coming in ~Adam
		if(!mHasEntered || mTarget == null || mBoss == null 
		   || (mBoss != null && (mBoss.mEntryTime>0f||mBoss.mDying) ) )
		{
			mInvincibleTimer = 2f;
			mShootTimer = 2f;
			if(transform.position.y > -33f)
			{
				mHasEntered = true;
			}
		}
		//Don't let the ship leave the bounds of the screen ~Adamelse
		{
			//Count down the shoot timer ~Adam
			mShootTimer -= Time.deltaTime;

			//Keep ship within screen bounds
			if(transform.position.x < -20f)
			{
				transform.position = new Vector3(-20f, transform.position.y, transform.position.z);
				mMoveDir*=-1f;
				mDodgeTimer = 0f;
			}
			if (transform.position.x > 20f)
			{
				transform.position = new Vector3(20f, transform.position.y, transform.position.z);
				mMoveDir*=-1f;
				mDodgeTimer = 0f;
			}

			if(transform.position.y < -33f)
			{
				transform.position = new Vector3(transform.position.x, -33f, transform.position.z);
				mMoveDir*=-1f;
				mDodgeTimer = 0f;
			}
			if (transform.position.y > 23f)
			{
				transform.position = new Vector3(transform.position.x, 23, transform.position.z);
				mMoveDir*=-1f;
				mDodgeTimer = 0f;
			}
		}


		//Don't try to go out of bounds due to the boss being too close to the bottom of the screen ~Adam
		
		if(mMoveDir.y < 0f && transform.position.y <-32f)
		{
			mMoveDir = new Vector3(mMoveDir.x,0f,mMoveDir.z);
		}

		//Move the ship ~Adam
		transform.Translate(mMoveDir);

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

	}//END of Update()

	void OnTriggerEnter(Collider other)
	{

		if(other.gameObject != this.gameObject && other.tag != "Player Bullet" && mInvincibleTimer <= 0.5f)
		{
			//Debug.Log ("enter "+other.gameObject.name);
			mDodgeTimer = 0.5f;
			mDodgeObject = other.gameObject;
			mDodgePoint = transform.position+Vector3.Normalize (mDodgeObject.transform.position-transform.position)*0.1f;
			mMoveDir = Vector3.Normalize (transform.position-mDodgePoint);
		}
	}//END of OnTriggerEnter()

	void OnTriggerStay(Collider other)
	{

		//else
		if(other.gameObject != this.gameObject && other.tag != "Player Bullet" && mInvincibleTimer <= 0.5f)
		{
			//Debug.Log ("Stay "+other.gameObject.name);

			mDodgeTimer = 0.5f;
			mDodgeObject = other.gameObject;
			mDodgePoint = transform.position+Vector3.Normalize (mDodgeObject.transform.position-transform.position)*0.1f;
			mMoveDir = Vector3.Normalize (transform.position-mDodgePoint);
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
			mInvincibleTimer = 3f;
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

				Destroy(this.gameObject);
			}
		}
	}//END of HitHeroShip()
}
