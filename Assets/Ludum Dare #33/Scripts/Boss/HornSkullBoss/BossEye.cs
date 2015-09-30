using UnityEngine;
using System.Collections;
using InControl;
using XInputDotNetPure;

public class BossEye : BossWeakPoint 
{

	public BossGenericScript mBossBody;

	public GameObject BuildUp;
	
	public GameObject mTarget;
	
	public GameObject bullet;
	public GameObject bulletSpread;

	public bool mTripleShot;

	public int health;
	
	public float timer;
	float timerTemp;

	public SpriteRenderer mMainBodySprite;

	bool mShooting = false;
	[SerializeField] private bool mBoss1 = false;

	public override void Start()
	{
		mTarget = GameObject.FindGameObjectWithTag ("Player");

		timerTemp = timer;	

		mBossCentral.mTotalHealth += health;
		mBossCentral.mCurrentHealth += health;
	}
	
	public override void Update()
	{
		if(mBoss1)
		{
			float bossHealth = mBossCentral.GetComponent<Boss1> ().mCurrentHealth;
		
			float threshHold = mBossCentral.GetComponent<LDBossDeathWeapon> ().mHealthThreshHold;
			
			if (bossHealth <= threshHold) {
				
				mTripleShot = true;
			} else {
				
				mTripleShot = false;
			}
		}


		//For flashing when hit ~Adam
		if(mMainBodySprite != null)
		{
			mMainBodySprite.color = Color.Lerp (mMainBodySprite.color, Color.white,0.1f);
		}

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
			mBossBody.mOverheated = true;
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
			if(bullet != null)
			{
				Instantiate(bullet, transform.position + new Vector3(0, 4), Quaternion.identity);

				if(mTripleShot){

					GameObject bulletOne;
					GameObject bulletTwo;
					bulletOne = Instantiate(bulletSpread, transform.position, Quaternion.identity) as GameObject;
					bulletOne.GetComponent<EnemyBulletController>().mFireDir = Quaternion.Euler(0f,0f,30f) * Vector3.Normalize(mBossCentral.mHero.transform.position - transform.position);
					bulletOne.transform.LookAt (bulletOne.transform.position + bulletOne.GetComponent<EnemyBulletController>().mFireDir);
					bulletOne.transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + bulletOne.transform.rotation.eulerAngles);
					
					
					bulletTwo = Instantiate(bulletSpread, transform.position, Quaternion.identity) as GameObject;
					bulletTwo.GetComponent<EnemyBulletController>().mFireDir = Quaternion.Euler(0f,0f,-30f) * Vector3.Normalize(mBossCentral.mHero.transform.position - transform.position);
					bulletTwo.transform.LookAt (bulletTwo.transform.position + bulletTwo.GetComponent<EnemyBulletController>().mFireDir);
					bulletTwo.transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + bulletTwo.transform.rotation.eulerAngles);
				}
			}
			Debug.Log("SHOOT!");
		}
		
		//float horizontal = Input.GetAxis ("RightAnalogHorizontal");
		//float vertical = Input.GetAxis ("RightAnalogVertical");
		
		//transform.localPosition = new Vector2 (horizontal / 15, (vertical / 15) + .04f);

		if(Mathf.Abs(mTarget.transform.position.x - transform.position.x) > 1f)
		{
			if(mTarget.transform.position.x > transform.position.x)
			{
				transform.localPosition = (new Vector3(.05f,transform.localPosition.y));
			}
			else
			{
				transform.localPosition = (new Vector3(-.05f,transform.localPosition.y));
			}
			
		}
		else
		{
			transform.localPosition = (new Vector3(0f,transform.localPosition.y - .02f));
		}
		//Eye Y position
		if(Mathf.Abs(mTarget.transform.position.y - transform.position.y) > .1f)
		{
			if(mTarget.transform.position.y > transform.position.y)
			{
				transform.localPosition = (new Vector3(transform.localPosition.x, .04f,-0.02f));
			}
			else
			{
				transform.localPosition = (new Vector3(transform.localPosition.x, -.04f,-0.02f));
			}
		}
		else
		{
			transform.localPosition = (new Vector3(transform.localPosition.x, .02f,-0.02f));
		}
	}

	public override void TakeDamage()
	{
		if(!mInvincible)
		{
			if (GetComponentInParent<Boss1> ().leftHornAlive == false) 
			{
				if (GetComponentInParent<Boss1> ().rightHornAlive == false) 
				{
					health --;
					base.TakeDamage ();
					//For flashing when hit ~Adam
					if(mMainBodySprite != null)
					{
						mMainBodySprite.color = Color.Lerp (mMainBodySprite.color, Color.red, 1f);
					}
				}
			}

			if (health <= 0) 
			{
				BlowUpEye();
			}
		}
	}

	public void BlowUpEye()
	{
		mBossBody.mDying = true;
		Destroy (gameObject);

	}
}
