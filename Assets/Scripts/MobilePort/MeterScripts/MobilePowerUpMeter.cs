using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobilePowerUpMeter : MonoBehaviour 
{
	[SerializeField] private Image mPowerTimerBar;

	PlayerShipController mPlayer;

	// Use this for initialization
	void Start () 
	{
		//Find the player ship -Adam
		mPlayer = FindObjectOfType<PlayerShipController>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		//Safety in case the player ship connection is lost -Adam
		if(mPlayer == null)
		{
			mPlayer = FindObjectOfType<PlayerShipController>();
		}

		else if(mPowerTimerBar.canvas.isActiveAndEnabled) //Only do stuff when the canvas is actually turned on
		{
			//Make the bar move up and down ~Adam
			if(mPlayer.mThreeBullet)
			{
				//Make the bar move up and down
				mPowerTimerBar.enabled = true;
				mPowerTimerBar.GetComponent<RectTransform>().localScale = new Vector3(mPlayer.mThreeBulletTimer/30f, 1, 1f);

			}
			//Hide the bar when not powered up ~Adam
			else
			{
				mPowerTimerBar.enabled = false;
			}
		}

	}//END of Update()
}
