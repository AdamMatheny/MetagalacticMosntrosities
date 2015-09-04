using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DuoBossReak : BossGenericScript 
{
	
	public bool leftHornAlive = true;
	public bool rightHornAlive = true;
	
	//public SpriteRenderer spriter;
	
	public GameObject tail;
	public GameObject head;
	public GameObject stomach;
	public GameObject mBile;
	public GameObject mFlame;
	


	public override void Start ()
	{
		//spriter = GetComponent<SpriteRenderer> ();
		base.Start ();
	}
	
	public override void Update ()
	{

		//Blow up head first ~Adam
		if(!leftHornAlive || !leftHornAlive)
		{
				
			head.SetActive(false);
		}
		//Then blow up tail and stomach ~Adam
		if(!leftHornAlive && !rightHornAlive)
		{
			tail.SetActive(false);

			stomach.SetActive(false);
			//mBile.SetActive(true);
			mCurrentCharge = 0f;
			mChargeReady = false;
		}

		if(mDying || mOverheated)
		{
			mFlame.SetActive (false);
		}
		else if(!mDying)
		{
			mFlame.SetActive (true);
		}
		base.Update ();

	}

}
