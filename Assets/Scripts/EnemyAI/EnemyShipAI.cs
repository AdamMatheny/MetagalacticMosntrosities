using UnityEngine;
using System.Collections;

//This is the script attached to all of the enemy ships in the game, dictating their AI behavior
//This script was partially written before I joined the project.  Parts that I did not write or re-write are marked.


public class EnemyShipAI : MonoBehaviour 
{

//The Serialie Field private variables and public variables can be changed in the editor to assign different behaviors to multiple enemy prefabs, using a single script
//Additionally, the public variables can be overwritten by in-editor settings on the enemy spawner prefab to adjust enemy behavior or difficulty on a level-by-level basis



	public float secondaryExplosionChance = 25f;
	public GameObject secondaryExplosion;

	public GameObject bulletExplosion;

	public float LRBulletChance = .25f;
	public float middleBulletChance = .95f;

	//Used for telling the ship which way to go
	[HideInInspector] public Vector2 mVel= new Vector2(0, 1);

	//The player's avatar ~Adam
	public Transform mPlayer;

	//The player 2 ship ~Adam  
	public Transform mPlayerClone;

	//Whether or not the ship has flown in a circle ~Adam
	public bool mHasLooped = false;
	//The point thta the ship will fly in a circle around on its way to the swarm if it isn't null ~Adam
	public GameObject mLoopPoint;
	//How tight of a circle the ship will fly in when it makes a loop.  Smaller absolute value == bigger loop.  Negative makes loop counter-clockwise ~Adam
	public float mLoopCircleTightness = 1.0f;

	//Minimum amount of time to spend on loops. ~Adam
	public float mLoopTime = 1f;

	//The speed of the ship ~Adam
	public float mSpeed = 5.0f;
	//For having the speed used to make formations be faster than the regular speed `Adam
	public float mFormSpeed = 5.0f;
	public float mDefaultSpeed;
	//Timer for changing AI states
	public float mSwitchCoolDown;

	//The swarm this unit will be joining ~Adam
	public SwarmGrid mSwarmGrid;
	//Where to hover in the swarm grid ~Adam
	public GameObject mSwarmGridPosition;
	//How long this unit hovers in the swarm before trying to attack ~Adam
	public float mAttackFrequencyTimerDefault = 8.0f;
	public float mAttackLengthTimerDefault = 8.0f;
	private float mAttackFrequencyTimer;
	//The particle system that spawns when this unit dies ~Adam
	public GameObject mDeathEffect;

	//Whether or not this unit sometimes does a loop when returning to the swarm ~Adam
	[SerializeField] private bool mPostAttackLoop = false;
	//For making the green and teal enemies always loop the same direction on retreat ~Adam
	[SerializeField] private bool mRetreatClockwise = true;

	//Whether or not this is a unit that tries to grab the player ~Adam
	public bool mGrabber = false; //Made public to be used by CoOpShooter
	//Whether or not this is a unit that tries to shoot the player ~Adam
	public bool mShooter = false; //Made public so this could be checked by CoOpShooter ~ Jonathan
	public bool mAutoShoot = false;
	[SerializeField] private bool mRandomShootTimer = false;
	[SerializeField] private bool mLoopShooter = false; //For only shooting while doing a loop ~Adam
	[SerializeField] private bool mRetreating = false; //For only shooting while doing a loop ~Adam
	[SerializeField] private bool mDeathShooter = false; //For firing a shot on death ~Adam


	//How often to shoot `Adam
	public float mShootTimerDefault = 1f;
	float mShootTimer;
	//the projectile that gets instantiated when shooting `Adam
	[SerializeField] private GameObject mEnemyBullet;

	//Enums for the current AI behavior state
	public enum AIState { FlightLooping, ApproachingSwarm, Swarming, Attacking };
	public AIState mCurrentAIState = AIState.ApproachingSwarm;

	//How much this unit is worth when destroyed
	[SerializeField] private int mPointValue = 1;
	//A reference to the object that handles scoring and player death `Adam
	ScoreManager mScoreManager;
	//A reference to the object that tracks enemy deaths per level for the purpose of spawning new waves of enemies and whether or not the level has been completed `Adam
	LevelKillCounter mKillCounter;

	//Is this enemy's death required to beat the level?  `Adam
	//Mainly used to distinguish between enemies that are part of the main swarm and enemies that fly across the screen and then destroy themselves once off-screen. `Adam
	public bool mDeathRequired = true;

	//Transform that is used for stealing copies of the player ship `Adam
	public Transform mTowPoint;
	//Prefab for ship to spawn set in-editor `Adam
	[SerializeField] GameObject mCapturedShip;
	public GameObject mShipInTow;

	//For enemies that we just want to fly across the screen and then disappear `Adam
	public bool mLimitedLifespan = false;
	public float mLifespanLength = 10f;

	//For keeping the enemies in formation at the start
	public float mMinimumFirstAttackTime = 0f;

	//For making the Grabbers invincible while attacking `Adam
	public bool mInvincible = false;
	[SerializeField] private GameObject mShieldBubble;


	//For controlling animation ~Adam
	[SerializeField] private Animator mAnimator;

	//For deleting enemies that get stuck off-screen ~Adam
	[SerializeField] private float mAutoDeleteTimer = 60f;



	//For determining which player killed this enemy ~Adam
	[HideInInspector] public int mKillerNumber = 0;

	// Use this for initialization
	void Start () 
	{
		//Find the other objects in the scene that we're going to be referencing
		if(GameObject.FindGameObjectWithTag("Player")!= null)
		{
			mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
		}
		#region for if there is a second player present ~Adam
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			mPlayerClone = FindObjectOfType<PlayerTwoShipController>().gameObject.transform;
		}
		#endregion
		mScoreManager = FindObjectOfType<ScoreManager>();
		mKillCounter = FindObjectOfType<LevelKillCounter>();

		//Find out where in the swarm grid this ship will be going (the next unoccupied slot)
		//mSwarmGrid is assigned by the enemy spawner
		mSwarmGridPosition = mSwarmGrid.GetGridPosition();

		//Set timers to their default values
		mAttackFrequencyTimer = mAttackFrequencyTimerDefault;
		mShootTimer = mShootTimerDefault;

		mDefaultSpeed = mSpeed;
		mSpeed = mFormSpeed;

		//Scale up for mobile screen ~Adam
		if(Application.isMobilePlatform)
		{
			//transform.localScale = new Vector3(1.75f,1.75f,1.75f);
		}
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		if(mPlayer == null)
		{
			if(GameObject.FindGameObjectWithTag("Player")!= null)
			{
				mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
			}
		}

		//Also kill the ship FINALLY!!! ~ Jonathan
		if(mPlayer.GetComponent<PlayerShipController> ()!= null)
		{
			if (mPlayer.GetComponent<PlayerShipController> ().mShipRecovered) 
			{
				
				if(Vector3.Distance(this.transform.position, mPlayer.GetComponent<PlayerShipController> ().mSecondShip.transform.position) <= 1.5f){
					
					Debug.Log("The second ship was shot");
					mScoreManager.LoseALife();
					Destroy(gameObject);
				}
			}
		}
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>();
		}
		//destroy self if no empty place in the swarm
		if (mSwarmGridPosition == null)
		{
			Destroy(this.gameObject);
		}

		//Destroy self if set to have a limited lifespan
		if(mLimitedLifespan)
		{
			mLifespanLength -= Time.deltaTime;
			if(mLifespanLength < 0f)
			{
				mSwarmGridPosition.GetComponent<SwarmGridSlot>().mOccupied = false;
				Destroy(this.gameObject);
			}
		}

		//Make the Grabber not invincible while not attacking or towing
		if(mGrabber && !mShipInTow && mCurrentAIState != AIState.Attacking)
		{
			mInvincible = false;
			//GetComponentInChildren<Renderer>().material.color = Color.white;
			if(mShieldBubble != null)
			{
				mShieldBubble.SetActive(false);
			}
		}
		//Toggle the visibilty of the shield bubble based on invincibility
		if(mInvincible)
		{
			//GetComponentInChildren<Renderer>().material.color = Color.magenta;
			if(mShieldBubble != null)
			{
				mShieldBubble.SetActive(true);
			}}
		else
		{
			//GetComponentInChildren<Renderer>().material.color = Color.white;
			if(mShieldBubble != null)
			{
				mShieldBubble.SetActive(false);
			}
		}

		#region This segment was already written when I joined.  Although I did update the variable and enum names for legibility and consistency.
		//Decrement the state switch timer
		mSwitchCoolDown -= Time.deltaTime;

		//Act based on what state we are currently in
		switch (mCurrentAIState)
		{
		case AIState.ApproachingSwarm:
			ApproachSwarm();
			break;
			
		case AIState.FlightLooping:
			DoFlightLoop();
			break;
			
		case AIState.Swarming:
			Swarm();
			break;
			
		case AIState.Attacking:
			AttackPlayer();
			break;
			
		default:
			break;
		}
		#endregion

		//Controls for firing projectiles ~Adam
		//recharging shooting ~Adam
		mShootTimer -= Time.deltaTime;

		//For enemies that only fire while doing a loop(i.e. green) ~Adam
		if(mLoopShooter)
		{
			if(mCurrentAIState == AIState.FlightLooping && mRetreating)
			{
				mShooter = true;
			}
			else
			{
				mShooter = false;
			}
		}

		if(mShootTimer <= 0f && mEnemyBullet != null && (transform.position.y <= 24f && transform.position.y >= -33f)&& (transform.position.x <= 20f && transform.position.y >= -20f))
		{
			//Fire automatically if set to do so ~Adam
			if(mShooter && mAutoShoot && mPlayer.GetComponent<PlayerShipController>().enabled == true)
			{
				//Play animation for firing (or skip straight to firing if no animation) ~Adam
				if(mAnimator != null)
				{
					mAnimator.Play("Shoot");
					if(mGrabber){ShootEnemyBullet();}
				}
				else
				{
					ShootEnemyBullet();
				}
				//Whether the timer is randomized or not ~Adam
				if(mRandomShootTimer)
				{
					mShootTimer = Random.Range(mShootTimerDefault/2f, mShootTimerDefault);
				}
				else
				{
					mShootTimer = mShootTimerDefault;
				}
			}
			//Otherwise, instantiate an enemy bullet if the player is below this enemy `Adam
			else if (mShooter && Mathf.Abs(mPlayer.position.x - transform.position.x) <= 2f)
			{
				//Play animation for firing (or skip straight to firing if no animation) ~Adam
				if(mAnimator != null)
				{
					mAnimator.Play("Shoot");
					if(mGrabber){ShootEnemyBullet();}
				}
				else
				{
					ShootEnemyBullet();
				}
				//Whether the timer is randomized or not `Adam
				if(mRandomShootTimer)
				{
					mShootTimer = Random.Range(mShootTimerDefault/2f, mShootTimerDefault);
				}
				else
				{
					mShootTimer = mShootTimerDefault;
				}			
			}
		}
	

		//Delete self is strangely far out of bounds for bug reasons ~Adam
		if((transform.position.y > 60f) || (transform.position.y <-70f) || (transform.position.x > 60f) || (transform.position.x < -60f) || (mAutoDeleteTimer<=0f))
		{
//			Debug.Log(transform.position.y);
//			Debug.Log(transform.position.x);
//			
//			Debug.Log(mAutoDeleteTimer.ToString());
//			
//			Debug.Log("Deleting for being offscreen");
			Destroy(this.gameObject);
		}
		if((transform.position.y > 25f) || (transform.position.y <-33f) || (transform.position.x > 20f) || (transform.position.x < -20f) || (mAutoDeleteTimer<=0f))
		{
			mAutoDeleteTimer-=Time.deltaTime;
		}
	}//END of Update()

	void LateUpdate()
	{

	}//END of LateUpdate()
	//Tells this unit to go towards the assigned swarm
	void ApproachSwarm()
	{
		//Animation Control (Mostly for blue/grabber enemy) ~Adam
		if(mAnimator != null)
		{
			mAnimator.SetBool("IsIdle", false);
			mAnimator.SetBool("IsChasing", false);
			mAnimator.SetBool("IsReturning", true);
		}


		#region Basic enemy movement was already written when I joined the project.  My main edit to this region was to make timers variable-based rather than hard-coded in order to allow variations between enemy types.
		//Variables for the direciton and distance this unit has to go to reach its SwarmGridSlot
		Vector3 toSwarm = new Vector3();
		float dist;

		//If this unit hasn't flown in a circle yet, fly towards the base of the swarm, otherwise, fly towards the appropriate GridSlot
		if (mHasLooped == false && mSwarmGrid != null)
		{
			if (mLoopPoint == null)
			{
				toSwarm = mSwarmGrid.transform.position - transform.position;
			}
			else
			{
				toSwarm = mLoopPoint.transform.position - transform.position;
			}
		}
		else if(mSwarmGridPosition != null && toSwarm != null)
		{
			toSwarm = mSwarmGridPosition.transform.position - transform.position;
		}

		//Fly in the appriate direction
		dist = toSwarm.magnitude;
		toSwarm.Normalize();
		
		transform.up += toSwarm;
		transform.up.Normalize();
		
		GetComponent<Rigidbody>().velocity = transform.up * mSpeed;



		//Transitions to other states after timer reaches 0
		//If mHasLooped is false, start flying in a circle, otherwise take place in the swarm
		if (mSwitchCoolDown <= 0.0f && mHasLooped == false)
		{
			if (mLoopPoint == null)
			{
				mCurrentAIState = AIState.FlightLooping;
				mSwitchCoolDown = mLoopTime;
			}
			else if (mLoopPoint != null && (dist<1f) )
			{
				mCurrentAIState = AIState.FlightLooping;
				mSwitchCoolDown = mLoopTime;
			}
		}

		if (mSpeed != mDefaultSpeed && dist < 2f)
		{
			mSpeed = mDefaultSpeed;
			//mSpeed = Mathf.Lerp(mSpeed, mDefaultSpeed, 0.3f);
		}


		//Take place in the swarm if timer is down and this unit has reached its destination
		else if (mSwitchCoolDown <= 0.0f && (dist < 0.5f))
		{
			mCurrentAIState = AIState.Swarming;
			mSwitchCoolDown = 1.0f;
			mAttackFrequencyTimer = mAttackFrequencyTimerDefault;
		}
		#endregion

	}//END of ApproachSwarm()

	//Tells this unit to fly in a circle
	void DoFlightLoop()
	{
		#region Basic enemy movement was already written when I joined the project.  My main edit to this region was to make movement speed, circle-tightness, and timers variable-based rather than hard-coded in order to allow variations between enemy types.

		//Set up the directional angle to fly in a circle
		mVel += new Vector2(transform.right.x*mLoopCircleTightness, transform.right.y*mLoopCircleTightness) * Time.deltaTime;
		
		mVel.Normalize();
		transform.up += new Vector3(mVel.x, mVel.y, 0);
		transform.up.Normalize();
		//Set the velocity to move in the cicle
		GetComponent<Rigidbody>().velocity = transform.up * mSpeed;

		//Figure out whether or not we are pointed at this unit's swarm grid position
		Vector3 toSpot = mSwarmGridPosition.transform.position - transform.position;
		toSpot.Normalize();
		float difference = Vector3.Dot(toSpot, transform.up);
		
		
		//Transitions to ApproachingSwarm AI state if the timer has run out and this unit is pointed towards its grid slot
		if (mSwitchCoolDown <= 0.0f && (difference > 0.97f))
		{
			mHasLooped = true;
			mCurrentAIState = AIState.ApproachingSwarm;
			mSwitchCoolDown = 0.5f;
		}
		#endregion
	}//END of DoFlightLoop()

	//The behavior for moving around on top if this unit's swarm grid slot
	void Swarm()
	{
		//Animation Control (Mostly for blue/grabber enemy) ~Adam
		if(mAnimator != null)
		{
			mAnimator.SetBool("IsIdle", true);
			mAnimator.SetBool("IsChasing", false);
			mAnimator.SetBool("IsReturning", false);
			if(mGrabber &&mShipInTow != null)
			{
				mAnimator.SetBool("IsIdle", false);
				mAnimator.SetBool("IsReturning", true);
			}
		}
		//Stop using the speed for the alternate speed for making the formation
		if(mSpeed == mFormSpeed && mFormSpeed != mDefaultSpeed)
		{
			mSpeed = mDefaultSpeed;
		}

		#region Basic enemy movement was already written by Jonathan when I joined the project.  
		//Count down to get ready to attack
		mAttackFrequencyTimer -= Time.deltaTime;
		
		transform.position = mSwarmGridPosition.transform.position;
		transform.up = mSwarmGridPosition.transform.up;

//		Vector3 toPlayer;
//		toPlayer = mPlayer.position - transform.position;
//		toPlayer.Normalize();

		#endregion

		#region from when we were doing 2 players ~Adam
		//Find the direction to the player (or the clone if it's closer) ~Adam
		Vector3 toPlayer = Vector3.down;
		if(mPlayer != null)
		{
			if(mPlayerClone != null && Vector3.Distance(transform.position,mPlayerClone.position) <= Vector3.Distance(transform.position,mPlayer.position) )
			{
				toPlayer = mPlayerClone.position - transform.position;
			}
			else
			{
				toPlayer = mPlayer.position - transform.position;
			}
		}
		else if (mPlayerClone != null)
		{
			toPlayer = mPlayerClone.position - transform.position;
		}
		toPlayer.Normalize();
		#endregion



		//Switch to attack mode if the attack timer has run out
		if (mAttackFrequencyTimer <= 0.0f && (mMinimumFirstAttackTime < Time.time))
		{
			mCurrentAIState = AIState.Attacking;
			mSwitchCoolDown = mAttackLengthTimerDefault;
		}
		mRetreating = false;

		mInvincible = false;
	}//END of Swarm()

	void AttackPlayer()
	{
		//Animation Control (Mostly for blue/grabber enemy) ~Adam
		if(mAnimator != null)
		{
			mAnimator.SetBool("IsIdle", false);
			mAnimator.SetBool("IsChasing", true);
			mAnimator.SetBool("IsReturning", false);
			if(mGrabber &&mShipInTow != null)
			{
				mAnimator.SetBool("IsChasing", false);
				mAnimator.SetBool("IsReturning", true);
			}}

		#region From when we were doing 2 player mode ~Adam
		//Find the direction to the player (or the clone if it's closer) ~Adam
		Vector3 toPlayer;

		if(mPlayer != null){

			if(mPlayerClone != null && Vector3.Distance(transform.position,mPlayerClone.position) <= Vector3.Distance(transform.position,mPlayer.position) )
			{
				toPlayer = mPlayerClone.position - transform.position;
			}
			else
			{
				toPlayer = mPlayer.position - transform.position;
			}
		}else{

			toPlayer = mPlayerClone.position - transform.position;
		}

			toPlayer.Normalize();
		
		#endregion

		#region Basic enemy movement was already written by Jonathan when I joined the project.  Before I joined, enemies would move toward the player for a few seconds, then move back to the swarm without actually affecting the player.

//		Vector3 toPlayer = mPlayer.position - transform.position;
//		toPlayer.Normalize();
		Vector3 vel = transform.gameObject.GetComponent<Rigidbody>().velocity;
		
		vel += toPlayer;
		vel.Normalize();
		vel *= mSpeed;

		
		transform.gameObject.GetComponent<Rigidbody>().velocity = vel;
		#endregion

		//Make the Grabber enemies invincible while attacking
		if(mGrabber && !mPlayer.GetComponent<PlayerShipController>().mShipRecovered && !mPlayer.GetComponent<PlayerShipController>().mShipStolen )
		{
			mInvincible = true;
		}
		else if (mGrabber && (mPlayer.GetComponent<PlayerShipController>().mShipRecovered || mPlayer.GetComponent<PlayerShipController>().mShipStolen) )
		{
			mInvincible = false;
			GetComponentInChildren<Renderer>().material.color = Color.white;
		}

		//Return to the swarm, possibly stopping to fly in a loop along the way
		//Triggers either once the enemy has been attacking for its full attack time, or if the player is sent into their tempory invulnerable state after being hit
		//Grabbers will still go after invincible players if they are still capable of stealing a ship
		if (mSwitchCoolDown <= 0.0f || (mScoreManager.GetComponent<ScoreManager>().mPlayerSafeTime > 0 ) )
		{
			if( !mGrabber 
			   || (mGrabber && (mPlayer.GetComponent<PlayerShipController>().mShipRecovered || mPlayer.GetComponent<PlayerShipController>().mShipStolen)) 
			   		|| (mGrabber && mSwitchCoolDown <= 0.0f) )
			{
				if (mPostAttackLoop && mAttackLengthTimerDefault > 0.0f)
				{
					//For making all green enemies retreatloop one way, teal the other way, and blue alternate
					if(mGrabber)
					{
						mLoopCircleTightness *= -1f;
					}
					else if(mRetreatClockwise && mLoopCircleTightness < 0)
					{
						mLoopCircleTightness *= -1f;
					}
					else if(!mRetreatClockwise && mLoopCircleTightness > 0)
					{
						mLoopCircleTightness *= -1f;
					}
					mHasLooped = (Random.value < 0.5);
					mSwitchCoolDown = Random.Range(0.5f, 3.0f);


				}
				else
				{
					mSwitchCoolDown = 0.5f;
				}
				mRetreating = true;
				mCurrentAIState = AIState.ApproachingSwarm;
			}
		}


	}//END of AttackPlayer()


	public void ShootEnemyBullet()
	{
		GameObject enemyBullet;
		enemyBullet = Instantiate(mEnemyBullet, transform.position, Quaternion.identity) as GameObject;
	}//End of ShootEnemyBullet()

	void OnCollisionEnter(Collision other)
	{

//		if (other.gameObject.GetComponent<BulletLeft>() != null || other.gameObject.GetComponent<BulletRight>() != null) { //If collide w/ Left/Right Bullet
//
//			if(!mInvincible)
//			{
//				if(Random.value <= LRBulletChance)
//				{
//					EnemyShipDie();
//				}
//				else
//				{
//					Instantiate(bulletExplosion, transform.position, Quaternion.identity);
//				}
//			}
//
//			Destroy(other.gameObject);
//		}

		//Get destroyed when colliding with a bullet
		if (other.gameObject.GetComponent<PlayerBulletController>() != null)
		{

			if(!mInvincible)
			{
				//decide if middle or side bullet
				//If collided with side bullet
				if(other.gameObject.GetComponent<PlayerBulletController>().mSideBullet)
				{
					if(Random.value <= LRBulletChance)
					{
						mKillerNumber = other.gameObject.GetComponent<PlayerBulletController>().mPlayerBulletNumber;
						EnemyShipDie();
					}
					else
					{
						Instantiate(bulletExplosion, transform.position, Quaternion.identity);
					}
				}
				//If collided with central bullet
				else
				{
					if(Random.value <= middleBulletChance)
					{
						mKillerNumber = other.gameObject.GetComponent<PlayerBulletController>().mPlayerBulletNumber;
						EnemyShipDie();
					}
					else
					{
						Instantiate(bulletExplosion, transform.position, Quaternion.identity);
					}
				}

				Destroy(other.gameObject);
			}
			else
			{
				if(Random.value < 0.5)
				{
					other.gameObject.GetComponent<Rigidbody>().AddForce(Random.Range(30,50), 0, 0);
				}
				else
				{
					other.gameObject.GetComponent<Rigidbody>().AddForce(Random.Range(-50,-30), 0, 0);

				}
			}
			 
		}

		//Make the player lose a life on contact with an enemy
		if (other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			//mScoreManager.HalfScore();
			if(!(mGrabber && !mPlayer.GetComponent<PlayerShipController>().mShipStolen) || (mGrabber && mPlayerClone != null) )
			{
				mScoreManager.LoseALife();
			}
			//A debug log for tracking which enemy hit the player
			//Debug.Log("Player was hit by the enemy at grid slot " + mSwarmGridPosition.name +" in grid " + mSwarmGrid.name + "!!!");

			//If the enemy that hit the player is a Grabber, and it's not 2-player mode, and there is not currently a stolen ship in play, steal a ship from the player
			else if(mGrabber && mPlayer != null && !mPlayer.GetComponent<PlayerShipController>().mShipStolen  &&!mPlayer.GetComponent<PlayerShipController>().mShipRecovered && mTowPoint != null)
			{
				mPlayer.GetComponent<PlayerShipController>().mShipStolen = true;
				GameObject capturedShipToSpawn;
				capturedShipToSpawn = Instantiate(mCapturedShip, mTowPoint.position, Quaternion.identity) as GameObject;
				capturedShipToSpawn.GetComponent<CapturedShip>().mGrabbingEnemy = this;
				mShipInTow = capturedShipToSpawn;

				//Stop attacking and return to swarm
				if (mPostAttackLoop && mAttackLengthTimerDefault > 0.0f)
				{
					mHasLooped = (Random.value < 0.5);
					mSwitchCoolDown = Random.Range(0.5f, 3.0f);
					
				}
				else
				{
					mSwitchCoolDown = 0.5f;
				}
				mCurrentAIState = AIState.ApproachingSwarm;
			}


		}

		//Make Player-2 lose a life on contact with an enemy
		if (other.gameObject.GetComponent<PlayerTwoShipController>() != null)
		{
			mScoreManager.LosePlayerTwoLife();
		}
		#region from when we were doing the clone/twin-stick ship
//		//Kill a player clone ship on contact
//		if (other.gameObject.GetComponent<PlayerShipCloneController>() != null)
//		{
//			Debug.Log("Hit a clone ship");
//			other.gameObject.GetComponent<PlayerShipCloneController>().CloneShipDie();
//		}
		#endregion

	}//END of OnCollisionEnter()

	public void EnemyShipDie()
	{
		if(mShipInTow != null)
		{
			mShipInTow.GetComponent<CapturedShip>().mInTow = false;
			mShipInTow = null;
		}

		if (Random.value * 100 <= secondaryExplosionChance) 
		{

			Instantiate(secondaryExplosion, transform.position, Quaternion.identity);
			//For firing a shot upon death (i.e. red enemies) ~Adam
			if(mDeathShooter && Application.loadedLevel != 0)
			{
				ShootEnemyBullet();
			}

		}
		else 
		{
			GameObject deathParticles = Instantiate(mDeathEffect, transform.position + new Vector3(0f, 0f, -1f), Quaternion.identity) as GameObject;
		}

		//Shake the camera on death
		if (Camera.main.GetComponent<CameraShaker>() != null)
		{
		//	Camera.main.GetComponent<CameraShaker>().ShakeCameraEnemy();
		//
			if(mGrabber){
		//
				Camera.main.GetComponent<CameraShaker> ().RumbleController(.5f, .5f);
			}
		}
//		if(Random.value < 0.05f)
//		{
//			deathParticles.GetComponent<ParticleSystem>().startLifetime += 0.05f;
//			if(deathParticles.GetComponent<AudioSource>() != null)
//			{
//				deathParticles.GetComponent<AudioSource>().volume += 0.05f;
//			}
//		}
		mSwarmGridPosition.GetComponent<SwarmGridSlot>().mOccupied = false;
		if(mScoreManager != null)
		{
			mScoreManager.AdjustScore(mPointValue, mKillerNumber <= 1);
		}
		
		if(mDeathRequired)
		{
			mKillCounter.UpdateKillCounter();
		}
		if(FindObjectOfType<SlowTimeController>()!= null)
		{
			FindObjectOfType<SlowTimeController>().SlowDownTime(0.8f,1f);
		}
		Destroy(gameObject);
	}//END of EnemyShipDie()
}
