using UnityEngine;
using System.Collections;

public class SwarmBoss : BossGenericScript 
{
	public float mAutoDieTimer = 60f;
	public GameObject[] mSpawners;

	//Set Starting health ~Adam
	public override void Start ()
	{
		base.Start ();
		mCurrentHealth = 100f;
		mTotalHealth = 100f;
	}

	// Update is called once per frame
	public override void Update () 
	{
		mAutoDieTimer -= Time.deltaTime;
		Time.timeScale = 1f;

		//Kill all the enemy ships after a time limit in case something gets stuck off-screen ~Adam
		if(mAutoDieTimer <= 0f)
		{
//			foreach(FakeEnemy faker in FindObjectsOfType<FakeEnemy>())
//			{
//				Destroy (faker.gameObject);
//			}
//			foreach(EnemyShipAI enemy in FindObjectsOfType<EnemyShipAI>())
//			{
//				enemy.EnemyShipDie();
//			}
		}

		//Stop spawning enemies while overheated
		if(mOverheated)
		{
			foreach(GameObject spawner in mSpawners)
			{
				spawner.SetActive (false);
			}
		}
		else
		{
			foreach(GameObject spawner in mSpawners)
			{
				spawner.SetActive (true);
			}
		}




		//Die when there are no more enemies left ~Adam
		if(FindObjectOfType<FakeEnemy>() == null && FindObjectOfType<EnemyShipAI>() == null)
		{
			mDying = true;
		}
		//Figure out how much "health" this swarm has left ~Adam
		else
		{
			mCurrentHealth = 0;
			if(!mDying)
			{
				foreach(FakeEnemy faker in FindObjectsOfType<FakeEnemy>())
				{
					mCurrentHealth ++;
					if(mWeakPoints.Count >0)
					{
						mWeakPoints[0] = faker.transform;
					}
				}
				foreach(EnemyShipAI enemy in FindObjectsOfType<EnemyShipAI>())
				{
					mCurrentHealth ++;
					if(mWeakPoints.Count >0)
					{
						mWeakPoints[0] = enemy.transform;
					}
				}
			}
		}


		base.Update ();
	}


}
