using UnityEngine;
using System.Collections;

public class LDBossBeam : MonoBehaviour 
{

	public int mHitDamage = 1;

	public void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.name == "ShipCore") 
		{
			if(other.transform.parent.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.transform.parent.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (other.gameObject.name == "ShipCore") 
		{
			if(other.transform.parent.gameObject.GetComponent<HeroShipAI>().mInvincibleTimer <= 0f)
			{
				other.transform.parent.gameObject.GetComponent<HeroShipAI>().HitHeroShip(mHitDamage);
			}
		}
	}

}
