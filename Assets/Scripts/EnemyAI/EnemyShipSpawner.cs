using UnityEngine;
using System.Collections;

public class EnemyShipSpawner : MonoBehaviour 
{
	//Set this to true if we want this spawner to do more than one wave of enemies
	public bool mSpawnMultipleWaves = false;
	public float mWaveRefreshTimeMin = 30f;
	public float mWaveRefreshTimeMax = 60f;


	[SerializeField] private bool mLimitedWaveRespawns = false;
	[SerializeField] private int mMaxWaveRespawns = 3;
	[SerializeField] private int mRespawnWaveCount = 0;
	[SerializeField] private bool mSpawnsRequiredEnemies = true;

	//The SwarmGrid this spawner is sending enemies to
	public SwarmGrid mTargetSwarmGrid;
	//Whether or not to have the spawned enemies fly to and loop around a specified point
	[SerializeField] private bool mUsingLoopPoint = false;
	//The point that the spawned enemies will loop around if the above is true
	[SerializeField] private GameObject mLoopPoint;


	//*****Values for overriding spawned enemy properties******

	//Whether or not to overide enemy flight speed
	[SerializeField] private bool mOverridingFlightSpeed;
	//Overriden flight speed value
	[SerializeField] private float mOverriddenFlightSpeed = 15f;
	[SerializeField] private float mOverriddenFormSpeed = 15f;
	//Whether or not the spawned enemies skip the looping step on their way to the swarm for the first time
	[SerializeField] private bool mDontDoFirstLoop = false;
	//Whether or not we override how tight a loop the spawned enemies make
	[SerializeField] private bool mOverridingLoopTightness = false;
	//How tight to make the overriden loops
	[SerializeField] private float mLoopOverrideTightnessAmount = 1f;
	//Whther or not to ovveride how long is spent making loops
	[SerializeField] private bool mOverridingLoopTime = false;
	//How long to make the overriden loops last
	[SerializeField] private float mLoopOverrideTimeAmount = 0.5f;
	//Whther or not to ovveride how long is spent in the swarm between making attacks
	[SerializeField] private bool mOverridingAttackFrequency = false;
	//How long to make the overriden loops last
	[SerializeField] private float mAttackFrequencyOverrideTimeAmount = 10f;
	//Whther or not to ovveride how long is spent diving down when making attacks
	[SerializeField] private bool mOverridingAttackLength = false;
	//How long to make the overriden loops last
	[SerializeField] private float mAttackLengthOverrideTimeAmount = 10f;
	//How long to make the overriden loops last
	[SerializeField] private bool mOverridingShotFrequency = false;
	[SerializeField] private float mShootingFrequencyOverrideTimeAmount = 2f;


	[SerializeField] private float mMininumFirstAttackTimeOverride = 0f;

	//Whether or not to override enemy lifespan
	[SerializeField] private bool mOverridingLifespan = false;
	[SerializeField] private float mLifespanOverrideTimeAmount = 10f;

	//*****End of spawned enemy override variables*****

	//Are we currently spawning enemies?
	public bool mSpawning = true;
	//The speed at which enemies are spawned
	public float mDefaultSpawnInterval = 1f;
	private float mSpawnInterval;
	//How long to wait before enemies are first spawned
	public float mWaveStartTime = 2f;
	//Which type of enemy to spawn
	public GameObject mEnemyToSpawn;
	//How many enemies to spawn per wave
	[SerializeField] private int mMaxEnemySpawn = 5;
	//How many enemies we've spawned so far
	[HideInInspector] public int mSpawnCounter = 0;
	
	// Use this for initialization
	void Start () 
	{
		mSpawnInterval = mDefaultSpawnInterval;
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		mWaveStartTime -= Time.deltaTime;
		
		if (mWaveStartTime <= 0.0f && mSpawning)
		{
			SpawnEnemies();
		}
	}//END of Update()

	public void SpawnEnemies()
	{
		mSpawnInterval -= Time.deltaTime;
		
		if (mSpawnInterval <= 0.0f)
		{
			GameObject NewEnemy = Instantiate(mEnemyToSpawn, transform.position, Quaternion.identity) as GameObject;
			NewEnemy.GetComponent<EnemyShipAI>().mSwarmGrid = mTargetSwarmGrid;

			NewEnemy.GetComponent<EnemyShipAI>().mDeathRequired = mSpawnsRequiredEnemies;

			NewEnemy.GetComponent<EnemyShipAI>().mMinimumFirstAttackTime = mMininumFirstAttackTimeOverride+Time.time;

			//If statement for overriding flight speed
			if(mOverridingFlightSpeed)
			{
				NewEnemy.GetComponent<EnemyShipAI>().mSpeed = mOverriddenFlightSpeed;
			}
			NewEnemy.GetComponent<EnemyShipAI>().mFormSpeed = mOverriddenFormSpeed;

			//If statements for overriding the Loop behavior for the spawned enemies
			if(mDontDoFirstLoop)
			{
				NewEnemy.GetComponent<EnemyShipAI>().mHasLooped = true;
			}
			if (mUsingLoopPoint && mLoopPoint != null)
			{
				NewEnemy.GetComponent<EnemyShipAI>().mLoopPoint = this.mLoopPoint;
			}
			if(mOverridingLoopTightness)
			{
				NewEnemy.GetComponent<EnemyShipAI>().mLoopCircleTightness = mLoopOverrideTightnessAmount;
			}
			if (mOverridingLoopTime)
			{
				NewEnemy.GetComponent<EnemyShipAI>().mLoopTime = mLoopOverrideTimeAmount;
			}

			//If statements for overriding attack behavior
			if(mOverridingAttackFrequency)
			{
				NewEnemy.GetComponent<EnemyShipAI>().mAttackFrequencyTimerDefault = mAttackFrequencyOverrideTimeAmount;
			}
			if(mOverridingAttackLength)
			{
				NewEnemy.GetComponent<EnemyShipAI>().mAttackLengthTimerDefault = mAttackLengthOverrideTimeAmount;
			}
			if(mOverridingShotFrequency)
			{
				NewEnemy.GetComponent<EnemyShipAI>().mShootTimerDefault = mShootingFrequencyOverrideTimeAmount;
			}

			//If statements for overriding lifespan
			if(mOverridingLifespan)
			{
				NewEnemy.GetComponent<EnemyShipAI>().mLimitedLifespan = true;
				NewEnemy.GetComponent<EnemyShipAI>().mLifespanLength = mLifespanOverrideTimeAmount;
			}

			//Delete the enemy if it couldn't find a spot in the swarm
			if (NewEnemy.GetComponent<EnemyShipAI>().mSwarmGrid == null)
			{
				Destroy(NewEnemy.gameObject);
			}
		
			mSpawnCounter++;
			mSpawnInterval += mDefaultSpawnInterval;
			
			if (mSpawnCounter >= mMaxEnemySpawn)
			{
				mSpawnCounter = 0;

				if (mSpawnMultipleWaves && (!mLimitedWaveRespawns || (mLimitedWaveRespawns && mRespawnWaveCount < mMaxWaveRespawns) ) )
				{
					//Set the time to spawn a new wave to a random amount
					mWaveStartTime = Random.Range(mWaveRefreshTimeMin, mWaveRefreshTimeMax);
					mSpawning = true;
					mSpawnCounter = 0;
					mRespawnWaveCount++;
				}
				else
				{
					mSpawning = false;

				}
			}
		}
	}
}
