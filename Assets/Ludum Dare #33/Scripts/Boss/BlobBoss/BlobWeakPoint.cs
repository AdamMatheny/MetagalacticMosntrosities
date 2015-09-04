using UnityEngine;
using System.Collections;

public class BlobWeakPoint : BossWeakPoint 
{
	public GameObject mBossBody;
	public SpriteRenderer mMainBodySprite;




	public override void TakeDamage()
	{
		

		mBossBody.GetComponent<BlobBoss>().mhealth --;
		base.TakeDamage ();
		//For flashing when hit ~Adam
		if(mMainBodySprite != null)
		{
			mMainBodySprite.color = Color.Lerp (mMainBodySprite.color, Color.red, 1f);
		}
	
		

		
	}
}
