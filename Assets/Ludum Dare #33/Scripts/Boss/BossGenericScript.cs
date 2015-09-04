using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BossGenericScript : MonoBehaviour 
{

	//Put generic boss code here
	public List<Transform> mWeakPoints = new List<Transform>();

	public HeroShipAI mHero;

	public float mCurrentHealth = 0f;
	public float mTotalHealth = 0f;

	public float mCurrentOverheat = 0f;
	public float mMaxOverheat = 30f;
	public bool mOverheated = false;

	public float mCurrentCharge = 0f;
	public float mMaxCharge = 10f;
	public bool mChargeReady = false;

	public Rigidbody2D rgb2d;

	//For Dying and spawning the next boss ~Adam
	public bool mDying = false;
	public float mDeathTimer = 5f;
	public GameObject mDeathEffect;
	public GameObject mNextBoss;
	public float mMoveSpeed = 15f;

	public float[] mBounds;
	public float mEntryTime = 3f;

	public virtual void Start()
	{
	
		if(mHero == null)
		{
			mHero = FindObjectOfType<HeroShipAI>();
		}
		rgb2d = GetComponent<Rigidbody2D> ();
	}

	public virtual void Update()
	{
		if(mEntryTime >= 0f)
		{
			mEntryTime -= Time.deltaTime;
		}

		if(mHero == null)
		{
			mHero = FindObjectOfType<HeroShipAI>();
		}
		else
		{
			if(!mHero.mHasEntered)
			{
				mOverheated = true;
				mCurrentOverheat = 10f;
			}
			if(mWeakPoints.Count >0 && mWeakPoints[0] != null)
			{
				mHero.mTarget = mWeakPoints[0];
			}
		}

		if(mWeakPoints.Count >0 && mWeakPoints[0] == null)
		{
			mWeakPoints.Remove(null);
		}

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		rgb2d.velocity = new Vector2 (horizontal * mMoveSpeed, vertical * mMoveSpeed);



		//Keep the boss within the bounds of the screen ~Adam
		if(mBounds.Length >=4)
		{
			//X-Min ~Adam
			if(transform.position.x < mBounds[0])
			{
				transform.position = new Vector3(mBounds[0], transform.position.y, transform.position.z);

			}
			//X-Max ~Adam
			if (transform.position.x > mBounds[1])
			{
				transform.position = new Vector3(mBounds[1], transform.position.y, transform.position.z);

			}
			//Y-Min ~Adam

			if(transform.position.y < mBounds[2])
			{
				transform.position = new Vector3(transform.position.x, mBounds[2], transform.position.z);

			}
			//Y-Max ~Adam
			if (transform.position.y > mBounds[3])
			{
				transform.position = new Vector3(transform.position.x, mBounds[3], transform.position.z);
			}
		}

		//Die and spawn the next boss ~Adam
		if(mDying == true)
		{
			mDeathTimer -= Time.deltaTime;
			if(mDeathEffect != null)
			{
				mDeathEffect.SetActive (true);
			}
			if(mDeathTimer <= 0f)
			{
				if(mNextBoss != null)
				{
					Instantiate (mNextBoss, new Vector3(0f,0f,-2f), Quaternion.identity);
				}
				else
				{
					Application.LoadLevel(0);
				}
				Destroy (this.gameObject);
			}
		}


		//For Overheat Timers ~Adam
		if(mCurrentOverheat <= mMaxOverheat && !mOverheated)
		{
			mCurrentOverheat += Time.deltaTime;
		}
		else if(mCurrentOverheat >= mMaxOverheat)
		{
			mOverheated = true;
		}
		if(mOverheated)
		{
			mCurrentOverheat -= Time.deltaTime*5f;
			if(mCurrentOverheat <= 0f)
			{
				mCurrentOverheat = 0f;
				mOverheated = false;
			}
		}
		//For Charge Timers ~Adam
		if(mCurrentCharge < mMaxCharge && !mChargeReady)
		{
			mCurrentCharge += Time.deltaTime;
		}
		else if(mCurrentCharge >= mMaxCharge)
		{
			mChargeReady = true;
			if(mCurrentCharge > mMaxCharge)
			{
				mCurrentCharge = mMaxCharge;
			}
		}

	}

	public virtual void TakeDamage()
	{

		//health --;
	}
}