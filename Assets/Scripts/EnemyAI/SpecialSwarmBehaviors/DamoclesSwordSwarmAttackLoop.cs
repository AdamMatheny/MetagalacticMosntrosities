using UnityEngine;
using System.Collections;

public class DamoclesSwordSwarmAttackLoop : MonoBehaviour 
{
	[SerializeField] private float mStateTimer = -10f;
	//Timer value states: ~Adam
//		/*
//		 * <=20: Idle
//		 * 21-22: Drop
//		 * 23-24: Sweep left
//		 * 24-26: Sweep right
//		 * 26-28: Swing up to center
//		 * 28-30: Spin and lift to start
//		 * >30: Reset
//		 */

	//Positions and rotations to move to as the sword swings around ~Adam
	[SerializeField] private Vector3 mIdlePos;
	[SerializeField] private Vector3 mIdleRot;

	[SerializeField] private Vector3 mDropPos;
	[SerializeField] private Vector3 mLeftSweepPos;
	[SerializeField] private Vector3 mRightSweepPos;

	[SerializeField] private Vector3 mSwingUpPos;
	[SerializeField] private Vector3 mSwingUpRot;


	
	// Update is called once per frame
	void Update () 
	{
		mStateTimer += Time.deltaTime;


		#region Changing attack state based on timer
		//>30: Reset ~Adam
		if(mStateTimer > 30f)
		{
			mStateTimer = 0f;
		}
		//28-30: Spin and lift to start ~Adam
		else if (mStateTimer > 28f)
		{
			transform.position = Vector3.Lerp(transform.position, mIdlePos, 0.05f);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler (transform.localRotation.eulerAngles+ Vector3.back*40f), 0.05f);
		}
		//26-28: Swing up to center ~Adam
		else if (mStateTimer > 26f)
		{
			transform.position = Vector3.Lerp(transform.position, mSwingUpPos, 0.1f);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler (mSwingUpRot), 0.1f);
		}
		//24-26: Sweep right ~Adam
		else if (mStateTimer > 24f)
		{
			transform.position = Vector3.Lerp(transform.position, mRightSweepPos, 0.05f);
		}
		//23-24: Sweep left ~Adam
		else if (mStateTimer > 22f)
		{
			transform.position = Vector3.Lerp(transform.position, mLeftSweepPos, 0.05f);

		}
		//20-22: Drop ~Adam
		else if (mStateTimer > 20f)
		{
			transform.position = Vector3.Lerp(transform.position, mDropPos, 0.1f);
		}
		// <=20: Idle ~Adam
		else
		{
			transform.position = mIdlePos;
			//transform.localRotation = Quaternion.Euler (mIdleRot);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler (mIdleRot), 0.1f);
		}
		#endregion


		//Keep swarm grid slots all facing the same way ~Adam
		foreach(SwarmGridSlot gridSlot in FindObjectsOfType<SwarmGridSlot>())
		{
			gridSlot.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
		}
	}//END of Update()
}
