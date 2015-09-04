using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using XInputDotNetPure; // Required in C#

public class PlayerShipController : MonoBehaviour 
{
	public bool secondShipOnHip = true;

	public GameObject mPlayerClone;

	//For multiplaye co-op ~Adam
	public PlayerTwoShipController mPlayerTwo;
	public InputDevice mPlayerOneInputDevice;
	public string mPlayerOneInputMeta = "";

	public GameObject mDamageParticles;//Particles that play when the player gets hit ~Adam
	bool playerIndexSet = false;
	public PlayerIndex playerIndex;
//	GamePadState state;
//	GamePadState prevState;
	
	public bool cheats = false;
	//For if animating the ship ~Adam
	[SerializeField] private Animator mMainShipAnimator;
	[SerializeField] private Animator mSecondShipAnimator;
	
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
	
	
	//For Overheating
	public bool overHeatProcess = true;
	public bool isOverheated = false;
	public float heatLevel = 0f;
	public float mBaseHeatMax = 60f;
	public float maxHeatLevel;

	//For when the player has 3 bullets  ~Adam
	public bool mThreeBullet = true;
	public float mThreeBulletTimer = 0f;
	public GameObject mSideBullet;

	//For when the ship has a shieldPowerUp ~Adam
	public bool mShielded = false;
	[SerializeField] private SpriteRenderer mMainShipShieldSprite;
	[SerializeField] private SpriteRenderer mSecondShipShieldSprite;
	public float mShieldTimer = 0f;

	
	//For deleting duplicate ships when we change levels ~Adam
	public int mShipCreationLevel;
	public bool mToggleFireOn = true;
	
	//For tracking where the ship was last frame so we can see how much/in what direction its moving ~Adam
	public Vector3 mLastFramePosition;
	public Vector3 mLastFrameDifference = Vector3.zero;
	float mLastNonZeroHorizontalDifference;
	bool mDriftDown = true;
	
	//For spinning the ship around when the player gets hit ~Adam
	float mSpinning = 0f;
	float mSpinTimer = 0f;
	float mSpinTimerDefault = 0.5f;
	
	//For Super Screen-Wiper powerup ~Adam
	public bool mHaveLaserFist = false;
	public GameObject mLaserFist;
	public bool mHaveBigBlast = false;
	public GameObject mBigBlast;
	
	
	
	//For making the ship flash when hit
	public GameObject mMainShipHitSprite;
	public GameObject mSecondShipHitSprite;
	
	// Use this for initialization
	void Start () 
	{

		//transform.localScale = new Vector3 (1.75f, 1.75f, 1.75f);

		//Adjust speed and scale for mobile ~Adam
		if (Application.isMobilePlatform)
		{
			mBaseMovementSpeed = 15.0f;
			transform.localScale = new Vector3(1.5f,1.5f,1.5f);
		}
		
		mShipCreationLevel = Application.loadedLevel;
		
		PlayerShipController[] otherPlayerShips = FindObjectsOfType<PlayerShipController>();
		//Debug.Log(otherPlayerShip.name);
		foreach(PlayerShipController othership in otherPlayerShips)
		{
			if(othership.mShipCreationLevel < this.mShipCreationLevel)
			{
				Debug.Log("Found another ship so destroying self.");
				Destroy(this.gameObject);
			}
		}
		
		mLastFramePosition = transform.position;
		
	}//END of Start()
	
	
	//Persist between level loads/reloads ~Adam
	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
		InputManager.Setup();


	}//END of Awake()
	
	
	// Update is called once per frame
	void Update () 
	{

//		prevState = state;
//		state = GamePad.GetState(playerIndex);

#if UNITY_ANDROID // Make hitbox smaller for Android
        if (gameObject.GetComponent<BoxCollider>().size != new Vector3 (1.5f, 1.5f, 1.5f))
        {
            gameObject.GetComponent<BoxCollider>().size = new Vector3(1.5f, 1.5f, 1.5f);
        }
#endif

		#region for managing input devices on co-op mode ~Adam
		if (mPlayerOneInputDevice == null) 
		{
		//	Debug.Log("Setting player one controller");
			if(mPlayerOneInputMeta == "")
			{
				mPlayerOneInputDevice = InputManager.ActiveDevice;
				mPlayerOneInputMeta = mPlayerOneInputDevice.Meta;
			}
			else if (mPlayerOneInputMeta == InputManager.ActiveDevice.Meta)
			{
				mPlayerOneInputDevice = InputManager.ActiveDevice;
				mPlayerOneInputMeta = mPlayerOneInputDevice.Meta;
			}
		}
		else
		{
		//	Debug.Log("Player 1: "+mPlayerOneInputDevice.Name);
		}
		#endregion


		//maxHeatLevel = mBaseHeatMax +  mBaseHeatMax * Application.loadedLevel/26f;
		maxHeatLevel = mBaseHeatMax +  mBaseHeatMax * Application.loadedLevel/(Application.levelCount-3);//26f;
		GetComponent<AudioSource>().volume = 0.18f*(30f-Application.loadedLevel)/30f;
		
		if (cheats) 
		{
			
			if(Input.GetKeyDown(KeyCode.Q))
			{
				Application.LoadLevel(Application.loadedLevel + 1);
				mShipStolen = false;
			}
			
			if(Input.GetKeyDown(KeyCode.R))
			{
				Application.LoadLevel(Application.loadedLevel - 1);
				mShipStolen = false;
			}
		}
		
		//Spin the ships when hit
		if(mSpinning != 0f)
		{
			mSpinTimer -= Time.deltaTime;
			SpinShip(mSpinning);
			
			if (mSpinTimer <= 0f)
			{
				mSpinning = 0f;
				mMainShip.transform.rotation = Quaternion.identity;

				if(secondShipOnHip)
				{
					mSecondShip.transform.rotation = Quaternion.identity;
				}
				else
				{

					mSecondShip.transform.rotation = Quaternion.Euler(0,0,180);
				}
				//mSecondShip.transform.rotation = Quaternion.identity;
//				mSecondShip.transform.rotation = Quaternion.Euler(0,0,180);
			}
		}
		

		//Toggle shield sprites ~Adam
		if(mShielded)
		{
			mMainShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 1);
			mSecondShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 1);
			
			mMainShipShieldSprite.enabled = true;
			mMainShipShieldSprite.GetComponent<Light>().enabled = true;
			if(mShipRecovered)// && Application.isMobilePlatform) (mobile part is from when we were doing twin-stick)
			{
				mSecondShipShieldSprite.enabled = true;
				mSecondShipShieldSprite.GetComponent<Light>().enabled = true;
			}
			else
			{
				mSecondShipShieldSprite.enabled = false;
			}
			//Decrease Shield time ~Adam
			mShieldTimer -= Time.deltaTime;
			if(mShieldTimer <= 0f)
			{
				mShielded = false;
			}
			if(mShieldTimer < 2f)
			{
				mMainShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 0);
				mSecondShipShieldSprite.GetComponent<Animator>().SetInteger ("ShieldState", 0);
				
			}
			if(mShieldTimer < 5f)
			{
				mMainShipShieldSprite.GetComponent<Renderer>().material.color = Color.Lerp (mMainShipShieldSprite.GetComponent<Renderer>().material.color, Color.red,0.1f);
				mSecondShipShieldSprite.GetComponent<Renderer>().material.color = Color.Lerp (mSecondShipShieldSprite.GetComponent<Renderer>().material.color, Color.red,0.1f);
			}
			else
			{
				mMainShipShieldSprite.GetComponent<Renderer>().material.color = Color.white;
				mSecondShipShieldSprite.GetComponent<Renderer>().material.color = Color.white;
			}
			
		}
		else
		{
			mMainShipShieldSprite.enabled = false;
			mMainShipShieldSprite.GetComponent<Light>().enabled = false;
			mSecondShipShieldSprite.enabled = false;
			mSecondShipShieldSprite.GetComponent<Light>().enabled = false;
			
		}
		
		//Increase movement speed as we progress through levels
		if(Time.timeScale > 0f)
		{
		//	mMovementSpeed = ( mBaseMovementSpeed + (6f/25f*(Application.loadedLevel)) ) /Time.timeScale;
			mMovementSpeed = ( mBaseMovementSpeed + (0.24f +5.76f*(Application.loadedLevel-1)/(Application.levelCount-4) )) /Time.timeScale;
		}
		else
		{
		//	mMovementSpeed = ( mBaseMovementSpeed + (6f/25f*(Application.loadedLevel)) );
			mMovementSpeed = ( mBaseMovementSpeed + (0.24f +5.76f*(Application.loadedLevel-1)/(Application.levelCount-4) )) /Time.timeScale;
		}
		//Make the player drift toward the bottom of the screen
		// transform.position += new Vector3(0f,mDropSpeed*-1f, 0f);
		if(mMoveDir.y < 0f && mDriftDown)
		{
			foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
			{
				if(shipTrail.gameObject != mDamageParticles)
				{
					shipTrail.enableEmission = false;
				}
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
				if(shipTrail.gameObject != mDamageParticles)
				{
					if(!(mShipRecovered && !secondShipOnHip))
					{
						shipTrail.enableEmission = true;
					}
					else
					{
						shipTrail.enableEmission = false;
					}
				}
			}
			
			mDropSpeed -= mDropDeccelRate;
			
			if(mDropSpeed <= 0.01f)
			{
				mDropSpeed = 0.01f;
			}
		}
		
		//Make the player drift faster towards the bottom while firing ~Adam
		if(mToggleFireOn && Time.timeScale != 0f)
		{
			//Don't drift down from firing if you have a second ship pointing down (because it would be pushing you back up) ~Adam
			if(!(mShipRecovered && !secondShipOnHip) )
			{
				//transform.position += new Vector3(0f,-0.00255f*Application.loadedLevel, 0f);
				transform.position += new Vector3(0f,-0.06375f*Application.loadedLevel/(Application.levelCount-3), 0f);
			}
			//Decrease the timer on triple bullets while firing ~Adam
			mThreeBulletTimer -= Time.deltaTime;
		}
		

		//Default keyboard/gamepad stick input ~Adam
		float horizontal = 0f;
		float vertical = 0f;

		//If statement for avoiding getting NaN returns when paused
		if(!GetComponent<PauseManager>().isPaused && !GetComponent<PauseManager>().isPrePaused)
		{
			//horizontal = Input.GetAxis("Horizontal");
			//vertical = Input.GetAxis("Vertical");

			if(mPlayerOneInputDevice.LeftStick.X > 0.3f || mPlayerOneInputDevice.LeftStick.X < -0.3f)
			{
				horizontal = mPlayerOneInputDevice.LeftStick.X;
			}
			if(mPlayerOneInputDevice.LeftStick.Y > 0.3f || mPlayerOneInputDevice.LeftStick.Y < -0.3f)
			{
				vertical = mPlayerOneInputDevice.LeftStick.Y;
			}

			if(Input.GetKey(KeyCode.W))
				vertical = 1;
			if(Input.GetKey(KeyCode.A))
				horizontal = -1;
			if(Input.GetKey(KeyCode.S))
				vertical = -1;
			if(Input.GetKey(KeyCode.D))
				horizontal = 1;

			/*if(Input.GetKey(KeyCode.UpArrow))
				vertical = 1;
			if(Input.GetKey(KeyCode.LeftArrow))
				horizontal = -1;
			if(Input.GetKey(KeyCode.DownArrow))
				vertical = -1;
			if(Input.GetKey(KeyCode.RightArrow))
				horizontal = 1;*/

			//Use P2 controls for P1 if in single-player mode ~Adam
			/*if(mPlayerTwo == null || (mPlayerTwo != null && !mPlayerTwo.isActiveAndEnabled) )
			{
				if(Input.GetAxis("HorizontalP2") != 0)
				{
					horizontal = Input.GetAxis("HorizontalP2");
				}
				if(Input.GetAxis("VerticalP2") != 0)
				{
					vertical = Input.GetAxis("VerticalP2");
				}
			}*/			//Decivating this because player one moved when player two moved. ~ Jonathan

			//Gamepad D-Pad input ~Adam
			if(mPlayerOneInputDevice.DPadDown.IsPressed)
			{
				vertical = -1f;
			}
			if(mPlayerOneInputDevice.DPadUp.IsPressed)
			{
				vertical = 1f;
			}
			if(mPlayerOneInputDevice.DPadLeft.IsPressed)
			{
				horizontal = -1f;
			}
			if(mPlayerOneInputDevice.DPadRight.IsPressed)
			{
				horizontal = 1f;
			}
		}
		mMainShipAnimator.SetInteger("Direction", Mathf.RoundToInt(horizontal));
		mSecondShipAnimator.SetInteger("Direction", Mathf.RoundToInt(horizontal));

		
		//Delete the ship if we've returned to the title screen
		if(Application.loadedLevel == 0)
		{
			Destroy(this.gameObject);
		}
		
		
		
		
		
		
		//Keyboard Movement Controls
		//For making the ship drift down when not trying to go up
		if(vertical > 0f)
		{
			mDriftDown = false;
		}
		else
		{
			mDriftDown = true;
		}
		
		//Movement input for mouse/touch
		if(Input.GetMouseButton(0) && (Application.isMobilePlatform)  && Time.timeScale != 0f)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
			//Debug.Log(screenPos + ", " + Input.mousePosition);
			
			Vector3 transitionAboveFinger = new Vector3(0f, Screen.height * 0.1f, 0f);
			Vector3 translationDirection = Vector3.Normalize(Input.mousePosition + transitionAboveFinger - screenPos);
			
			//Debug.Log(translationDirection*mMovementSpeed*Time.deltaTime);
			
			//For making the ship drift down when not trying to go up
			if (Input.mousePosition.y + 10f + Screen.height * 0.1f > screenPos.y - 10f)
			{
				mDriftDown = false;
			}
			else
			{
				mDriftDown = true;
			}
			//transform.Translate(new Vector3(translationDirection.x, translationDirection.y, 0f)*mMovementSpeed*Time.deltaTime);
			//mMoveDir +=new Vector3(translationDirection.x, translationDirection.y, 0f)*0.5f*mMovementSpeed*Time.deltaTime;
#if UNITY_ANDROID
            mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(translationDirection.x, translationDirection.y, 0f) * 2f * mMovementSpeed * Time.deltaTime, 0.5f);
#else
            mMoveDir = Vector3.Lerp(mMoveDir, new Vector3(translationDirection.x, translationDirection.y, 0f)*2f*mMovementSpeed*Time.deltaTime, 0.08f);
#endif

        }
		
		// if (Input.GetKey (KeyCode.Mouse0)) {
		// transform.Translate(new Vector3(0.0f, (mMovementSpeed * Time.deltaTime) + .6f, 0.0f));
		// }
		
		//Taking in directional Input from the keyboard
		else if (horizontal != 0.0f || vertical != 0.0f && Time.timeScale != 0f)
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
		
		//Toggle bullet firing ~Adam
		//Keyboard and mouse input and InControl Gamepad input ~Adam
		if(mPlayerOneInputDevice.Action1.WasPressed || mPlayerOneInputDevice.Action4.WasPressed || Input.GetButtonDown("FireGun"))
		{
			Debug.Log("InControl button pressed");
			ToggleFire();
		}

		//Fire held super weapon ~Adam
		//Can hold multiple super weapons.  They fire in a priority order: Big Blast, then Laser Fist ~Adam
		//Have to wait for one to finish firing before firing another ~Adam
		if( (mPlayerOneInputDevice.RightBumper.WasPressed || Input.GetButtonDown("FireSuperGun")) && !mBigBlast.activeSelf && !mLaserFist.activeSelf)
		{
			if(mHaveLaserFist)
			{
				mLaserFist.SetActive(true);
				mHaveLaserFist = false;

				Camera.main.GetComponent<CameraShaker> ().RumbleController(.3f, 5.5f);
			}
			else if(mHaveBigBlast)
			{
				mBigBlast.SetActive(true);
				mHaveBigBlast = false;

				Camera.main.GetComponent<CameraShaker> ().RumbleController(.6f, 2f);
			}
		}


		//Jonathan's alternate control input
//		if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released) {
//
//			Debug.Log("InControl button pressed");
//			ToggleFire();
//		}
		
		if (isOverheated) 
		{
			mToggleFireOn = false;
			mMainShip.GetComponent<Renderer>().material.color = Color.Lerp(mMainShip.GetComponent<Renderer>().material.color,Color.red,0.05f);
			mSecondShip.GetComponent<Renderer>().material.color = Color.Lerp(mSecondShip.GetComponent<Renderer>().material.color,Color.red,0.05f);
		}
		else if (heatLevel/maxHeatLevel > 0.9f) 
		{
			mMainShip.GetComponent<Renderer>().material.color = Color.Lerp(mMainShip.GetComponent<Renderer>().material.color,Color.yellow,0.1f);
			mSecondShip.GetComponent<Renderer>().material.color = Color.Lerp(mSecondShip.GetComponent<Renderer>().material.color,Color.yellow,0.1f);
		}
		
		else
		{
			mMainShip.GetComponent<Renderer>().material.color = Color.Lerp(mMainShip.GetComponent<Renderer>().material.color,Color.white,0.1f);
			mSecondShip.GetComponent<Renderer>().material.color = Color.Lerp(mSecondShip.GetComponent<Renderer>().material.color,Color.white,0.1f);
		}
		
		//firing bullets
		if (mToggleFireOn) 
		{
			
			if(!isOverheated)
			{
				if(heatLevel < maxHeatLevel)
				{
					heatLevel += Time.deltaTime;
				}
				
				if(heatLevel >= maxHeatLevel)
				{
					
					heatLevel = maxHeatLevel;
					isOverheated = true;
				}
				
				//Firing Bullets
				if (Time.time > mBulletFireTime) 
				{
					
					// Make the bullet object
					GameObject newBullet = Instantiate (mBulletPrefab, mBulletSpawns[0].position, mMainShip.transform.rotation * Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f))) as GameObject;
					
					if (mThreeBullet) 
					{
						
						if(!mShipRecovered)// || !Application.isMobilePlatform) (mobile part is from when we were doing twin-stick)
						{
							Instantiate (mSideBullet, mBulletSpawns[2].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, 10f) * Quaternion.Euler (0f,0f,Random.Range(-5.0f,5.0f)));
						}
						//Adjust triple-bullet firing when you have the double/side ship ~Adam
						else if(mShipRecovered)// && Application.isMobilePlatform)
						{
							Instantiate (mSideBullet, mBulletSpawns[2].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, 5f) * Quaternion.Euler (0f,0f,Random.Range(-10.0f,3.0f)));
						}
						Instantiate (mSideBullet, mBulletSpawns[3].position, mMainShip.transform.rotation * Quaternion.Euler (0f, 0f, -10f) * Quaternion.Euler (0f,0f,Random.Range(-5.0f,5.0f)));
						
					}
					//Play bullet-firing sound effect ~Adam
					GetComponent<AudioSource> ().Play ();
			
					//Do side ship bullets
					if (mShipRecovered)// && Application.isMobilePlatform)  (mobile part is from when we were doing twin-stick)
					{


						GameObject secondBullet;
						secondBullet = Instantiate (mBulletPrefab, mBulletSpawns[1].position, mSecondShip.transform.rotation * Quaternion.Euler (0f,0f,Random.Range(-3.0f,3.0f))) as GameObject;
						secondBullet.name = "SECONDBULLET";
						if (mThreeBullet) 
						{
							Instantiate (mSideBullet, mBulletSpawns[4].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, 10f) * Quaternion.Euler (0f,0f,Random.Range(-5.0f,5.0f)));

							Instantiate (mSideBullet, mBulletSpawns[5].position, mSecondShip.transform.rotation * Quaternion.Euler (0f, 0f, -5f) * Quaternion.Euler (0f,0f,Random.Range(-3.0f,10.0f)));
						}
					}
					//Reset the timer to fire bullets.  The later the level, the smaller the time between shots
					if(mSpinning == 0)
					{
						if(Application.loadedLevelName != "Credits")
						{
						//	mBulletFireTime = Time.time + bulletShootSpeed - (0.25f / 25f * (Application.loadedLevel));
							mBulletFireTime = Time.time + bulletShootSpeed - ((0.01f +.24f*(Application.loadedLevel-1)/(Application.levelCount-4) ));
						}
						else
						{
							mBulletFireTime = Time.time + (bulletShootSpeed - (0.25f / 25f * 21f));
						}
					}
					else
					{
						mBulletFireTime = Time.time + (bulletShootSpeed - 0.25f)/3f;
					}
				}
			}
		}
		else 
		{
			
			if(heatLevel > 0)
			{
				
				if(isOverheated)
				{
					heatLevel -= Time.deltaTime * maxHeatLevel/5f;
				}
				else
				{
					heatLevel -= Time.deltaTime * 3f;
				}
			}
		}
		
		if (heatLevel <= 0f) 
		{
			
			isOverheated = false;
			if (Application.isMobilePlatform) //Start shooting when weapons are Cool. Lol, weapons are always cool.
			{
				mToggleFireOn = true;
			}
		}

		//Keyboard and mouse input and InControl Gamepad input ~Adam
		if(mPlayerOneInputDevice.Action2.IsPressed || mPlayerOneInputDevice.Action3.IsPressed || Input.GetButton("Thrusters"))
		{
			//Slow down movement while hovering~Adam
			mMoveDir *= 0.95f;

			mDropSpeed -= mDropDeccelRate*3f;
			if(mDropSpeed <= 0.01f)
			{
				mDropSpeed = 0.00f;
			}
			
			if(mMoveDir.y < -0.2f)
			{
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
				{
					if(shipTrail.gameObject != mDamageParticles)
					{
						shipTrail.enableEmission = false;
					}
				}
			}
			else
			{
				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
				{
					if(shipTrail.gameObject != mDamageParticles)
					{
						if(!(mShipRecovered && !secondShipOnHip))
						{
							shipTrail.enableEmission = true;
						}
						else
						{
							shipTrail.enableEmission = false;
						}
					}
				}
			}
		}

		//Jonathan's alternative input method
//		if (state.Buttons.X == ButtonState.Pressed) {
//
//			mDropSpeed -= mDropDeccelRate*3f;
//			if(mDropSpeed <= 0.01f)
//			{
//				mDropSpeed = 0.00f;
//			}
//			
//			if(mMoveDir.y < -0.2f)
//			{
//				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
//				{
//					shipTrail.enableEmission = false;
//				}
//			}
//			else
//			{
//				foreach (ParticleSystem shipTrail in this.GetComponentsInChildren<ParticleSystem>())
//				{
//					shipTrail.enableEmission = true;
//				}
//			}
//		}
		
		//Move the ship by the mMoveDir vector if not paused
		if(Time.timeScale != 0)
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
//		mMainShipAnimator.speed = Application.loadedLevel/5f+1f;
//		mSecondShipAnimator.speed = Application.loadedLevel/5f+1f;
		mMainShipAnimator.speed = Application.loadedLevel/(Application.levelCount-3)*5f +1f;//Application.loadedLevel/5f+1f;
		mSecondShipAnimator.speed = Application.loadedLevel/(Application.levelCount-3)*5f +1f;//Application.loadedLevel/5f+1f;
		if(mToggleFireOn)
		{
			mMainShipAnimator.SetBool("IsFiring", true);
			mSecondShipAnimator.SetBool("IsFiring", true);
		}
		else
		{
			mMainShipAnimator.SetBool("IsFiring", false);
			mSecondShipAnimator.SetBool("IsFiring", false);
		}
		



		//Control whether or not to render the second ship on the side ~Adam
		if (mShipRecovered)
		{
			mSecondShip.GetComponent<SpriteRenderer>().enabled = true;
			foreach (ParticleSystem shipTrail in mSecondShip.GetComponentsInChildren<ParticleSystem>())
			{
				if(!(mMoveDir.y < 0f && mDriftDown))
				{
					if(shipTrail.gameObject != mDamageParticles)
					{
						if(secondShipOnHip)
						{
							shipTrail.enableEmission = true;
						}
						else
						{
							shipTrail.enableEmission = false;
						}
					}
				}
			}
		}
	
		else
		{
			mSecondShip.GetComponent<SpriteRenderer>().enabled = false;
			foreach (ParticleSystem shipTrail in mSecondShip.GetComponentsInChildren<ParticleSystem>())
			{
				if(shipTrail.gameObject != mDamageParticles)
				{
					shipTrail.enableEmission = false;
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.V) || mPlayerOneInputDevice.LeftBumper.WasPressed) 
		{
			

			if (mShipRecovered) 
			{
				secondShipOnHip = !secondShipOnHip;

				if(secondShipOnHip)
				{
					
					mSecondShip.transform.localPosition = new Vector3(-3.5f, -0.1f,-0.53f);
					mSecondShip.transform.localScale = new Vector3(8, 8, 8);
					mSecondShip.transform.rotation = Quaternion.identity;			
				}
				else
				{
					
					mSecondShip.transform.localPosition = new Vector3(0, -3.3f,-0.53f);
					mSecondShip.transform.localScale = new Vector3(-8, 8, 8);
					mSecondShip.transform.rotation = Quaternion.Euler(0,0,180);
				}
			}
		}


		#region For using the twin-stick clone
//		//If not mobile, base ship recoved on whether or not the clone ship is alive/active ~Adam
//		else
//		{
//			mShipRecovered = (mPlayerClone != null);
//		}
		#endregion
	}//END of Update()
	
	void LateUpdate () 
	{
		//Keep ship within screen bounds
		if (transform.position.x < -16.5 && mShipRecovered && secondShipOnHip)// && Application.isMobilePlatform) (from when we were doing twin-stick
		{
			transform.position = new Vector3(-16.5f, transform.position.y, transform.position.z);
		}																							//Second ship is in new position now ~ Jonathan
		else if(transform.position.x < -20f)
		{
			transform.position = new Vector3(-20f, transform.position.y, transform.position.z);
		}
		if (transform.position.x > 20f)
		{
			transform.position = new Vector3(20f, transform.position.y, transform.position.z);
		}
		if(transform.position.y < -29.5f && mShipRecovered && !secondShipOnHip) //Original is -33, but there is a new second ship position now ~ Jonathan
		{
			transform.position = new Vector3(transform.position.x, -29.5f, transform.position.z);
		}else if(transform.position.y < -33f)
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

		mPlayerOneInputDevice = null;
//		mPlayerOneInputMeta = "";
//		Debug.Log("reseting input1 meta to" + mPlayerOneInputMeta);
		//mToggleFireOn = false;
	}
	

	
	public void StartSpin()
	{
		mSpinTimer = mSpinTimerDefault;
		mSpinning = Random.Range(-1,1);
		if (mSpinning == 0f)
		{
			mSpinning += 0.1f;
		}
	}
	

	
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
		
	}
	
	//For getting hit by boss beams ~Adam
	void OnParticleCollision(GameObject other)
	{
		Debug.Log("The player was shot by a particle");
		FindObjectOfType<ScoreManager>().LoseALife();
	}
	
}//END of MonoBehavior