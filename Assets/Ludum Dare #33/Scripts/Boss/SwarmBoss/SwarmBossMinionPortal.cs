using UnityEngine;
using System.Collections;

public class SwarmBossMinionPortal : MonoBehaviour 
{
	//Refrence to the main boss script ~Adam
	public BossGenericScript mBossCentral;

	//For toggling on and off ~Adam
	public GameObject mSpawnerHolder;


	//For moving around ~Adam
	public float mMoveSpeed = 15f;
	
	public float[] mBounds;
	public Rigidbody2D rgb2d;

	// Use this for initialization
	void Start () 
	{
		//Detach self from main boss object so this can move independently ~Adam
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Turn off when the boss is overheated or dying
		if(mBossCentral.mOverheated || mBossCentral.mDying)
		{
			mSpawnerHolder.SetActive (false);
			if(mBossCentral.mDying)
			{
				Destroy (this.gameObject);
			}
		}
		else
		{
			//Turn back on ~Adam
			mSpawnerHolder.SetActive (true);


			//Move Around ~Adam
			float horizontal = Input.GetAxis ("RightAnalogHorizontal");
			float vertical = Input.GetAxis ("RightAnalogVertical");
			rgb2d.velocity = new Vector2 (horizontal * mMoveSpeed, vertical * mMoveSpeed);
			
			
			
			//Keep the boss within the bounds of the screen ~Adam
			if(mBounds.Length >=4)
			{
				//X-Min ~Adam
				if(transform.position.x < mBounds[0])
				{
					transform.position = new Vector3(mBounds[0], transform.position.y, transform.position.z);
					
				}
				//X-Max ~Adam
				if (transform.position.x > mBounds[1])
				{
					transform.position = new Vector3(mBounds[1], transform.position.y, transform.position.z);
					
				}
				//Y-Min ~Adam
				
				if(transform.position.y < mBounds[2])
				{
					transform.position = new Vector3(transform.position.x, mBounds[2], transform.position.z);
					
				}
				//Y-Max ~Adam
				if (transform.position.y > mBounds[3])
				{
					transform.position = new Vector3(transform.position.x, mBounds[3], transform.position.z);
				}
			}
		}
	}
}
