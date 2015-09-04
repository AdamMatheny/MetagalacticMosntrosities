using UnityEngine;
using System.Collections;

//When special Grabber type enemies hit the player, they will steal a copy of the player's ship that then becomes hostile to the player, shooting back at them
//These ships are initially towed around by the enemy that captured them and then released to move back and forth on their own once the capturing ship is either destroyed or returns to its position in the swarm


public class CapturedShip : MonoBehaviour 
{
	[SerializeField] private GameObject mCloneShip; //The clone ship that gets spawned on death

	public bool mInTow = true;
	PlayerShipController mPlayer;
	public EnemyShipAI mGrabbingEnemy;

	[SerializeField] private GameObject mDeathEffect;
	[SerializeField] private GameObject mSecondShipSpawnEffect;
	[SerializeField] private GameObject mCapturedShipBullet;

	private float mMovementSpeed;

	private float mShootTimer;
	private float mShootTimerDefault;


	bool mMovingRight = false;

	// Use this for initialization
	void Start () 
	{
		mPlayer = FindObjectOfType<PlayerShipController>();
		if(mPlayer != null)
		{
			mPlayer.mShipStolen = true;
		}
		//Randomly decide the direction the ship will move upon being released
		mMovingRight = (Random.value < 0.5);

		//Base the movement speed off of the players movement speed
		mMovementSpeed = mPlayer.mMovementSpeed;

		//Like the player, the later the level, the faster the captured ship shoots.
		//mShootTimerDefault = 0.25f-(0.15f/25f*Application.loadedLevel);
		mShootTimerDefault = 5f;
		mShootTimer = mShootTimerDefault;

		if(FindObjectOfType<PlayerTwoShipController>()!= null)
		{
			Destroy (this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mPlayer == null)
		{
			mPlayer = FindObjectOfType<PlayerShipController>();
			if(mPlayer != null)
			{
				mPlayer.mShipStolen = true;
			}
		}
		//Being towed away by the grabbing enemy
		if(mGrabbingEnemy != null && mInTow == true)
		{
			transform.position = mGrabbingEnemy.mTowPoint.position;
		
			foreach (ParticleSystem shipTrail in GetComponentsInChildren<ParticleSystem>())
			{
				shipTrail.enableEmission = false;
			}
		}
		//Moving left and right once no longer being towed
		else
		{
			if (transform.position.z != -2)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, -2);
			}
			foreach (ParticleSystem shipTrail in GetComponentsInChildren<ParticleSystem>())
			{
				shipTrail.enableEmission = true;
			}

			//Staying within the bounds of the play space
			if (transform.position.x >= 19f)
			{
				mMovingRight = false;
			}
			else if (transform.position.x <= -19f)
			{
				mMovingRight = true;
			}
			if(transform.position.y < -30f)
			{
				transform.position = new Vector3(transform.position.x, -30f, transform.position.z);
			}
			if (transform.position.y > 23f)
			{
				transform.position = new Vector3(transform.position.x, 23, transform.position.z);
			}


			if(mMovingRight)
			{
				transform.position += Vector3.right*mMovementSpeed* Time.deltaTime;
			}
			else
			{
				transform.position += Vector3.right*mMovementSpeed*-1f* Time.deltaTime;
			}
		}

		//Shooting back at the player
		mShootTimer -= Time.deltaTime;
		//Instantiate an enemy bullet if the player is below this enemy
		if(Mathf.Abs(mPlayer.transform.position.x - transform.position.x) <= 2f  && mShootTimer <=0f && mCapturedShipBullet != null)
		{
			GameObject enemyBullet;
			enemyBullet = Instantiate(mCapturedShipBullet, transform.position, Quaternion.identity) as GameObject;
			mShootTimer = mShootTimerDefault;
		}

	}//END of Update()

	void OnCollisionEnter(Collision other)
	{

		//Get destroyed when colliding with a bullet -Adam
		//Invulnerable while still being towed around -Adam
		//Upon destruction, the player is awarded with a second ship that stays attached to their main ship, enabling them to fire double the amount of projectiles, and is destroyed in the place of losing a life the next time the player is hit -Ada,
		if (other.gameObject.tag == "Player Bullet" && (!mInTow || mGrabbingEnemy == null || !mGrabbingEnemy.mInvincible) )
		{
			//Create a particle death effect -Adam
			Instantiate(mDeathEffect, transform.position, Quaternion.identity);
			//Destroy the bullet that hit the captured ship -ADam
			Destroy(other.gameObject);

			//Give the player a second ship, which can in turn be stolen back by grabber enemies -Adam
			mPlayer.mShipStolen = false;
			mPlayer.mShipRecovered = true;
			//Give a ship attached to the side of the main ship and make an effect as it spawns -Adam
			Instantiate(mSecondShipSpawnEffect, mPlayer.mSecondShip.transform.position, Quaternion.identity);

			//other.gameObject.transform.rotation = new Quaternion(0, 0, 180, 0);

			#region for twin-stick clone spawning
//			//If not mobile, spawn a clone ship next to the player and make an effect as it spawns -Adam
//			else
//			{
//				GameObject newClone = Instantiate(mCloneShip,mPlayer.transform.position + Vector3.right*2f, Quaternion.identity) as GameObject;
//				newClone.GetComponent<PlayerShipCloneController>().mOriginalShip = mPlayer.GetComponent<PlayerShipController>();
//				Instantiate(mSecondShipSpawnEffect, newClone.transform.position, Quaternion.identity);
//				mPlayer.mPlayerClone = newClone;
//			}
			#endregion
			Destroy(this.gameObject);

		}
	}

}
