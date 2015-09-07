using UnityEngine;
using System.Collections;

public class SkullBossHorn : MonoBehaviour 
{

	[SerializeField] private float mHornBeamRotationSpeed = 1f;

	ScoreManager mScoreManager;
	private int mHornHP = 200;
	public bool mRightHorn; //True for the right horn, false for the left horn (boss's right and left, not the player's) `Adam
	[SerializeField] private SkullBossController mBossBody;
	[SerializeField] private SpriteRenderer mHornSprite;
	[SerializeField] private GameObject mHornDeathParticles;
	[SerializeField] private GameObject mHornBeam;
	[SerializeField] private Transform mBeamSource;
	[SerializeField] private GameObject mHornParticleBeam;

	[SerializeField] private Vector2 mBulletFireDir = new Vector3(0f,-1f,0f);

	// Use this for initialization
	void Start () 
	{
		mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Make sure we always have a ScoreManager ~Adam
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>() as ScoreManager;
		}
		mHornSprite.color = Color.Lerp(mHornSprite.color, Color.white, 0.1f);
		if(mHornHP <= 0)
		{
			DestroyHorn();
		}
	}//END of Update()

	void OnCollisionEnter(Collision other)
	{
		//Get hit by bullets
		if(other.gameObject.GetComponent<PlayerBulletController>() != null)
		{
			if(!other.gameObject.GetComponent<PlayerBulletController>().mSideBullet)
			{
				Debug.Log("Player bullet hit" + name);
				Destroy (other.gameObject);
				mHornHP--;
				mHornSprite.color = Color.Lerp(mHornSprite.color, Color.red, 0.9f);
			}
		}
		//Kill Player when touched
		if(other.gameObject.GetComponent<PlayerShipController>() != null)
		{
			mScoreManager.LoseALife();
			
		}
	}//END of OnCollsionEnter()

	void DestroyHorn()
	{
		Instantiate(mHornDeathParticles, transform.position, Quaternion.identity);
		if(mRightHorn)
		{
			mBossBody.mRightHornAlive = false;
		}
		else
		{
			mBossBody.mLeftHornAlive = false;
		}
		Destroy(this.gameObject);
	}//END of DestroyHorn()

	public void FireHornBeam()
	{
		mHornParticleBeam.SetActive(true);
		mHornParticleBeam.transform.Rotate(new Vector3(0f,0f,mHornBeamRotationSpeed));
		mBulletFireDir = Quaternion.AngleAxis(mHornBeamRotationSpeed,Vector3.forward)*mBulletFireDir;
//		for(int i = 0; i < 2; i++)
//		{
//			Vector3 beamSpawnOffset = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0f);
//			beamSpawnOffset+= mBeamSource.position;
//			GameObject hornBeam = Instantiate(mHornBeam, beamSpawnOffset, Quaternion.identity) as GameObject;
//			hornBeam.GetComponent<EnemyBulletController>().mFireDir = mBulletFireDir;
//			mBulletFireDir = Quaternion.AngleAxis(mHornBeamRotationSpeed,Vector3.forward)*mBulletFireDir;
//		}

	}//END of FireHornBeam
	public void FireHornBeamAlt()
	{
		mHornParticleBeam.SetActive(true);
		mHornParticleBeam.transform.Rotate(new Vector3(0f,0f,-1f*mHornBeamRotationSpeed));
		mBulletFireDir = Quaternion.AngleAxis(-1f*mHornBeamRotationSpeed,Vector3.forward)*mBulletFireDir;
//		for(int i = 0; i < 2; i++)
//		{
//			Vector3 beamSpawnOffset = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0f);
//			beamSpawnOffset+= mBeamSource.position;
//			GameObject hornBeam = Instantiate(mHornBeam, beamSpawnOffset, Quaternion.identity) as GameObject;
//			hornBeam.GetComponent<EnemyBulletController>().mFireDir = mBulletFireDir;
//			mBulletFireDir = Quaternion.AngleAxis(-1f*mHornBeamRotationSpeed,Vector3.forward)*mBulletFireDir;
//		}
		
	}//END of FireHornBeamAlt

}
