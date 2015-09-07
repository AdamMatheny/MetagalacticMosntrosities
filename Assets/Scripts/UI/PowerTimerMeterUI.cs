using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerTimerMeterUI : MonoBehaviour 
{
	[SerializeField] private Image mPowerTimerBar;
	[SerializeField] private Image mPowerTimerBulb;
	[SerializeField] private Image mPowerTimerSwirl;

	public float swirlStartY;

	PlayerShipController mPlayer;
	PlayerTwoShipController mPlayerTwo;

	public bool mPlayerTwoUI = false;

	// Use this for initialization
	void Start () 
	{
		swirlStartY = mPowerTimerSwirl.transform.position.y;

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
		//Safety in case the player ship connection is lost -Adam
		if(mPlayer == null)
		{
			mPlayer = FindObjectOfType<PlayerShipController>();
		}
		
		else
		{


			if( (!mPlayerTwoUI && mPlayer != null && mPlayer.mThreeBullet) || (mPlayerTwoUI && mPlayerTwo != null && mPlayerTwo.mThreeBullet) )
			{
				//Make the bar move up and down
				mPowerTimerBar.enabled = true;
				if(!mPlayerTwoUI && mPlayer != null)
				{
					mPowerTimerBar.GetComponent<RectTransform>().localScale = new Vector3(1f, mPlayer.mThreeBulletTimer/30f, 1f);
				}
				else if(mPlayerTwoUI && mPlayerTwo != null)
				{
					mPowerTimerBar.GetComponent<RectTransform>().localScale = new Vector3(1f, mPlayerTwo.mThreeBulletTimer/30f, 1f);
				}
				mPowerTimerSwirl.transform.position = new Vector3(mPowerTimerSwirl.transform.position.x, swirlStartY + (mPowerTimerBar.GetComponent<RectTransform>().localScale.y * 24), mPowerTimerSwirl.transform.position.z);
				mPowerTimerBulb.enabled = true;
			}
			else
			{
				mPowerTimerBar.enabled = false;
				mPowerTimerBulb.enabled = false;

			}
		}
	}//END of Update()
}
