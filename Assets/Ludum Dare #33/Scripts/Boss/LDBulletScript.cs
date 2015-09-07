using UnityEngine;
using System.Collections;

public class LDBulletScript : EnemyBulletController 
{
	public int mHitDamage = 1;

	//For Steering bullets
	public bool mSteerable = false;
	public Vector3 mSteerRotation = Vector3.zero;
	public float mRotateSpeed = 10f;

	public void OnCollisionEnter(Collision other)
	{
		
//		if (other.gameObject.tag == "Player") 
//		{
//			if(other.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
//			{
//				other.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
//			}
//		}

		//else
		if (other.gameObject.name == "ShipCore") 
		{
			Debug.Log (gameObject.name + " hit ship core");
			if(other.transform.parent.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.transform.parent.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
			Destroy(gameObject);
		}
	}

	public override void Update()
	{
		if(mSteerable)
		{
			mSteerRotation = new Vector3(Input.GetAxis ("RightAnalogHorizontal"), Input.GetAxis ("RightAnalogVertical"), 0);
			Vector3.Normalize (mSteerRotation);
			mSteerRotation = new Vector3(0f, 0f, Vector3.Angle(mSteerRotation, Vector3.down));
			if(Input.GetAxis ("RightAnalogHorizontal") < 0f)
			{
				mSteerRotation *= -1f;
			}
			if(Input.GetAxis ("RightAnalogHorizontal") !=0f || Input.GetAxis ("RightAnalogVertical") != 0f)
			{
				
				
				//transform.rotation =Quaternion.Lerp (transform.rotation, Quaternion.Euler (mSteerRotation), 0.001f*mRotateSpeed);
				transform.rotation = Quaternion.Euler (mSteerRotation);
			}

			bulletForce = transform.up*mBulletSpeed*-1f;
			//			bulletForce = new Vector2(0.0f,mBulletSpeed * -1.0f);
			GetComponent<Rigidbody> ().velocity = bulletForce;

			//Self-destruct after a certain amount of time
			mSelfDestructTimer-=Time.deltaTime;
			if(mSelfDestructTimer<0.0f)
			{
				if(bulletExplosion != null)
				{
						
					Instantiate(bulletExplosion, transform.position, Quaternion.identity);
				}

				Destroy(gameObject);
			}
		}
		else
		{
			base.Update ();
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		
//		if (other.gameObject.tag == "Player") 
//		{
//			if(other.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
//			{
//				other.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
//			}
//		}


		//else
		if (other.gameObject.name == "ShipCore") 
		{
			Debug.Log (gameObject.name + " hit ship core");
			if(other.transform.parent.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.transform.parent.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
			transform.GetChild(0).SetParent (null);
			Destroy(gameObject);
		}
	}
}
