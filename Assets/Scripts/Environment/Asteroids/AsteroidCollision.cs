using UnityEngine;
using System.Collections;

public class AsteroidCollision : MonoBehaviour 
{
	
	public GameObject[] mPieces;

	public GameObject mLaserFistEmblem;
	public GameObject mBigBlastEmblem;


	void OnTriggerEnter(Collider other)
	{
		//From when we were experimenting with being able to hide under asteroids -Adam
//		if(other.gameObject.GetComponent<PlayerShipController>() != null)
//		{
//			if(transform.parent.transform.position.z < -2.63f && FindObjectOfType<ScoreManager>() != null)
//			{
//				FindObjectOfType<ScoreManager>().mPlayerSafeTime = 0.1f;
//			}
//
//		}

		if (other.gameObject.GetComponent<PlayerBulletController> () != null) 
		{
			AsteroidDeath();
			Destroy(other.gameObject);
		}
	}

	//From when we were experimenting with being able to hide under asteroids -Adam
//	void OnTriggerStay(Collider other)
//	{
//		if(other.gameObject.GetComponent<PlayerShipController>() != null)
//		{
//			if(transform.parent.transform.position.z < -2.63f && FindObjectOfType<ScoreManager>() != null)
//			{
//				FindObjectOfType<ScoreManager>().mPlayerSafeTime = 0.1f;
//			}
//			
//		}
//		
//
//	}

	public void AsteroidDeath()
	{
		//Update the Asteroid death count
		PlayerPrefs.SetInt("AsteroidCount", PlayerPrefs.GetInt("AsteroidCount") + 1);

		for (int i = 0; i < mPieces.Length; i ++)
		{
			GameObject newAsteroidBit = Instantiate(mPieces[i], transform.position, Quaternion.identity) as GameObject;
			newAsteroidBit.transform.localScale = transform.parent.localScale;
			newAsteroidBit.transform.localScale = transform.parent.localScale;
			if(transform.parent.transform.position.z > -2.63f)
			{
				newAsteroidBit.GetComponent<Renderer>().material.color = Color.gray;
			}


			newAsteroidBit = Instantiate(mPieces[mPieces.Length-(i+1)], transform.position, Quaternion.identity) as GameObject;
			newAsteroidBit.transform.localScale = transform.parent.localScale;
			newAsteroidBit.transform.localScale = transform.parent.localScale;
			if(transform.parent.transform.position.z > -2.63f)
			{
				newAsteroidBit.GetComponent<Renderer>().material.color = Color.gray;
			}
		}


		//float spawnChance = Random.Range(1,1000);
		//if(spawnChance == 777)
		if(PlayerPrefs.GetInt("AsteroidCount") >= PlayerPrefs.GetInt("AsteroidRequiredCount"))
		{
			if(PlayerPrefs.GetInt("SpawnLaserFist") == 0)
			{
				Instantiate(mBigBlastEmblem, new Vector3(transform.position.x, transform.position.y, -2f), Quaternion.identity);
				PlayerPrefs.SetInt("SpawnLaserFist",1);
			}
			else if(PlayerPrefs.GetInt("SpawnLaserFist") == 1)
			{
				Instantiate(mLaserFistEmblem, new Vector3(transform.position.x, transform.position.y, -2f), Quaternion.identity);
				PlayerPrefs.SetInt("SpawnLaserFist",0);
			}

			PlayerPrefs.SetInt("AsteroidRequiredCount", PlayerPrefs.GetInt("AsteroidRequiredCount") + Random.Range(175,250));

		}

		Destroy(transform.parent.gameObject);
	}
}