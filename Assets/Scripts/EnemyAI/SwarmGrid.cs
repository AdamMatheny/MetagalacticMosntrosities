using UnityEngine;
using System.Collections;

public class SwarmGrid : MonoBehaviour 
{
	//An array of all the GridSlot game object that this is going to have as children
	[SerializeField] private SwarmGridSlot[] mGridSlots;
	
	//The spot that the grid's hub moves around
	[SerializeField] private bool mOverrideFocusPoint = false;
	[SerializeField] private Vector3 mFocusPoint = new Vector3	(0f,0f,-2f);

	//How fast the swarm grid moves
	[SerializeField] private float mGridMoveSpeed = 3f;

	//Variables for moving the swarm up and down
	[SerializeField] private bool mMoveVeritcal = false;
	[SerializeField] private float mVerticalMoveDist = 5f;
	[SerializeField] private bool mMovingUp = false;

	//Variables for moving the swarm left and right
	[SerializeField] private bool mMoveHorizontal = false;
	[SerializeField] private float mHorizontaMoveDist = 5f;
	[SerializeField] private bool mMovingRight = false;

	//The direction the grid is going to be moving for circluar movement
	[SerializeField] private bool mMoveCirclular = true;
	[SerializeField] private Vector3 mGridMoveDirection = new Vector3(5, 1, 0);

	[SerializeField] private bool mUseMultipleFormations = false;
	[SerializeField] private float mFormationSwitchTimeDefault = 10f;
	private float mFormationSwitchTime;
	[SerializeField] private bool mLimitedFormationSwitches = false;
	[SerializeField] private int mMaxFormationSwitches = 100;
	[SerializeField] private int mFormationSwitchCount = 0;
	[SerializeField] private int mFormationUsed = -1;
	[SerializeField] private SwarmGrid[] mAlternateFormations;

	// Use this for initialization
	void Start () 
	{
		if (!mOverrideFocusPoint)
		{
			mFocusPoint = transform.position;
		}
		mFormationSwitchTime = mFormationSwitchTimeDefault;
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Moving Up and Down
		if(mMoveVeritcal)
		{
			if (transform.position.y >= mFocusPoint.y+mVerticalMoveDist)
			{
				mMovingUp = false;
			}
			else if (transform.position.y <= mFocusPoint.y-mVerticalMoveDist)
			{
				mMovingUp = true;
			}
			if(mMovingUp)
			{
				transform.position += Vector3.up*mGridMoveSpeed* Time.deltaTime;
			}
			else
			{
				transform.position += Vector3.up*mGridMoveSpeed*-1f* Time.deltaTime;
			}
		}

		//Moving Left and Right
		if(mMoveHorizontal)
		{
			if (transform.position.x >= mFocusPoint.x+mHorizontaMoveDist)
			{
				mMovingRight = false;
			}
			else if (transform.position.x <= mFocusPoint.x-mHorizontaMoveDist)
			{
				mMovingRight = true;
			}
			if(mMovingRight)
			{
				transform.position += Vector3.right*mGridMoveSpeed* Time.deltaTime;
			}
			else
			{
				transform.position += Vector3.right*mGridMoveSpeed*-1f* Time.deltaTime;
			}
		}

		//Moving around in a circle
		if(mMoveCirclular)
		{
			mGridMoveDirection += mFocusPoint - transform.position;
			mGridMoveDirection.Normalize();
			mGridMoveDirection *= mGridMoveSpeed;
			
			transform.position += (mGridMoveDirection * Time.deltaTime);
		}

		if(mUseMultipleFormations && mFormationUsed >= 0)
		{
			if(!mLimitedFormationSwitches || mMaxFormationSwitches > mFormationSwitchCount)
			{
				mFormationSwitchTime -= Time.deltaTime;
				if (mFormationSwitchTime <= 0)
				{
					mFormationUsed++;
					mFormationSwitchCount++;

					if(mFormationUsed >= mAlternateFormations.Length)
					{
						mFormationUsed = 0;
					}
					mFormationSwitchTime = mFormationSwitchTimeDefault;
				}
				if(mAlternateFormations.Length > 0 && mAlternateFormations[mFormationUsed] !=null)
				{
					ChangeFormation(mFormationUsed);
				}
			}
		}

	}//END of Update()


	//Finding the next empty position in the grid
	public GameObject GetGridPosition()
	{
		//Set to -1 to show that we haven't found a GridSlot yet
		int gridPosition = -1;

		//Loop through mGridSlots until you find an empty one
		for (int i = 0; i < mGridSlots.Length; i++)
		{
			//if we find an open GridSlot, set is occupied status to true and make that the gridPosition that we return
			if (mGridSlots[i] != null && !mGridSlots[i].mOccupied)
			{
				gridPosition = i;
				mGridSlots[i].mOccupied = true;
				break;
			}
		}

		//Return a gridPosition if there was a slot open
		if (gridPosition >= 0)
		{
			return mGridSlots[gridPosition].gameObject;
		}
		//Return null if there are no empty slots (Hopefully this won't break things)
		else
		{
			return null;
		}

	}//END of GetGridPosition()
	
	//For Changing the shape of the Swarm's formation
	public void ChangeFormation(int formationNumber)
	{
		for (int i = 0; i < mAlternateFormations[formationNumber].mGridSlots.Length; i++)
		{
			if (mGridSlots[i] != null && mAlternateFormations[formationNumber].mGridSlots[i] != null)
			{
				//Debug.Log(mGridSlots[i].transform.localPosition + " "+ mAlternateFormations[formationNumber].mGridSlots[i].transform.localPosition);
				mGridSlots[i].mFormationPosition = mAlternateFormations[formationNumber].mGridSlots[i].transform;
			}
		}

	}//END of ChangeFormation()
	
}
