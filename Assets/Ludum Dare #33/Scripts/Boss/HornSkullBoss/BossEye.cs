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

	public int health;
	
	public float timer;
	float timerTemp;

	public SpriteRenderer mMainBodySprite;

	bool mShooting = false;

	public override void Start()
	{
		mTarget = GameObject.FindGameObjectWithTag ("Player");

		timerTemp = timer;	

		mBossCentral.mTotalHealth += health;
		mBossCentral.mCurrentHealth += health;
	}
	
	public override void Update()
	{

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

		if (GetComponentInParent<Boss1> ().leftHornAlive == false) {

			if (GetComponentInParent<Boss1> ().rightHornAlive == false) {
				
				health --;
				base.TakeDamage ();
				//For flashing when hit ~Adam
				if(mMainBodySprite != null)
				{
					mMainBodySprite.color = Color.Lerp (mMainBodySprite.color, Color.red, 1f);
				}
			}
		}

		if (health <= 0) {

			BlowUpEye();
		}

	}

	public void BlowUpEye()
	{
		mBossBody.mDying = true;
		Destroy (gameObject);

	}
}
