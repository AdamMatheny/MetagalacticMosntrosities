using UnityEngine;
using System.Collections;

public class LaserFistPowerup : MonoBehaviour 
{
	BoxCollider mDeathBox;
	
	Vector3 mLaserCenterFull = new Vector3(0f,4.8f,0f);
	Vector3 mLaserSizeFull = new Vector3(1f,10f, 3f);
	
	Vector3 mLaserCenterStart = new Vector3(0f,0f,0f);
	Vector3 mLaserSizeStart = new Vector3(0.1f,0.1f, 3f);
	
	public float maxTime;
	public float time;
	
	public GameObject bigBoom;
	
	public bool mPlayer2Weapon = false;
	
	float mLaserFistTimer = 0f;
	
	public bool mHasHitBoss = false;
	
	// Use this for initialization
	void Start () 
	{
		mDeathBox = GetComponent<BoxCollider>();
	}//END Start()
	
	
	
	// Update is called once per frame
	void Update () 
	{
		
		if (bigBoom != null) 
		{
			
			//			if (time > 0) 
			//			{
			//				time -= Time.deltaTime;
			//			} 
			//			else 
			//			{
			//				
			//				time = maxTime;
			//				bigBoom.SetActive(true);
			//				mDeathBox.size *= 10f;
			//			}
		}
		
		mLaserFistTimer += Time.deltaTime;
		
		if(GetComponent<Animator>().GetBool("Expanding"))
		{
			mDeathBox.center = Vector3.Lerp(mDeathBox.center, mLaserCenterFull, 0.1f);
			mDeathBox.size = Vector3.Lerp(mDeathBox.size, mLaserSizeFull, 0.1f);
			Camera.main.GetComponent<CameraShaker>().ShakeCameraEnemy();
		}
		else
		{
			mDeathBox.center = Vector3.Lerp(mDeathBox.center, mLaserCenterStart, 0.1f);
			mDeathBox.size = Vector3.Lerp(mDeathBox.size, mLaserSizeStart, 0.1f);
			
		}
		
		if(mLaserFistTimer >= 5f)
		{
			StartDeathBoxShrinkage();
		}
	}//END Update()
	
	public void StartDeathBoxExpansion()
	{
		GetComponent<Animator>().SetBool("Expanding", true);
		mHasHitBoss = false;
		
	}//END StartDeathBoxExpansion()
	
	public void StartDeathBoxShrinkage()
	{
		GetComponent<Animator>().SetBool("Expanding", false);
	}//END StartDeathBoxExpansion()
	
	public void StopLaserFist()
	{
		mLaserFistTimer = 0f;
		if(bigBoom != null)
		{
			bigBoom.SetActive(false);
		}
		
		mDeathBox.center = mLaserCenterStart;
		mDeathBox.size = mLaserSizeStart;
		GetComponent<Animator>().SetBool("Expanding", false);
		this.gameObject.SetActive(false);
	}//END StopLaserFist()
	
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<EnemyShipAI>() != null)
		{
			if(mPlayer2Weapon)
			{
				other.GetComponent<EnemyShipAI>().mKillerNumber = 2;
			}
			other.GetComponent<EnemyShipAI>().EnemyShipDie();
			Debug.Log("EnterTrigger Destroying " + other.GetComponent<EnemyShipAI>().name);
		}
		if(other.GetComponent<EnemyBulletController>() != null)
		{
			if(other.GetComponent<EnemyBulletController>().mShootable)
			{
				Destroy(other.gameObject);
			}
		}
		
		//Destroy a BossWeakPoint ~Adam
		if(!mHasHitBoss && other.GetComponent<BossWeakPoint>() != null)
		{
			Debug.Log ("Hit a weakpoint!" + other.name);
			mHasHitBoss = true;
			BossWeakPoint weakPoint = other.GetComponent<BossWeakPoint>().mBossCentral.mWeakPoints[0];

			for(int i= 0; i < 50; i++)
			{
				if(weakPoint != null)
				{
					Debug.Log ("Hitting weakpoint " + weakPoint.name);
					weakPoint.TakeDamage();
				}
			}
		}
	}//END OnTriggerEnter()
	
	public void ShowBigBoom()
	{
		if(bigBoom != null)
		{
			bigBoom.SetActive (true);
			foreach(EnemyShipAI enemy in FindObjectsOfType<EnemyShipAI>())
			{
				if(mPlayer2Weapon)
				{
					enemy.mKillerNumber = 2;
				}
				enemy.EnemyShipDie();
			}
		}
	}
	
	//	void OnTriggerStay(Collider other)
	//	{
	//		if(other.GetComponent<EnemyShipAI>() != null)
	//		{
	//			other.GetComponent<EnemyShipAI>().EnemyShipDie();
	//			Debug.Log("StayTrigger Destroying " + other.GetComponent<EnemyShipAI>().name);
	//		}
	//	}
}
