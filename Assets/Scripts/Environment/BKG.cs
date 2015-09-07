using UnityEngine;
using System.Collections;

public class BKG : MonoBehaviour
{

	float m_fSpeed;

	GameObject mPlayerAvatar;
	Vector3 mDefaultPosition;

	Vector3 mPlayerMoveDirection = Vector3.zero;
	[SerializeField] private Vector3 desiredPosition;
	//float mPlayerMoveDistance = 0f;
	float mRenderOffest = 0f;

	//For making the scrolling persist between levels ~Adam
	[SerializeField] private ScoreManager mScoreManager;
	[SerializeField] private bool mCheckLastOffset = true;
	[SerializeField] private bool mFadeAway = false;
	[SerializeField] private bool mFirstOfKind = false;

	//For setting how far the background can scroll horizontally ~Adam
	[SerializeField] private float mXMin;
	[SerializeField] private float mXMax;


    // Use this for initialization
    void Start()
    {
		mPlayerAvatar = GameObject.FindGameObjectWithTag("Player");
		mDefaultPosition = transform.position;

		//transform.position = new Vector3 (transform.position.x, transform.position.y, 50f);

		//Deal with potential duplicate score managers and only use the oldest one ~Adam
		ScoreManager[] existingScoreManagers = FindObjectsOfType<ScoreManager>();
		foreach(ScoreManager scoreManager in existingScoreManagers)
		{
			if(mScoreManager == null)
			{
				mScoreManager = scoreManager;
			}
			else
			{
				if(scoreManager.mOriginalLevel < mScoreManager.mOriginalLevel)
				{
					mScoreManager = scoreManager;
				}
			}
		}
		//mScoreManager = GameObject.FindObjectOfType<ScoreManager>();
		m_fSpeed = 0.04f;

		if(mCheckLastOffset)
		{
			mRenderOffest = mScoreManager.mBackgroundOffset;
			transform.position = new Vector3(mScoreManager.mBackgroundPosition.x, mScoreManager.mBackgroundPosition.y, transform.position.z);
			desiredPosition = transform.position;
		}
		if(mFirstOfKind)
		{
			mRenderOffest = 0f;
			//mScoreManager.mBackgroundOffset = 0f;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>();
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

			if(mPlayerAvatar != null)
			{

				mPlayerMoveDirection = mPlayerAvatar.GetComponent<PlayerShipController>().mMoveDir;
				if( (transform.position.x  < mXMax && mPlayerAvatar.transform.position.x < -15) 
				   || (transform.position.x > mXMin && mPlayerAvatar.transform.position.x > 15) )
				{
					desiredPosition = Vector3.Lerp(desiredPosition, new Vector3(transform.position.x + ((mPlayerMoveDirection.x)) *-0.6f, transform.position.y, transform.position.z), 0.8f);
			
					//desiredPosition = Vector3.Lerp(desiredPosition, new Vector3(mDefaultPosition.x + (mPlayerAvatar.transform.position.x+(mPlayerMoveDirection.x*10f)) *-0.4f, transform.position.y, transform.position.z), 0.8f);
		//			desiredPosition += new Vector3((mPlayerMoveDirection.x*-10f),0f,0f);

					transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f-(Time.deltaTime* Mathf.Abs(mPlayerMoveDirection.x)));
					transform.position = new Vector3(transform.position.x, mDefaultPosition.y, mDefaultPosition.z);
				}
			}
			if(mPlayerMoveDirection.y >= 0f)
			{
				mRenderOffest += (Time.deltaTime * m_fSpeed) + (Time.deltaTime*m_fSpeed*mPlayerMoveDirection.y/15f) ;
			}
			else
			{
				mRenderOffest += (Time.deltaTime * m_fSpeed) + (Time.deltaTime*m_fSpeed*mPlayerMoveDirection.y*1.5f) ;
			}

			GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, mRenderOffest));
		}
	}

	void LateUpdate()
	{
		if(Time.timeScale != 0f)
		{
			if(!mFadeAway)
			{
				mScoreManager.mBackgroundOffset = mRenderOffest;
				mScoreManager.mBackgroundPosition = transform.position;
			}
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
