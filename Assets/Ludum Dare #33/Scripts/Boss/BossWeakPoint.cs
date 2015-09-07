using UnityEngine;
using System.Collections;

public class BossWeakPoint : MonoBehaviour 
{
	public BossGenericScript mBossCentral;

	// Use this for initialization
	public virtual void Start () 
	{
	
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
	
	}

	public virtual void TakeDamage()
	{
		
		if(mBossCentral.mCurrentHealth >0)
		{
			mBossCentral.mCurrentHealth--;
		}
		if(mBossCentral.mCurrentHealth <0)
		{
			mBossCentral.mCurrentHealth = 0;
		}
	}
}
