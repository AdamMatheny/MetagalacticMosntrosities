using UnityEngine;
using System.Collections;

public class TentaclesWeakPoint : BossWeakPoint {

	public int health;

	public bool isGem;

	public SpriteRenderer sprite;

	public GameObject mDeathEffect;
	public Transform mExplosionPoint;

	public override void Start(){

		mBossCentral.mTotalHealth += health;
		mBossCentral.mCurrentHealth += health;
	}

	public override void Update(){

		if(sprite != null)
		{
			sprite.color = Color.Lerp (sprite.color, Color.white,0.1f);
		}
	}

	public override void TakeDamage()
	{
		
		health --;
		
		//For flashing when hit ~Adam
		if(sprite != null)
		{
			sprite.color = Color.Lerp (sprite.color, Color.blue,1f);
		}
		
		base.TakeDamage ();
		
		if (health <= 0) {
			
			BlowUp();
		}
	}

	public void BlowUp(){

		if (isGem) {

			mBossCentral.GetComponent<Boss5> ().ChangeAnimation();
			//Debug.Log("Called The Change Anim Function!");
		}
		
		if(mDeathEffect != null)
		{
			if(mExplosionPoint !=null)
			{
				Instantiate(mDeathEffect, mExplosionPoint.position, Quaternion.identity);
			}
			else
			{
				Instantiate(mDeathEffect, transform.position, Quaternion.identity);
			}
		}
		Destroy (gameObject);
	}
}
