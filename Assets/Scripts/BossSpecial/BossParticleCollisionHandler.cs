using UnityEngine;
using System.Collections;

public class BossParticleCollisionHandler : MonoBehaviour 
{
	ScoreManager mScoreController;
	// Use this for initialization
	void Start () 
	{
		mScoreController = FindObjectOfType<ScoreManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mScoreController == null)
		{
			mScoreController = FindObjectOfType<ScoreManager>();
		}
	}


	void OnParticleCollision(GameObject other)
	{
		Debug.Log("A particle hit " + other.name);
		if(other.tag == "Player")
		{
			Debug.Log("The player was shot by a particle");
			mScoreController.LoseALife();
		}

		if(other.GetComponent<PlayerBulletController>() != null)
		{
			Destroy(other.gameObject);

		}
	}
}
