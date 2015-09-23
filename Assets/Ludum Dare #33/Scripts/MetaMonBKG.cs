using UnityEngine;
using System.Collections;

public class MetaMonBKG : MonoBehaviour 
{
	
	float m_fSpeed;
	
	[SerializeField] private GameObject mPlayerAvatar;
	[SerializeField] private GameObject mCurrentBoss;

	Vector3 mDefaultPosition;
	
	Vector3 mPlayerMoveDirection = Vector3.zero;
	[SerializeField] private Vector3 desiredPosition;
	//float mPlayerMoveDistance = 0f;
	float mRenderOffest = 0f;
	
	//For making the scrolling persist between levels ~Adam
	[SerializeField] private bool mFadeAway = false;

	//For setting how far the background can scroll horizontally ~Adam
	[SerializeField] private float mXMin;
	[SerializeField] private float mXMax;
	
	[SerializeField] private int mBossCounter = 0;
	[SerializeField] private int mBossNumber;
	bool mBossDeathCounted = false;

	// Use this for initialization
	void Start()
	{
		mPlayerAvatar = GameObject.FindGameObjectWithTag("Player");
		mDefaultPosition = transform.position;
		if(FindObjectOfType<BossGenericScript>() != null)
		{
			mCurrentBoss = FindObjectOfType<BossGenericScript>().gameObject;
		}

		m_fSpeed = 0.04f;
		


	}
	
	// Update is called once per frame
	void Update()
	{

		if(mPlayerAvatar == null)
		{
			mPlayerAvatar = GameObject.FindGameObjectWithTag("Player");
		}
		if(mCurrentBoss == null && FindObjectOfType<BossGenericScript>() != null)
		{
			mCurrentBoss = FindObjectOfType<BossGenericScript>().gameObject;
		}

		//For fading away with each boss death ~Adam
		if(mCurrentBoss != null && mCurrentBoss.GetComponent<BossGenericScript>().mDying && !mBossDeathCounted)
		{
			mBossDeathCounted = true;
			mBossCounter++;
			if(mBossCounter >= mBossNumber)
			{
				mFadeAway = true;
			}
		}
		if(mCurrentBoss != null && !mCurrentBoss.GetComponent<BossGenericScript>().mDying && mBossDeathCounted)
		{
			mBossDeathCounted = false;
		}

		if(Time.timeScale != 0f)
		{
			if(mFadeAway)
			{
				GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, new Color(0f,0f,1f,0f), 0.01f);
				if(GetComponent<Renderer>().material.color.a < 0.01f)
				{
					Destroy(this.gameObject);
				}
			}
			
//			if(mPlayerAvatar != null)
//			{
//				
//				mPlayerMoveDirection = mPlayerAvatar.GetComponent<HeroShipAI>().mMoveDir;
//				if( (transform.position.x  < mXMax && mPlayerAvatar.transform.position.x < -15) 
//				   || (transform.position.x > mXMin && mPlayerAvatar.transform.position.x > 15) )
//				{
//					desiredPosition = Vector3.Lerp(desiredPosition, new Vector3(transform.position.x + ((mPlayerMoveDirection.x)) *-0.6f, transform.position.y, transform.position.z), 0.8f);
//					
//					//desiredPosition = Vector3.Lerp(desiredPosition, new Vector3(mDefaultPosition.x + (mPlayerAvatar.transform.position.x+(mPlayerMoveDirection.x*10f)) *-0.4f, transform.position.y, transform.position.z), 0.8f);
//					//			desiredPosition += new Vector3((mPlayerMoveDirection.x*-10f),0f,0f);
//					
//					transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f-(Time.deltaTime* Mathf.Abs(mPlayerMoveDirection.x)));
//					transform.position = new Vector3(transform.position.x, mDefaultPosition.y, mDefaultPosition.z);
//				}
//			}

			if(mCurrentBoss != null)
			{
				if(mCurrentBoss.transform.position.x < mCurrentBoss.GetComponent<BossGenericScript>().mBounds[0]+5f)
				{
					desiredPosition += Vector3.left*m_fSpeed;
				}
				else if(mCurrentBoss.transform.position.x > mCurrentBoss.GetComponent<BossGenericScript>().mBounds[1]-5f)
				{
					desiredPosition += Vector3.right*m_fSpeed;
				}
//				else
//				{
//					desiredPosition = Vector3.Lerp(desiredPosition, new Vector3(mCurrentBoss.transform.position.x*-1f, transform.position.y, transform.position.z), 0.8f);
//				}

				transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f);
				transform.position = new Vector3(transform.position.x, mDefaultPosition.y, mDefaultPosition.z);
			}

			mRenderOffest += (Time.deltaTime * m_fSpeed);
			
		
			
			GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, mRenderOffest));
		}
	}
	
	void LateUpdate()
	{
		if(Time.timeScale != 0f)
		{
			//	Debug.Log("Updating Score Manager background position.");
			if(transform.position.x > mXMax)
			{
				transform.position = new Vector3(mXMax, transform.position.y, transform.position.z);
			}
			if(transform.position.x < mXMin)
			{
				transform.position = new Vector3(mXMin, transform.position.y, transform.position.z);
			}
		}
	}
}
