using UnityEngine;
using System.Collections;

public class SkullBossController : MonoBehaviour 
{
	[SerializeField] private float mHornBeamTime = 10f;
	[SerializeField] private float mHornBeamAltTime = 3f;
	[SerializeField] private float mEyeBeamFireTimerMin = 3f;
	[SerializeField] private float mEyeBeamFireTimerMax = 7f;
	[SerializeField] private float mEyeBeamFireLength = 1f;


	ScoreManager mScoreManager;
	[SerializeField] private SpriteRenderer mBodyGraphic;
	[SerializeField] private SpriteRenderer mLeftHornGraphic;
	[SerializeField] private SpriteRenderer mRightHornGraphic;
	[SerializeField] private Sprite mFullBodySprite;
	[SerializeField] private Sprite mLeftHornMissingSprite;
	[SerializeField] private Sprite mRightHornMissingSprite;
	[SerializeField] private Sprite mNoHornsSprite;
	public bool mRightHornAlive = true;
	public bool mLeftHornAlive = true;
	[SerializeField] private SkullBossShield mSkullShield;
	[SerializeField] private SkullBossHorn mLeftHorn;
	[SerializeField] private SkullBossHorn mRightHorn;
	int mBossHP = 200;
	
	//For moving the eyeball around `Adam
	[SerializeField] private GameObject mEyeball;
	PlayerShipController mPlayerShip;
	
	//For firing projectiles `Adam
	float mHornFireTimer = 5f;
	float mEyeFireTimer = 5f;
	bool mBeamFireAlt = false;
	[SerializeField] private GameObject mEyeBeam;
	[SerializeField] private GameObject mEyeBeamAlt;
	[SerializeField] private GameObject mEyeBeamBurst;
	Vector3 mBurstBeamDir = new Vector3(0f,-1f,0f);

	
	//For ending the game `Adam
	float mEndGameTimer = 0f;
	bool mGameOver = false;
	[SerializeField] private GameObject mDeathExplosion;
	[SerializeField] private GameObject mFinalExplosion;
	[SerializeField] private GameObject mGameHUD;
	[SerializeField] private GameObject mScreenFader;
	
	
	//For Moving the Boss Around `Adam
	public bool mMovingToCenter = false;
	Vector3 mDriftPoint = new Vector3(0f,0f,-2f);
	float mDriftTimer = 0f;

	//For using the particle-based Eye Beams
	[SerializeField] private GameObject mEyeBeamPartHolder;
	[SerializeField] private GameObject mEyeBeamPartSmall;
	[SerializeField] private GameObject mEyeBeamPartFinal;


	// Use this for initialization
	void Start () 
	{
		mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		mPlayerShip = FindObjectOfType<PlayerShipController>() as PlayerShipController;
		mScreenFader.GetComponent<Renderer>().material.color = new Color(0f,0f,0f,0f);
		
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		mHornFireTimer += Time.deltaTime;
		mEyeFireTimer += Time.deltaTime;

		mEyeBeamPartHolder.transform.LookAt(mPlayerShip.transform.position);

		//Make sure we always have a ScoreManager ~Adam
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		}
		
		//Do behavior to end the game ~Adam
		if(mGameOver)
		{
			mEyeBeamPartFinal.GetComponent<ParticleSystem>().Stop();

			FindObjectOfType<ScoreManager>().enabled = false;
			FindObjectOfType<LevelKillCounter>().enabled = false;
		//	FindObjectOfType<PauseManager>().enabled = false;
			mGameHUD.SetActive(false);
			mEndGameTimer += Time.deltaTime;
			mEyeball.GetComponent<SpriteRenderer>().enabled = false;
			
			Instantiate(mDeathExplosion, transform.position+(new Vector3(Random.Range(-5f,5f),Random.Range(-5f,5f),-0.5f)), Quaternion.identity);

			//Fade out the screen ~Adam
			mScreenFader.GetComponent<Renderer>().enabled = true;
			mScreenFader.GetComponent<Renderer>().material.color = Color.Lerp(mScreenFader.GetComponent<Renderer>().material.color, new Color(0,0,0,1f),0.01f);

			//Fade out the audio ~Adam
			if(FindObjectOfType<BGMVolumeController>() != null)
			{
				FindObjectOfType<BGMVolumeController>().GetComponent<AudioSource>().ignoreListenerVolume = false;
				FindObjectOfType<BGMVolumeController>().enabled = false;
			}
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
				Application.LoadLevel("Credits");
			}
		}
		//Do regular behavior
		else
		{
			//For moving the boss on-screen ~Adam
			if(mMovingToCenter)
			{
				//Shake the camera as the boss enters ~Adam
				if (Camera.main.GetComponent<CameraShaker>() != null)
				{
					Camera.main.GetComponent<CameraShaker>().ShakeCameraDeath();
				}
				transform.position = Vector3.Lerp(transform.position, new Vector3(0f,0f,-2f), 0.01f);
				
				if(Vector3.Distance(transform.position, new Vector3(0f,0f,-2f)) < 2f)
				{
					transform.position = new Vector3(0f,0f,-2f);
					mMovingToCenter = false;
				}
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, mDriftPoint, 0.01f);
				mDriftTimer+= Time.deltaTime;
				if(mDriftTimer > 5f)
				{
					mDriftTimer = 0f;
					mDriftPoint = new Vector3(Random.Range (-10f,10f),Random.Range (-20f,15f), -2f);
				}
			}
			
			//For deciding which sprite to use
			if(mLeftHornAlive && mRightHornAlive)
			{
				mBodyGraphic.sprite = mFullBodySprite;
			}
			else if(!mLeftHornAlive && mRightHornAlive)
			{
				mBodyGraphic.sprite = mLeftHornMissingSprite;
				mLeftHornGraphic.enabled = false;
			}
			else if(mLeftHornAlive && !mRightHornAlive)
			{
				mBodyGraphic.sprite = mRightHornMissingSprite;
				mRightHornGraphic.enabled = false;
			}
			else if(!mLeftHornAlive && !mRightHornAlive)
			{
				mBodyGraphic.sprite = mNoHornsSprite;
				mSkullShield.mShieldDown = true;
				mLeftHornGraphic.enabled = false;
				mRightHornGraphic.enabled = false;
			}
			
			//Fire the Horn Beams
			if(mHornFireTimer >= 10f)
			{
				if(!mBeamFireAlt)
				{
					if(mRightHorn != null)
					{
						mRightHorn.FireHornBeamAlt();
					}
					if(mLeftHorn != null)
					{
						mLeftHorn.FireHornBeam();
					}
					
					if(mHornFireTimer >= 10f+mHornBeamTime)
					{
						mHornFireTimer = 10f;
						mBeamFireAlt = !mBeamFireAlt;
					}
				}
				else
				{
					if(mRightHorn != null)
					{
						mRightHorn.FireHornBeam();
					}
					if(mLeftHorn != null)
					{
						mLeftHorn.FireHornBeamAlt();
					}
					
					if(mHornFireTimer >= 10f+mHornBeamAltTime)
					{
						mHornFireTimer = 10f;
						mBeamFireAlt = !mBeamFireAlt;
					}
				}
			}//End of horn beam firing
			//Firing Eye Beam
			if(mEyeFireTimer >= 10f || !mLeftHornAlive && !mRightHornAlive)
			{
				FireEyeBeam();
				if(mEyeFireTimer >= 10f + mEyeBeamFireLength)
				{
					mEyeFireTimer = Random.Range(10f-mEyeBeamFireTimerMax,10f-mEyeBeamFireTimerMin);
					if(!mLeftHornAlive && !mRightHornAlive)
					{
						mEyeFireTimer = 10f;
					}
				}
			}
			else
			{
				mEyeBeamPartFinal.GetComponent<ParticleSystem>().Stop();
				mEyeBeamPartSmall.GetComponent<ParticleSystem>().Stop();
			}
			//Die at 0 HP
			if(mBossHP <= 0)
			{
				mGameOver = true;
			}
		}
		//For flashing the sprite color when damage is taken `Adam
		mBodyGraphic.color = Color.Lerp(mBodyGraphic.color, Color.white, 0.1f);
		
		//Moving the eyeball around `Adam
		//Eye X position
		if(Mathf.Abs(mPlayerShip.transform.position.x - transform.position.x) > 5f)
		{
			if(mPlayerShip.transform.position.x > transform.position.x)
			{
				mEyeball.transform.localPosition = (new Vector3(1f,mEyeball.transform.localPosition.y,-0.2f));
			}
			else
			{
				mEyeball.transform.localPosition = (new Vector3(-1f,mEyeball.transform.localPosition.y,-0.2f));
			}
			
		}
		else
		{
			mEyeball.transform.localPosition = (new Vector3(0f,mEyeball.transform.localPosition.y,-0.2f));
		}
		//Eye Y position
		if(Mathf.Abs(mPlayerShip.transform.position.y - transform.position.y) > 5f)
		{
			if(mPlayerShip.transform.position.y > transform.position.y)
			{
				mEyeball.transform.localPosition = (new Vector3(mEyeball.transform.localPosition.x, 2f,-0.2f));
			}
			else
			{
				mEyeball.transform.localPosition = (new Vector3(mEyeball.transform.localPosition.x, 0f,-0.2f));
			}
		}
		else
		{
			mEyeball.transform.localPosition = (new Vector3(mEyeball.transform.localPosition.x, 1f,-0.2f));
		}
	}//END of Update()
	
	
	void OnCollisionEnter(Collision other)
	{
		//Get hit by bullets
		if(other.gameObject.GetComponent<PlayerBulletController>() != null)
		{

			if(!other.gameObject.GetComponent<PlayerBulletController>().mSideBullet)
			{
				if(mSkullShield.mShieldDown)
				{
					Destroy (other.gameObject);
					mBossHP--;
					mBodyGraphic.color = Color.Lerp(mBodyGraphic.color, Color.red, 0.9f);
				}
				else
				{
					Destroy (other.gameObject);
					for(int i = 0; i < 8; i++)
					{
						GameObject hornBeam = Instantiate(mEyeBeamBurst, mEyeball.transform.position+new Vector3(0f,0f,0.05f), Quaternion.identity) as GameObject;
						hornBeam.GetComponent<EnemyBulletController>().mFireDir = mBurstBeamDir;
						mBurstBeamDir = Quaternion.AngleAxis(45f,Vector3.forward)*mBurstBeamDir;
					}
					if(GetComponent<AudioSource>() != null && !GetComponent<AudioSource>().isPlaying)
					{
						GetComponent<AudioSource>().Play();
					}
				}
			}

		}
		//Kill Player when touched
		if(other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			mScoreManager.LoseALife();
		}
	}//END of OnCollsionEnter()
	
	void FireEyeBeam()
	{
		if(mLeftHornAlive || mRightHornAlive)
		{
//			Instantiate(mEyeBeam, mEyeball.transform.position+new Vector3(0f,0f,0.05f), Quaternion.identity);
			if(!mEyeBeamPartSmall.GetComponent<ParticleSystem>().isPlaying)
			{
				mEyeBeamPartSmall.GetComponent<ParticleSystem>().Play();
			}
		}
		else
		{
//			Instantiate(mEyeBeamAlt, mEyeball.transform.position+new Vector3(0f,0f,0.05f), Quaternion.identity);
			if(!mEyeBeamPartFinal.GetComponent<ParticleSystem>().isPlaying)
			{
				mEyeBeamPartFinal.GetComponent<ParticleSystem>().Play();
			}
			mEyeBeamPartSmall.GetComponent<ParticleSystem>().Stop();
		}
	}//ENd of FireEyeBeam()
}
