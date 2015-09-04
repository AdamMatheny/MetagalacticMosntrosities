using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverheatMeterUI : MonoBehaviour 
{
	[SerializeField] private Image mOverHeatBar;
	[SerializeField] private Image mOverHeatOverlay;
	[SerializeField] private Image mBlankBulb;
	[SerializeField] private Image mRedBulb;
	[SerializeField] private ParticleSystem mSteamUp;
	[SerializeField] private ParticleSystem mSteamDown;
	[SerializeField] private string mSteamUpName = "OverheatSteamUp";
	[SerializeField] private string mSteamDownName = "OverheatSteamDown";

	public bool mPlayerTwoUI = false;

	bool mCanPlaySteamNoise = true;

	PlayerShipController mPlayer;
	PlayerTwoShipController mPlayerTwo;

	float mHeatLevel = 0f;
	float mMaxHeat = 45f;
	bool mIsOverheated = false;
	// Use this for initialization
	void Start () 
	{

		//Find the player ship -Adam
		mPlayer = FindObjectOfType<PlayerShipController>();
		//Find the second player's ship ~Adam
		if(mPlayerTwoUI && mPlayer.mPlayerTwo != null)
		{
			mPlayerTwo = mPlayer.mPlayerTwo;
		}
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{

		if(mSteamUp == null)
		{
			mSteamUp = GameObject.Find(mSteamUpName).GetComponent<ParticleSystem>();
		}
		else
		{
			if(mPlayer.heatLevel/mPlayer.maxHeatLevel < 0.9f && !mSteamUp.GetComponent<AudioSource>().isPlaying)
			{
				mCanPlaySteamNoise = true;
			}
		}
		if(mSteamDown == null)
		{
			mSteamDown = GameObject.Find(mSteamDownName).GetComponent<ParticleSystem>();
		}

		//Read Heat Level, Maximum Heat Level, and overheat status from either player 1 or player 2 ~Adam
		if(!mPlayerTwoUI && mPlayer != null)
		{
			mHeatLevel = mPlayer.heatLevel;
			mMaxHeat = mPlayer.maxHeatLevel;
			mIsOverheated = mPlayer.isOverheated;
		}
		else if (mPlayerTwoUI && mPlayerTwo != null)
		{
			mHeatLevel = mPlayerTwo.heatLevel;
			mMaxHeat = mPlayerTwo.maxHeatLevel;
			mIsOverheated = mPlayerTwo.isOverheated;
		}

		//Safety in case the player ship connection is lost -Adam
		if(mPlayer == null)
		{
			mPlayer = FindObjectOfType<PlayerShipController>();
		}

		else if (mPlayerTwo == null && mPlayerTwoUI && mPlayer.mPlayerTwo != null)
		{
			mPlayerTwo = mPlayer.mPlayerTwo;
		}

		else if(GetComponent<Image>().canvas.isActiveAndEnabled) //Only do stuff when the canvas is actually turned on
		{
			//Make the bar move up and down
			mOverHeatBar.GetComponent<RectTransform>().localScale = new Vector3(1f, mHeatLevel/mMaxHeat, 1f);

			//Display overlay when overheated
			if(mIsOverheated)
			{
				mOverHeatOverlay.enabled = true;
				GetComponent<Animator>().speed = 0f;
				if(mSteamUp != null && mSteamDown != null)
				{
					mSteamUp.Play();
					mSteamUp.GetComponentInChildren<ParticleSystem>().Play();
					mSteamDown.Stop();
				}
				mRedBulb.enabled = false;
				mBlankBulb.enabled = false;
			}
			else
			{
				if(mSteamUp != null & mSteamDown != null)
				{
					mSteamDown.Play();
					mSteamUp.Stop();
					mSteamUp.GetComponentInChildren<ParticleSystem>().Stop();
				}
				mOverHeatOverlay.enabled = false;
				if(mHeatLevel/mMaxHeat > 0.9f)
				{
					mRedBulb.enabled = true;
					mBlankBulb.enabled = false;

					GetComponent<Animator>().speed = 5f;
					if(mSteamDown != null)
					{
						mSteamDown.startSpeed = 5f;
						mSteamDown.startLifetime = 0.5f;
						mSteamDown.emissionRate = 50f;
						if(mSteamUp != null && mCanPlaySteamNoise)
						{
							mSteamUp.GetComponent<AudioSource>().Play();
							mCanPlaySteamNoise = false;
						}
					}
				}
				else
				{
					mRedBulb.enabled = false;
					mBlankBulb.enabled = true;

					GetComponent<Animator>().speed = 1f;
					if(mSteamDown != null)
					{
						mSteamDown.startSpeed = 1f;
						mSteamDown.startLifetime = 2f;
						mSteamDown.emissionRate = 10f;
					}
				}
			}
		}
		//Hide the steam if the canvas it turned off or the ship is missing
		else
		{
			if(mSteamUp != null & mSteamDown != null)
			{
				mSteamUp.Stop();
				mSteamUp.GetComponentInChildren<ParticleSystem>().Stop();
				mSteamDown.Stop();
			}
		}
	}//END of Update()
}
