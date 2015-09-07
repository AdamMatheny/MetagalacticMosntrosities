using UnityEngine;
using System.Collections;

public class SuperWeaponUI : MonoBehaviour 
{
	[SerializeField] private bool mCoOpUI = false;
	[SerializeField] private GameObject mWeaponReadyText;
	[SerializeField] private GameObject mLaserFistIcon;
	[SerializeField] private GameObject mBigBlastIcon;
	[SerializeField] private GameObject mShip;



	// Use this for initialization
	void Start () 
	{
		if (mShip != null) {

			//Find either the player 1 or the player 2 ship ~Adam
			if (!mCoOpUI) {
				mShip = FindObjectOfType<PlayerShipController> ().gameObject;
			} else {
				mShip = FindObjectOfType<PlayerTwoShipController> ().gameObject;
			}
		}
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		if(mShip != null)
		{
			#region Show/hide things if we're looking at player 1 ~Adam
			if(!mCoOpUI)
			{
				//Only show stuff if we're holding one of the super weapons ~Adam
				if(mShip.GetComponent<PlayerShipController>().mHaveBigBlast || mShip.GetComponent<PlayerShipController>().mHaveLaserFist)
				{
					mWeaponReadyText.SetActive(true);
					//Show/Hide the BigBlast icon ~Adam
					if(mShip.GetComponent<PlayerShipController>().mHaveBigBlast)
					{
						mBigBlastIcon.SetActive(true);
					}
					else
					{
						mBigBlastIcon.SetActive(false);
					}
					//Show/Hide the LaserFist icon
					if(mShip.GetComponent<PlayerShipController>().mHaveLaserFist)
					{
						mLaserFistIcon.SetActive(true);
					}
					else
					{
						mLaserFistIcon.SetActive(false);
					}
				}
				//Hide everything if we don't have a super weapon ~Adam
				else
				{
					mWeaponReadyText.SetActive(false);
					mBigBlastIcon.SetActive(false);
					mLaserFistIcon.SetActive(false);
				}
			}
			#endregion
			#region Show/hide things if we're looking at player 2 ~Adam
			else
			{
				//Only show stuff if we're holding one of the super weapons ~Adam
				if(mShip.GetComponent<PlayerTwoShipController>().mHaveBigBlast || mShip.GetComponent<PlayerTwoShipController>().mHaveLaserFist)
				{
					mWeaponReadyText.SetActive(true);
					//Show/Hide the BigBlast icon ~Adam
					if(mShip.GetComponent<PlayerTwoShipController>().mHaveBigBlast)
					{
						mBigBlastIcon.SetActive(true);
					}
					else
					{
						mBigBlastIcon.SetActive(false);
					}
					//Show/Hide the LaserFist icon
					if(mShip.GetComponent<PlayerTwoShipController>().mHaveLaserFist)
					{
						mLaserFistIcon.SetActive(true);
					}
					else
					{
						mLaserFistIcon.SetActive(false);
					}
				}
				//Hide everything if we don't have a super weapon ~Adam
				else
				{
					mWeaponReadyText.SetActive(false);
					mBigBlastIcon.SetActive(false);
					mLaserFistIcon.SetActive(false);
				}
				#endregion
			}
		}
		else
		{
			//Find either the player 1 or the player 2 ship ~Adam
			if(!mCoOpUI && FindObjectOfType<PlayerShipController>() != null)
			{
				mShip = FindObjectOfType<PlayerShipController>().gameObject;
			}
			else if (mCoOpUI && FindObjectOfType<PlayerTwoShipController>() != null)
			{
				mShip = FindObjectOfType<PlayerTwoShipController>().gameObject;
			}
		}
	}//END of Update()
}
