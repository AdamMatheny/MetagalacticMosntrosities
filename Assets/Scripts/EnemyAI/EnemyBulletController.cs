using UnityEngine;
using System.Collections;

public class EnemyBulletController : MonoBehaviour 
{
	
	public GameObject mPlayer = null;
	public GameObject mPlayerClone = null; //For co-op mode ~Adam
	public float mBulletSpeed = 20.0f;
	protected float mSelfDestructTimer = 5.0f;
	private ScoreManager mScoreController;
	public bool mShootable;
	public bool mAimAtPlayer = false;
	public bool mFixedFireDir = false;
	public bool mMoveTowardsPlayer = false;
	public Vector3 mFireDir;

	public Vector2 bulletForce;

	public GameObject bulletExplosion;

	public void Start()
	{
		mPlayer = FindObjectOfType<PlayerShipController>().gameObject;
		mScoreController = FindObjectOfType<ScoreManager>();
		#region co-op mode stuff
		if(FindObjectOfType<PlayerTwoShipController>() != null)
		{
			mPlayerClone = FindObjectOfType<PlayerTwoShipController>().gameObject;
		}
		#endregion
		//Vector2 bulletForce;

		//Used for firing in a particular pattern (i.e. rotational pattern on boss horns)~Adam
		if (mFixedFireDir) 
		{
			bulletForce = mFireDir * mBulletSpeed;
			//transform.rotation = Quaternion.Euler(new Vector3(90f,0f,0f) + transform.rotation.eulerAngles);
		}
		//Used for aiming at the player ~Adam
		else if (mAimAtPlayer) 
		{
			Vector3 directionToPlayer = Vector3.down;
			#region twin-stick clone stuff
			//Fire at the clone ship if it is both present and closer -Adam
			if (mPlayerClone != null && Vector3.Distance (transform.position, mPlayerClone.transform.position) <= Vector3.Distance (transform.position, mPlayer.transform.position)) 
			{
				directionToPlayer = mPlayerClone.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayerClone.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
			#endregion
			else 
			{
				//fire at the player
				directionToPlayer = mPlayer.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayer.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
		} 
		//For constantly tracking/homing in on the player
		else if (mMoveTowardsPlayer) 
		{
			Vector3 directionToPlayer = Vector3.down;
			#region twin-stick clone stuff
			//Fire at the clone ship if it is both present and closer -Adam
			if (mPlayerClone != null && Vector3.Distance (transform.position, mPlayerClone.transform.position) <= Vector3.Distance (transform.position, mPlayer.transform.position)) 
			{
				directionToPlayer = mPlayerClone.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayerClone.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
			#endregion
			else 
			{
				//fire at the player
				directionToPlayer = mPlayer.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayer.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
		}
		//Just fire up and down ~Adam
		else
		{
			#region twin-stick clone stuff
			if(mPlayerClone != null && Vector3.Distance(transform.position,mPlayerClone.transform.position) <= Vector3.Distance(transform.position,mPlayer.transform.position) )
			{
				if (mPlayerClone.transform.position.y > transform.position.y)
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed);
				}
				else
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed * -1.0f);
				}
			}
			#endregion
			else
			{
				//Fire up/down
				if (mPlayer.transform.position.y > transform.position.y)
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed);
				}
				else
				{
					bulletForce = new Vector2(0.0f,mBulletSpeed * -1.0f);
				}

			}
		}

	
		GetComponent<Rigidbody> ().velocity = bulletForce;



		
	}
	
	public virtual void Update()
	{
		mSelfDestructTimer -= Time.deltaTime;
		if (mMoveTowardsPlayer) 
		{

//			Vector3 directionToPlayer = Vector3.down;
//
//			directionToPlayer = mPlayer.transform.position - transform.position;
//			bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
//			transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
//			GetComponent<Rigidbody> ().velocity = bulletForce;
//
//			transform.LookAt (mPlayer.transform.position);
//
//			transform.rotation = new Quaternion (0, 0, transform.rotation.z, 0);
			Vector3 directionToPlayer = Vector3.down;
			#region twin-stick clone stuff
			//Fire at the clone ship if it is both present and closer -Adam
			if (mPlayerClone != null && Vector3.Distance (transform.position, mPlayerClone.transform.position) <= Vector3.Distance (transform.position, mPlayer.transform.position)) 
			{
				directionToPlayer = mPlayerClone.transform.position - transform.position;
				bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
				transform.LookAt (mPlayerClone.transform.position);
				transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
			}
			#endregion
			else 
			{
				//fire at the player
				if(mPlayer != null)
				{
					directionToPlayer = mPlayer.transform.position - transform.position;
					bulletForce = Vector3.Normalize (directionToPlayer) * mBulletSpeed;
					transform.LookAt (mPlayer.transform.position);
					transform.rotation = Quaternion.Euler (new Vector3 (90f, 0f, 0f) + transform.rotation.eulerAngles);
				}
			}
			GetComponent<Rigidbody> ().velocity = bulletForce;

		}

		//Self-destruct after a certain amount of time
		if(mSelfDestructTimer<0.0f)
		{
			if(mMoveTowardsPlayer)
			{
				if(bulletExplosion != null)
				{
					
					Instantiate(bulletExplosion, transform.position, Quaternion.identity);
				}
			}

			Destroy(gameObject);
		}

		//Detect distance to player and slow down time if close but not quite hitting ~Adam
		if (Vector3.Distance(this.transform.position, mPlayer.transform.position) <= 2.5f)
		{
			if(FindObjectOfType<SlowTimeController>()!= null)
			{
				FindObjectOfType<SlowTimeController>().SlowDownTime(0.4f,1f);
			}
		}

		//Detect distance to player and kill the player and destroy self if close enough to "touch" ~Adam
		if (Vector3.Distance(this.transform.position, mPlayer.transform.position) <= 1.5f)
		{

			Debug.Log("The player was shot");
			if(mScoreController != null && mScoreController.enabled == true)
			{
				mScoreController.LoseALife();
				Destroy(gameObject);
			}
		}

		//If second ship is activated, extend the slow time down ~ Jonathan
		//Also kill the ship FINALLY!!! ~ Jonathan
		if (mPlayer.GetComponent<PlayerShipController> ().mShipRecovered) {

			if(Vector3.Distance(this.transform.position, mPlayer.GetComponent<PlayerShipController> ().mSecondShip.transform.position) <= 2.5f){

				if(FindObjectOfType<SlowTimeController>()!= null)
				{
					FindObjectOfType<SlowTimeController>().SlowDownTime(0.4f,1f);
				}
			}

			if(Vector3.Distance(this.transform.position, mPlayer.GetComponent<PlayerShipController> ().mSecondShip.transform.position) <= 1.5f){

				Debug.Log("The second ship was shot");
				mScoreController.LoseALife();
				Destroy(gameObject);
			}
		}

		#region twin-stick clone stuff
		//Detect distance to player clone and kill the clone and destroy self if close enough to "touch" ~Adam
		if(mPlayerClone != null)
		{
			if (Vector3.Distance(this.transform.position, mPlayerClone.transform.position) <= 1.5f)
			{
				Debug.Log("The clone was shot");
				mScoreController.LosePlayerTwoLife();
				Destroy(gameObject);
			}
		}
		#endregion
	}//END of Update()

	void OnTriggerEnter (Collider other)
	{

		/*if (other.tag == "Enemy" && mMoveTowardsPlayer) 
			other.GetComponent<EnemyShipAI> ().EnemyShipDie ();*/ //Enemy ship doesn't have OnTrigger

		if(other.tag == "Player Bullet")
		{
			if(mShootable)
			{

				if(bulletExplosion != null)
				{

					Instantiate(bulletExplosion, transform.position, Quaternion.identity);
				}

				Destroy(other.gameObject);
				Destroy(this.gameObject);
			}
		}

	}//END of OnTriggerEnter()



	void OnCollisionEnter (Collision other)
	{

		if (other.gameObject.tag == "Enemy" && mMoveTowardsPlayer && (other.gameObject.GetComponent<EnemyShipAI>()!= null && other.gameObject.GetComponent<EnemyShipAI>().mGrabber == false)) 
		{
			if (mPlayerClone != null && Vector3.Distance (transform.position, mPlayerClone.transform.position) <= Vector3.Distance (transform.position, mPlayer.transform.position)) 
			{
				other.gameObject.GetComponent<EnemyShipAI>().mKillerNumber = 2;
			}
			else
			{
				other.gameObject.GetComponent<EnemyShipAI>().mKillerNumber = 1;
			}


			other.gameObject.GetComponent<EnemyShipAI>().EnemyShipDie ();
		}
	}



}