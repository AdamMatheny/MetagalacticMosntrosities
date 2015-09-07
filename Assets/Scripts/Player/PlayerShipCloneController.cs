using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using XInputDotNetPure;

public class PlayerShipCloneController : MonoBehaviour 
{
	public PlayerShipController mOriginalShip;
	public GameObject mCloneDeathEffect;

	bool playerIndexSet = false;
	public PlayerIndex playerIndex;

	//For if we ever animate the ship ~Adam
	[SerializeField] private Animator mMainShipAnimator;
	//[SerializeField] private Animator mSecondShipAnimator;
	
	public float bulletShootSpeed = .4f;
	
	//For firing bullets on a set time interval ~Adam
	float mBulletFireTime = 0f;
	//The prefab we're using for the player bullets ~Adam
	public GameObject mBulletPrefab = null;
	
	public float mBaseMovementSpeed = 6f;
	public float mMovementSpeed = 0f;
	
	public Vector3 mMoveDir = new Vector3(0f,-1f,0f);
	
	//private float mTopMovementSpeed = 6f;
	
	//Variables for editing drop speeds ~Adam
	[SerializeField] private float mMaxDropSpeed = 0.4f;
	[SerializeField] private float mDropSpeed = 0;
	[SerializeField] private float mDropAccelRate = 0.05f;
	[SerializeField] private float mDropDeccelRate = 0.01f;
	
	//Used for the duplicating of the ship via Grabber Enemies ~Adam
	public bool mShipStolen = false;
	public bool mShipRecovered = false;
	
	//The game objects that are our ship sprites ~Adam
	public GameObject mMainShip;
	public GameObject mSecondShip;
	//Where our bullets spawn from `Adam
	//indexes:
	//0: Main ship, main bullet
	//1: Second ship, main bullet
	//2: Main ship, left bullet
	//3: Main ship, right bullet
	//4: Second ship, left bullet
	//5: Second ship, right bullet
	[SerializeField] private Transform[] mBulletSpawns;
	
	
	//For Overheating (We can just let the original ship handle overheating and sync to that
//	public bool overHeatProcess = true;
	public bool isOverheated = false;
//	public float heatLevel = 0f;
//	public float mBaseHeatMax = 60f;
//	public float maxHeatLevel;
//	[SerializeField] private Texture2D mOverheatTimerTex;
//	[SerializeField] private Texture2D mOverheatWarningTex;
	
	//For when the player has 3 bullets  ~Adam
	public bool mThreeBullet = true;
	public float mThreeBulletTimer = 0f;
	public GameObject mSideBullet;
	[SerializeField] private Texture2D mBulletTimerTex;
	[SerializeField] private Texture2D mBulletTimerTexVert;
	[SerializeField] private Texture2D mBulletTimerTubeTex;
	
	//For when the ship has a shieldPowerUp ~Adam
	public bool mShielded = false;
	[SerializeField] private SpriteRenderer mMainShipShieldSprite;
	[SerializeField] private SpriteRenderer mSecondShipShieldSprite;
	[SerializeField] private Texture2D mShieldTimerTex;
	public float mShieldTimer = 0f;
	
	[SerializeField] private Texture2D mSideMetersTex;
	
	
	//For deleting duplicate ships when we change levels ~Adam
	public int mShipCreationLevel;
	public bool mToggleFireOn = true;
	
	//For tracking where the ship was last frame so we can see how much/in what direction its moving ~Adam
	public Vector3 mLastFramePosition;
	public Vector3 mLastFrameDifference = Vector3.zero;
	float mLastNonZeroHorizontalDifference;
	bool mDriftDown = true;

	//For spinning the ship around when the player gets hit (while shielded) ~Adam
	float mSpinning = 0f;
	float mSpinTimer = 0f;
	float mSpinTimerDefault = 0.5f;
	
	//For Super Screen-Wiper powerup ~Adam
	public GameObject mLaserFist;
	public GameObject mBigBlast;
	
	
	
	//For making the ship flash when hit
	public GameObject mMainShipHitSprite;
	public GameObject mSecondShipHitSprite;
	
	// Use this for initialization
	void Start () 
	{

		//Adjust speed and scale for mobile ~Adam
		if (Application.isMobilePlatform)
		{
			mBaseMovementSpeed = 20.0f;
			transform.localScale = new Vector3(1.75f,1.75f,1.75f);
		}
		
		mShipCreationLevel = Application.loadedLevel;
		
//		PlayerShipCloneController[] otherPlayerShips = FindObjectsOfType<PlayerShipCloneController>();
//		//Debug.Log(otherPlayerShip.name);
//		foreach(PlayerShipCloneController othership in otherPlayerShips)
//		{
//			if(othership.mShipCreationLevel < this.mShipCreationLevel)
//			{
//				Debug.Log("Found another ship so destroying self.");
//				Destroy(this.gameObject);
//			}
//		}
		
		mLastFramePosition = transform.position;
		
	}//END of Start()
	
	
	//Pesist between level loads/reloads ~Adam
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}//END of Awake()
	
	
	// Update is called once per frame
	void Update () 
	{

		//Synchronize with original ship
		if(mOriginalShip != null)
		{
			mToggleFireOn = mOriginalShip.mToggleFireOn;
			mShielded = mOriginalShip.mShielded;
			mThreeBullet = mOriginalShip.mThreeBullet;
			isOverheated = mOriginalShip.isOverheated;
			mShieldTimer = mOriginalShip.mShieldTimer;
			mMainShip.GetComponent<Renderer>().material.color = mOriginalShip.mMainShip.GetComponent<Renderer>().material.color;
		}


		//maxHeatLevel = mBaseHeatMax +  mBaseHeatMax * Application.loadedLevel/26f;
		//GetComponent<AudioSource>().volume = 0.18f*(30f-Application.loadedLevel)/30f;
		

		//Spin the ships when hit (while shielded ~Adam
		if(mSpinning != 0f)
		{
			mSpinTimer -= Time.deltaTime;
			SpinShip(mSpinning);
			
			if (mSpinTimer <= 0f)
			{
				mSpinning = 0f;
				mMainShip.transform.rotation = Quaternion.identity;
				mSecondShip.transform.rotation = Quaternion.identity;
			}
		}

		
		//Toggle shield sprites ~Adam
		if(mShielded)
		{
			mMainShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 1);
//			mSecondShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 1);
			
			mMainShipShieldSprite.enabled = true;
			mMainShipShieldSprite.GetComponent<Light>().enabled = true;
//			if(mShipRecovered)
//			{
//				mSecondShipShieldSprite.enabled = true;
//				mSecondShipShieldSprite.GetComponent<Light>().enabled = true;
//			}
			//Decrease Shield time ~Adam
			mShieldTimer -= Time.deltaTime;
			if(mShieldTimer <= 0f)
			{
				mShielded = false;
			}
			if(mShieldTimer < 2f)
			{
				mMainShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 0);
//				mSecondShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 0);
				
			}
			if(mShieldTimer < 5f)
			{
				//				mMainShipShieldSprite.GetComponent<Animator>().speed = 0.5f;
				//				mMainShipShieldSprite.GetComponent<Animator>().Play("ShieldSprite_Flicker");
				//				mSecondShipShieldSprite.GetComponent<Animator>().speed = 0.5f;
				//				mSecondShipShieldSprite.GetComponent<Animator>().Play("ShieldSprite_Flicker");
				mMainShipShieldSprite.GetComponent<Renderer>().material.color = Color.Lerp (mMainShipShieldSprite.GetComponent<Renderer>().material.color, Color.red,0.1f);
//				mSecondShipShieldSprite.GetComponent<Renderer>().material.color = Color.Lerp (mSecondShipShieldSprite.GetComponent<Renderer>().material.color, Color.red,0.1f);
			}
			else
			{
				mMainShipShieldSprite.GetComponent<Renderer>().material.color = Color.white;
//				mSecondShipShieldSprite.GetComponent<Renderer>().material.color = Color.white;
				//				mMainShipShieldSprite.GetComponent<Animator>().speed = 0f;
				//				mMainShipShieldSprite.GetComponent<Animator>().Play("ShieldSprite_Solid");
				//				mMainShipShieldSprite.GetComponent<Animator>().StopPlayback();
				//				mSecondShipShieldSprite.GetComponent<Animator>().speed = 0f;
				//				mSecondShipShieldSprite.GetComponent<Animator>().Play("ShieldSprite_Solid");
				//				mSecondShipShieldSprite.GetComponent<Animator>().StopPlayback();
			}
			
		}
		else
		{
			mMainShipShieldSprite.enabled = false;
			mMainShipShieldSprite.GetComponent<Light>().enabled = false;
//			mSecondShipShieldSprite.enabled = false;
//			mSecondShipShieldSprite.GetComponent<Light>().enabled = false;
			
		}
		
		//Increase movement speed as we progress through levels
		if(Time.timeScale > 0f)
		{
			mMovementSpeed = ( mBaseMovementSpeed + (6f/25f*Application.loadedLevel) ) /Time.timeScale;
		}
		else
		{
			mMovementSpeed = ( mBaseMovementSpeed + (6f/25f*Application.loadedLevel) );
		}
		//Make the player drift toward the bottom of the screen
		// transform.position += new Vector3(0f,mDropSpeed*-1f, 0f);
		if(mMoveDir.y < 0f && mDriftDown)
		{
			foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
			{
				shipTrail.enableEmission = false;
			}
			
			
			if(mDropSpeed < mMaxDropSpeed)
			{
				mDropSpeed += mDropAccelRate;
			}
			else
			{
				mDropSpeed = mMaxDropSpeed;
			}
			
		}
		else
		{
			foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
			{
				shipTrail.enableEmission = true;
			}
			
			mDropSpeed -= mDropDeccelRate;
			
			if(mDropSpeed <= 0.01f)
			{
				mDropSpeed = 0.01f;
			}
		}
		
		//Make the player drift faster towards the bottom while firing ~Adam
		if(mToggleFireOn)
		{
			transform.position += new Vector3(0f,-0.00255f*Application.loadedLevel, 0f);
			//Decrease the timer on triple bullets while firing ~Adam
			mThreeBulletTimer -= Time.deltaTime;
		}
		
		//For keyboard controls ~Adam
		float horizontal = Input.GetAxis("HorizontalClone");
		float vertical = Input.GetAxis("VerticalClone");

		//For gamepad controls ~Adam
		if(InputManager.ActiveDevice.RightStickX != 0)
		{
			horizontal = InputManager.ActiveDevice.RightStickX;
		}
		if(InputManager.ActiveDevice.RightStickY != 0)
		{
			vertical = InputManager.ActiveDevice.RightStickY;
		}

		
		
		//Delete the ship if we've returned to the title screen
		if(Application.loadedLevel == 0)
		{
			Destroy(this.gameObject);
		}
		
		
		

		
		
		//For making the ship drift down when not trying to go up
		if(vertical > 0f)
		{
			mDriftDown = false;
		}
		else
		{
			mDriftDown = true;
		}
		

		
		//Taking in diretional Input from the keyboard/Gamepad ~Adam
		if (horizontal != 0.0f || vertical != 0.0f && Time.timeScale != 0f)
		{
			
			
			//Left
			if (horizontal < 0.0f && vertical == 0.0f)
			{
				//transform.Translate(new Vector3((mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
				mMoveDir = Vector3.Lerp(mMoveDir, new Vector3((2f*mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f), 0.08f);
			}
			//Right
			else if (horizontal > 0.0f && vertical == 0.0f)
			{
				//transform.Translate(new Vector3(mMovementSpeed * Time.deltaTime, 0.0f, 0.0f));
				mMoveDir = Vector3.Lerp(mMoveDir,new Vector3(2f*mMovementSpeed * Time.deltaTime, 0.0f, 0.0f), 0.08f);
			}
			//Down
			else if (vertical < 0.0f && horizontal == 0.0f)
			{ 
				//transform.Translate(new Vector3(0.0f, (mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f));
				mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0.0f, (2f*mMovementSpeed * -1.0f) * Time.deltaTime, 0.0f), 0.08f);
			}
			//Up
			else if (vertical > 0.0f && horizontal == 0.0f)
			{
				//transform.Translate(new Vector3(0.0f, mMovementSpeed * Time.deltaTime, 0.0f));
				mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0.0f, 2f*mMovementSpeed * Time.deltaTime, 0.0f), 0.08f);
			}
			//Up+Right
			else if (vertical > 0.0f && horizontal > 0.0f)
			{
				//transform.Translate(Vector3.Normalize(new Vector3(1f,1f,0))*mMovementSpeed * Time.deltaTime );
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(1f,1f,0))*2f*mMovementSpeed * Time.deltaTime , 0.08f);
			}
			//Up+Left
			else if (vertical > 0.0f && horizontal < 0.0f)
			{
				//transform.Translate(Vector3.Normalize(new Vector3(-1f,1f,0))*mMovementSpeed * Time.deltaTime );
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(-1f,1f,0))*2f*mMovementSpeed * Time.deltaTime , 0.08f);
			}
			//Down+Right
			else if (vertical < 0.0f && horizontal > 0.0f)
			{
				//transform.Translate(Vector3.Normalize(new Vector3(1f,-1f,0))*mMovementSpeed * Time.deltaTime );
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(1f,-1f,0))*2f*mMovementSpeed * Time.deltaTime, 0.08f);
			}
			//Down+Left
			else if (vertical < 0.0f && horizontal < 0.0f)
			{
				//transform.Translate(Vector3.Normalize(new Vector3(-1f,-1f,0))*mMovementSpeed * Time.deltaTime );
				mMoveDir = Vector3.Lerp(mMoveDir, Vector3.Normalize(new Vector3(-1f,-1f,0))*2f*mMovementSpeed * Time.deltaTime, 0.08f);
			}
		}
		//END of Keyboard Movement Controls
		



		

		
		//firing bullets
		if (mToggleFireOn) 
		{
			
			if(!isOverheated)
			{
				//Don't worry about modifying heat level, we're just reading of the original ship ~Adam
//				if(heatLevel < maxHeatLevel)
//				{
//					heatLevel += Time.deltaTime;
//				}
//				
//				if(heatLevel >= maxHeatLevel)
//				{
//					
//					heatLevel = maxHeatLevel;
//					isOverheated = true;
//				}
				
				//Firing Bullets
				if (Time.time > mBulletFireTime) 
				{
					
					// Make the bullet object
					GameObject newBullet = Instantiate (mBulletPrefab, mBulletSpawns[0].position, mMainShip.transform.rotation * Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f))) as GameObject;
					
					if (mThreeBullet) 
					{
						
						if(!mShipRecovered)
						{
							Instantiate (mSideBullet, mBulletSpawns[2].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, 10f) * Quaternion.Euler (0f,0f,Random.Range(-5.0f,5.0f)));
						}
						else
						{
							Instantiate (mSideBullet, mBulletSpawns[2].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, 5f) * Quaternion.Euler (0f,0f,Random.Range(-10.0f,3.0f)));
						}
						Instantiate (mSideBullet, mBulletSpawns[3].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, -10f) * Quaternion.Euler (0f,0f,Random.Range(-5.0f,5.0f)));
						
					}
					//GetComponent<AudioSource> ().Play ();
					//We're not making a side ship ~Adam
//					if (mShipRecovered) 
//					{
//						GameObject secondBullet;
//						secondBullet = Instantiate (mBulletPrefab, mBulletSpawns[1].position, mSecondShip.transform.rotation * Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f))) as GameObject;
//						secondBullet.name = "SECONDBULLET";
//						if (mThreeBullet) 
//						{
//							Instantiate (mSideBullet, mBulletSpawns[4].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, 10f) * Quaternion.Euler (0f,0f,Random.Range(-5.0f,5.0f)));
//							
//							Instantiate (mSideBullet, mBulletSpawns[5].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, -5f) * Quaternion.Euler (0f,0f,Random.Range(-3.0f,10.0f)));
//						}
//					}
					//Reset the timer to fire bullets.  The later the level, the smaller the time between shots

					if(mSpinning == 0)
					{
						if(Application.loadedLevelName != "Credits")
						{
							mBulletFireTime = Time.time + bulletShootSpeed - (0.25f / 25f * Application.loadedLevel);
						}
						else
						{
							mBulletFireTime = Time.time + (bulletShootSpeed - (0.25f / 25f * 21f));
						}
					}
					else
					{
						mBulletFireTime = Time.time + (bulletShootSpeed - (0.25f / 25f * Application.loadedLevel))/3f;
					}
				}
			}
		}
		//Not worrying about heat levels on the clone ~Adam
//		else 
//		{
//			
//			if(heatLevel > 0)
//			{
//				
//				if(isOverheated)
//				{
//					heatLevel -= Time.deltaTime * maxHeatLevel/5f;
//				}
//				else
//				{
//					heatLevel -= Time.deltaTime * 3f;
//				}
//			}
//		}
//		
//		if (heatLevel <= 0f) 
//		{
//			
//			isOverheated = false;
//			if (Application.isMobilePlatform) //Start shooting when weapons are Cool. Lol, weapons are always cool.
//			{
//				mToggleFireOn = true;
//			}
//		}
		
		if(InputManager.ActiveDevice.Action2.IsPressed || Input.GetButton("Thrusters"))
		{
			if(InputManager.ActiveDevice.Action2.IsPressed)
			{
				Debug.Log("InControl recognized Action2");
			}
			mDropSpeed -= mDropDeccelRate*3f;
			if(mDropSpeed <= 0.01f)
			{
				mDropSpeed = 0.00f;
			}
			
			if(mMoveDir.y < -0.2f)
			{
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
				{
					shipTrail.enableEmission = false;
				}
			}
			else
			{
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
				{
					shipTrail.enableEmission = true;
				}
			}
		}
		
		//Move the ship by the mMoveDir vector if not paused
		if(Time.timeScale != 0f)
		{
			if (Application.isMobilePlatform)
			{
				if(mMoveDir.y < 0f && !(vertical == 0.0f && !Input.GetMouseButton(0)))
				{
					mMoveDir = Vector3.Lerp(mMoveDir, mMoveDir+ new Vector3(0f,-mDropSpeed,0f), 0.08f);
				}
				transform.Translate(mMoveDir);
				
				if (vertical == 0.0f && !Input.GetMouseButton(0))
				{
					mDriftDown = true;
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(mMoveDir.x,-mDropSpeed,mMoveDir.z), 0.2f);
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0f,-mDropSpeed,0f), 0.03f);
					
				}
			}
			else
			{
				if(mMoveDir.y < 0f && !(vertical == 0.0f))
				{
					mMoveDir = Vector3.Lerp(mMoveDir, mMoveDir+ new Vector3(0f,-mDropSpeed,0f), 0.08f);
				}
				transform.Translate(mMoveDir);
				
				if (vertical == 0.0f)
				{
					mDriftDown = true;
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(mMoveDir.x,-mDropSpeed,mMoveDir.z), 0.2f);
					mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(0f,-mDropSpeed,0f), 0.03f);
					
				}
			}
		}
		mMainShipAnimator.speed = Application.loadedLevel/5f+1f;
		//mSecondShipAnimator.speed = Application.loadedLevel/5f+1f;
		if(mToggleFireOn)
		{
			mMainShipAnimator.SetBool("IsFiring", true);
			//mSecondShipAnimator.SetBool("IsFiring", true);
		}
		else
		{
			mMainShipAnimator.SetBool("IsFiring", false);
			//mSecondShipAnimator.SetBool("IsFiring", false);
		}
		
		//We're not extra-duplicating this ship
		//Control whether or not to render the second ship 
//		if (mShipRecovered)
//		{
//			mSecondShip.GetComponent<SpriteRenderer>().enabled = true;
//			foreach (ParticleSystem shipTrail in mSecondShip.GetComponentsInChildren<ParticleSystem>())
//			{
//				if(!(mMoveDir.y < 0f && mDriftDown))
//				{
//					shipTrail.enableEmission = true;
//				}
//			}
//		}
//		else
//		{
//			mSecondShip.GetComponent<SpriteRenderer>().enabled = false;
//			foreach (ParticleSystem shipTrail in mSecondShip.GetComponentsInChildren<ParticleSystem>())
//			{
//				shipTrail.enableEmission = false;
//			}
//		}
		
	}//END of Update()
	
	void LateUpdate () 
	{
		//Keep ship within screen bounds
		if (transform.position.x < -17.5 && mShipRecovered)
		{
			transform.position = new Vector3(-17.5f, transform.position.y, transform.position.z);
		}
		else if(transform.position.x < -20f)
		{
			transform.position = new Vector3(-20f, transform.position.y, transform.position.z);
		}
		if (transform.position.x > 20f)
		{
			transform.position = new Vector3(20f, transform.position.y, transform.position.z);
		}
		if(transform.position.y < -33f)
		{
			transform.position = new Vector3(transform.position.x, -33f, transform.position.z);
		}
		if (transform.position.y > 23f)
		{
			transform.position = new Vector3(transform.position.x, 23, transform.position.z);
		}
		
		if(mThreeBulletTimer <= 0f)
		{
			mThreeBullet = false;
		}
	}//END of LateUpdate()
	
	public void ToggleFire()
	{
		mToggleFireOn = !mToggleFireOn;
	}//END of ToggleFire()
	
	void OnLevelWasLoaded(){
		Input.ResetInputAxes();
		
		//mToggleFireOn = false;
	}//END of OnLevelWasLoaded()
	


	public void StartSpin()
	{
		mSpinTimer = mSpinTimerDefault;
		mSpinning = Random.Range(-1,1);
		if (mSpinning == 0f)
		{
			mSpinning += 0.1f;
		}
	}//END of StartSpin()
	

	
	public void SpinShip(float spinDir)
	{
		if(spinDir > 0f)
		{
			mMainShip.transform.Rotate(Vector3.forward*Time.deltaTime*720f);
			mSecondShip.transform.Rotate(Vector3.forward*Time.deltaTime*-720f);
		}
		else if (spinDir < 0f)
		{
			mMainShip.transform.Rotate(Vector3.forward*Time.deltaTime*-720f);
			mSecondShip.transform.Rotate(Vector3.forward*Time.deltaTime*720f);
		}
		
	}//END of SpinShip()
	
	//For getting hit by boss beams ~Adam
	void OnParticleCollision(GameObject other)
	{
		Debug.Log("The clone was shot by a particle");
		CloneShipDie();
	}//END of OnParticleCollision()

	public void CloneShipDie()
	{
		//Spin if shielded when hit, otherwise die ~Adam
		if(mShielded)
		{
			StartSpin();
		}
		else
		{
			GameObject cloneDeathParticles;
			cloneDeathParticles = Instantiate(mCloneDeathEffect, transform.position, Quaternion.identity) as GameObject;
			Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();
			Destroy(this.gameObject);
		}
	}//END of CloneShipDie()

}//END of MonoBehavior