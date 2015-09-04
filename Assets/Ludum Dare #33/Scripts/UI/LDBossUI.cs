using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LDBossUI : MonoBehaviour 
{
	public Image mBossIcon;
	public Sprite mCurrentIcon;
	public BossGenericScript mBoss;

	public Image mHealthPanel;
	public Image mChargePanel;
	public Image mOverheatPanel;

	public GameObject mChargeWeaponButton;

	// Use this for initialization
	void Start () 
	{
		//Find the boss ~Adam
		if(FindObjectOfType<BossGenericScript>() != null)
		{
			mBoss = FindObjectOfType<BossGenericScript>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Keep the icon updated ~Adam
		if(mCurrentIcon != null)
		{
			mBossIcon.sprite = mCurrentIcon;
		}

		//Find the boss ~Adam
		if(mBoss == null && FindObjectOfType<BossGenericScript>() != null)
		{
			mBoss = FindObjectOfType<BossGenericScript>();
		}


		if(mBoss != null)
		{
			//Adjust meters ~Adam
			mHealthPanel.rectTransform.localScale = new Vector3((mBoss.mCurrentHealth/mBoss.mTotalHealth),1f,1f);
			mChargePanel.rectTransform.localScale = new Vector3((mBoss.mCurrentCharge/mBoss.mMaxCharge),1f,1f);
			mOverheatPanel.rectTransform.localScale = new Vector3((mBoss.mCurrentOverheat/mBoss.mMaxOverheat),1f,1f);
			//Toggle weapon button visibility ~Adam
			if(mBoss.mChargeReady)
			{
				mChargeWeaponButton.SetActive (true);
			}
			else
			{
				mChargeWeaponButton.SetActive (false);
			}

		}
	}
}
