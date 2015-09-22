using UnityEngine;
using System.Collections;

public class HornetMinionKiller : MonoBehaviour 
{
	//Refrence to the main boss script ~Adam
	public BossGenericScript mBossCentral;

	public int mDestructionHealth;
	public GameObject mMinionSpawner;


	// Update is called once per frame
	void Update () 
	{
		if(mBossCentral.mCurrentHealth < mDestructionHealth)
		{
			Destroy (mMinionSpawner);
			foreach(EnemyShipAI minion in FindObjectsOfType<EnemyShipAI>())
			{
				minion.EnemyShipDie ();
			}
		}
	}
}
