using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FSCBossController : MonoBehaviour 
{
	//The timer that determines what phase the boss is in ~Adam
		// <0 Spawnning, invincible
		// 0-10 Firing, invincible minions circling
		// 10-15 hittable, minions attacking
		// 15-20 hitable, spawning, minions circling, go back to 0 when 20 is hit
	[SerializeField] private float mBehaviorTimer = -5f;

	public int mBossHP = 300;

	//The particle effect fiels where we'll be centering the enemy spawners ~Adam
	[SerializeField] private GameObject mHornSpawnPartLeft;
	[SerializeField] private GameObject mHornSpawnPartRight;
	//The enemy spawners for the minions ~Adam
	[SerializeField] private EnemyShipSpawner mHornSpawnerLeft;
	[SerializeField] private EnemyShipSpawner mHornSpawnerRight;
	

	//The main particle beam laser ~Adam
	[SerializeField] private GameObject mMouthLaser;


	[SerializeField] private SpriteRenderer mEyeGraphic;

	GameObject mPlayerShip;
	ScoreManager mScoreManager;
	[SerializeField] GameObject mScreenFader;
	[SerializeField] private GameObject mDeathExplosion;
	[SerializeField] private GameObject mFinalExplosion;
	[SerializeField] private GameObject mGameHUD;

	[SerializeField] private Animator mAnimator;
	bool mFiring = false;
	float mEndGameTimer = 0f;

	// Use this for initialization
	void Start () 
	{
		mPlayerShip = FindObjectOfType<PlayerShipController> ().gameObject;
		mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		mScreenFader.GetComponent<Renderer>().material.color = Color.clear;
//		mGameHUD = ;
		mEyeGraphic.color = Color.clear;
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Make sure we always have a ScoreManager ~Adam
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		}
		//Make sure we always have a Player Ship ~Adam
		if (mPlayerShip == null) 
		{
			mPlayerShip = FindObjectOfType<PlayerShipController> ().gameObject;
		}
		else
		{
			mBehaviorTimer += Time.deltaTime;
			//If the boss is still alive, do stuff ~Adam
			if(mBossHP >0)
			{
				mAnimator.SetBool("Firing", mFiring);
				GetComponent<Collider>().isTrigger = !mFiring;

				mEyeGraphic.color = Color.Lerp(mEyeGraphic.color, Color.clear, 0.01f);
				GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.white, 0.01f);
				//Go back to start of behavior loop ~Adam
				if(mBehaviorTimer > 20f)
				{
					mBehaviorTimer = 0f;
				}
				// 15-20 hitable, spawning, minions circling, go back to 0 when 20 is hit ~Adam
				else if (mBehaviorTimer > 15f)
				{
					#region Spawning minions ~Adam
					SpawnMinions();
					#endregion
					//Make all the minions constantly circle ~Adam
					MinionCircle();
				
				}
				// 10-15 hittable, minions attacking ~Adam
				else if (mBehaviorTimer > 10f)
				{
					//Spawn minions ~Adam
					SpawnMinions();
					//Make all the minions constantly attack ~Adam
					foreach(EnemyShipAI minion in FindObjectsOfType<EnemyShipAI>())
					{
						minion.mCurrentAIState = EnemyShipAI.AIState.Attacking;
						minion.gameObject.transform.up = transform.up;
					}

					//Stop firing the laser ~Adam
					mFiring = false;
					mMouthLaser.GetComponent<ParticleSystem>().Stop();
				}
				// 0-10 Firing, invincible minions circling
				else if (mBehaviorTimer >= 0f)
				{
					#region Stop the spawning of minions ~Adam

					StopMinionSpawning();
					#endregion

					//Make all the minions constantly circle ~Adam
					MinionCircle ();

					//Fire the Laser ~Adam
					mFiring = true;
					GetComponent<Collider>().isTrigger = false;
					mMouthLaser.transform.LookAt(mPlayerShip.transform.position);

					if(!mMouthLaser.GetComponent<ParticleSystem>().isPlaying)
					{
						mMouthLaser.GetComponent<ParticleSystem>().Play();
					}

				}
				// <0 Spawnning, invincible ~Adam
				else if (mBehaviorTimer < 0f)
				{
					#region Spawning minions ~Adam
					SpawnMinions();
					#endregion
					//Close the eye and become invincible
					mFiring = true;
					//Make all the minions constantly circle ~Adam
					MinionCircle ();
					//Stop firing the laser ~Adam
					mMouthLaser.GetComponent<ParticleSystem>().Stop();
				}
			}
			//Take care of the boss dying ~Adam
			else
			{
				#region Stop the spawning of minions ~Adam
				//StopMinionSpawning();
				
				#endregion

				//Kill the remaining minions ~Adam
				if(FindObjectsOfType<EnemyShipAI>().Length >0)
				{
					foreach(EnemyShipAI minion in FindObjectsOfType<EnemyShipAI>())
					{
						minion.EnemyShipDie();
					}
				}
				//Stop firing the laser ~Adam
				mFiring = false;
				mMouthLaser.GetComponent<ParticleSystem>().Stop();




				FindObjectOfType<ScoreManager>().enabled = false;
				FindObjectOfType<LevelKillCounter>().enabled = false;
				FindObjectOfType<PauseManager>().enabled = false;
				mGameHUD.SetActive(false);
				mEndGameTimer += Time.deltaTime;

				Instantiate(mDeathExplosion, transform.position+(new Vector3(Random.Range(-5f,5f),Random.Range(-5f,5f),-0.5f)), Quaternion.identity);
				
				mScreenFader.GetComponent<Renderer>().enabled = true;
				mScreenFader.GetComponent<Renderer>().material.color = Color.Lerp(mScreenFader.GetComponent<Renderer>().material.color, new Color(0,0,0,1f),0.01f);
				AudioListener.volume -=  0.001f;
				if(mEndGameTimer >= 2.8f)
				{
					Instantiate(mDeathExplosion, transform.position+(new Vector3(0f,0f,-0.5f)), Quaternion.identity);
					AudioListener.volume -=  0.01f;
					
				}
				if(mEndGameTimer >= 6f)
				{
					if(FindObjectOfType<PlayerShipController>() != null)
					{
						Destroy(FindObjectOfType<PlayerShipController>().gameObject);
					}
					if (FindObjectOfType<PlayerTwoShipController>() != null)
					{
						Destroy(FindObjectOfType<PlayerTwoShipController>().gameObject);
					}
					Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
					//Destroy(FindObjectOfType<ScoreManager>().gameObject);
					mScoreManager.mLevelInfoText.text = "Thank you for playing!";
					Application.LoadLevel("Credits");
				}
			}
		}

	}//END of Update()


	void OnTriggerEnter(Collider other)
	{
		//Get hit by bullets
		if(other.gameObject.GetComponent<PlayerBulletController>() != null)
		{
			
			if(!other.gameObject.GetComponent<PlayerBulletController>().mSideBullet && !mFiring)
			{
				Destroy (other.gameObject);
				mBossHP--;
				mEyeGraphic.color = Color.Lerp(mEyeGraphic.color, Color.white, 0.9f);
				GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.red, 0.9f);

			}
			
		}
		//Kill Player when touched
		if(other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			mScoreManager.LoseALife();
		}
	}//END of OnCollisionEnter()

	//For stoping the spawning of minions
	void StopMinionSpawning()
	{
		foreach(ParticleSystem portalPart in mHornSpawnPartLeft.GetComponentsInChildren<ParticleSystem>())
		{
			if(portalPart.isPlaying)
			{
				portalPart.Stop();
			}
		}
		foreach(ParticleSystem portalPart in mHornSpawnPartRight.GetComponentsInChildren<ParticleSystem>())
		{
			if(portalPart.isPlaying)
			{
				portalPart.Stop();
			}
		}
		mHornSpawnerLeft.enabled = false;
		mHornSpawnerRight.enabled = false;
	}//END of StopMinionSpawning()

	//For Spawning Minions()
	void SpawnMinions()
	{
		foreach(ParticleSystem portalPart in mHornSpawnPartLeft.GetComponentsInChildren<ParticleSystem>())
		{
			if(!portalPart.isPlaying)
			{
				portalPart.Play();
			}
		}
		foreach(ParticleSystem portalPart in mHornSpawnPartRight.GetComponentsInChildren<ParticleSystem>())
		{
			if(!portalPart.isPlaying)
			{
				portalPart.Play();
			}
		}
		mHornSpawnerLeft.enabled = true;
		mHornSpawnerRight.enabled = true;
		if(FindObjectsOfType<EnemyShipAI>().Length < 200)
		{
			mHornSpawnerLeft.mSpawnCounter = 0;
			mHornSpawnerRight.mSpawnCounter = 0;
		}
	}//END of SpawnMinions()

	//For forcing minions to circle
	void MinionCircle()
	{
		foreach(EnemyShipAI minion in FindObjectsOfType<EnemyShipAI>())
		{
			if(minion.mCurrentAIState == EnemyShipAI.AIState.Attacking || minion.mCurrentAIState == EnemyShipAI.AIState.Swarming)
			{
				minion.mCurrentAIState = EnemyShipAI.AIState.ApproachingSwarm;
			}
			if(minion.mCurrentAIState == EnemyShipAI.AIState.ApproachingSwarm && Vector3.Distance(minion.transform.position, minion.mLoopPoint.transform.position) <1.1f)
			{
				minion.transform.up = transform.up;
				minion.mVel = new Vector2(0, -1);
				minion.mCurrentAIState = EnemyShipAI.AIState.FlightLooping;
			}
			minion.mHasLooped = false;
		}
	}
}
