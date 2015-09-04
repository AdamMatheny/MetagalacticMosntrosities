using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipUIEmoteChanger : MonoBehaviour 
{
	[SerializeField] private Sprite[] mEmotes;
	/* The emote images that we'll be swapping out conditionally ~Adam
	 * 0: Dead
	 * 1: 1-25% lives remaining
	 * 2: 26-50% lives remaining
	 * 3: 51-75% lives remaining
	 * 4: 75-100% lives remaining
	 * 5: Shielded
	 * 6: OverHeat
	 * 7: Getting Hit
	 * 8: Firing Super Weapon
	 */
	[SerializeField] private ScoreManager mScoreMan;
	[SerializeField] private PlayerShipController mPlayer1Ship;
	[SerializeField] private Image mFace;
	// Use this for initialization
	void Start () 
	{
		//get the face sprite to change
		mFace = GetComponent<Image>();

		//find the palyer and the score manager ~Adam
		mPlayer1Ship = FindObjectOfType<PlayerShipController>();
		mScoreMan = FindObjectOfType<ScoreManager>();
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//make sure the player and the score manager aren't null
		if(mPlayer1Ship == null)
		{
			mPlayer1Ship = FindObjectOfType<PlayerShipController>();
		}
		if(mScoreMan == null)
		{
			mScoreMan = FindObjectOfType<ScoreManager>();
		}

		//Change the face
		if(mPlayer1Ship != null && mScoreMan != null)
		{
			//Firing super weapon ~Adam
			if(mPlayer1Ship.mBigBlast.activeInHierarchy || mPlayer1Ship.mLaserFist.activeInHierarchy)
			{
				mFace.sprite = mEmotes[8];
			}
			//Getting Hit
			else if(mScoreMan.mPlayerSafeTime > 0f)
			{
				mFace.sprite = mEmotes[7];
			}
			//OverHeated
			else if(mPlayer1Ship.isOverheated)
			{
				mFace.sprite = mEmotes[6];
			}
			//Shielded
			else if(mPlayer1Ship.mShielded)
			{
				mFace.sprite = mEmotes[5];
			}
			//Dead
			else if(mScoreMan.mLivesRemaining <= 0)
			{
				mFace.sprite = mEmotes[0];
			}
			//1-25%
			else if(mScoreMan.mLivesRemaining/(mScoreMan.mMaxLives+0.00001f) <= 0.25f)
			{
				mFace.sprite = mEmotes[1];
			}
			//26-50%
			else if(mScoreMan.mLivesRemaining/(mScoreMan.mMaxLives+0.00001f) <= 0.5f)
			{
				mFace.sprite = mEmotes[2];
			}
			//50-75%
			else if(mScoreMan.mLivesRemaining/(mScoreMan.mMaxLives+0.00001f) <= 0.75f)
			{
				mFace.sprite = mEmotes[3];
			}
			//76-100%
			else
			{
				mFace.sprite = mEmotes[4];
			}
		}
		else if(mScoreMan != null && mScoreMan.mLivesRemaining <= 0)
		{
			mFace.sprite = mEmotes[0];
		}

	}//END of Update()
}
