using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LDHeroUI : MonoBehaviour 
{
	public HeroShipAI mHero;
	public Sprite[] mShipStages;
	public float mHealthPercent;
	public Image mShipIcon;
	public RectTransform mHealthPanel;

	// Use this for initialization
	void Start () 
	{
		if(FindObjectOfType<HeroShipAI>() != null)
		{
			mHero = FindObjectOfType<HeroShipAI>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mHero == null)
		{
			mShipIcon.sprite = mShipStages[7];
			mHealthPercent = 0f;
			if(FindObjectOfType<HeroShipAI>() != null)
			{
				mHero = FindObjectOfType<HeroShipAI>();
			}

		}

		else
		{
			//Find how much health the current hero ship has left ~Adam
			mHealthPercent = mHero.mHitsRemaining/(mHero.mMaxHits+0.001f);


			//Change the display of the ship based on health percentage ~Adam
			if(mShipIcon != null && mShipStages.Length >= 8)
			{
				//Ship at full health ~Adam
				if(mHealthPercent >= 0.9f)
				{
					mShipIcon.sprite = mShipStages[0];
				}
				//One Claw down ~Adam
				else if(mHealthPercent >= 0.8f)
				{
					mShipIcon.sprite = mShipStages[1];
				}
				//Both Claws down ~Adam
				else if(mHealthPercent >= 0.6f)
				{
					mShipIcon.sprite = mShipStages[2];
				}
				//One Gun down ~Adam
				else if(mHealthPercent >= 0.4f)
				{
					mShipIcon.sprite = mShipStages[3];
				}
				//Both Guns down ~Adam
				else if(mHealthPercent >= 0.2f)
				{
					mShipIcon.sprite = mShipStages[4];
				}
				//One Wing down ~Adam
				else if(mHealthPercent >= 0.1f)
				{
					mShipIcon.sprite = mShipStages[5];
				}
				//Both Wings down ~Adam
				else if(mHealthPercent >= 0.01f)
				{
					mShipIcon.sprite = mShipStages[6];
				}
				//Dead Ship ~Adam
				else
				{
					mShipIcon.sprite = mShipStages[7];
				}

			}

			if(!mHero.mHasEntered)
			{
				mShipIcon.sprite = mShipStages[7];
			}
		}

		//Adjust life meter
		mHealthPanel.localScale = new Vector3(mHealthPercent,1f,1f);
	}
}
