using UnityEngine;
using System.Collections;

public class BossBulletRotater : MonoBehaviour 
{
	public Vector3 mTargetRotation = Vector3.zero;
	public Vector3 newDir;
	public float mRotateSpeed = 10f;
	

	public bool mAutoRotate = false;

	HeroShipAI mTargetPlayer;
	Vector3 mTargetPlayerPosition;


	void Start()
	{

	}

	void Update()
	{
		if(!mAutoRotate)
		{
			mTargetRotation = new Vector3(Input.GetAxis ("RightAnalogHorizontal"), Input.GetAxis ("RightAnalogVertical"), 0);
			Vector3.Normalize (mTargetRotation);
			mTargetRotation = new Vector3(0f, 0f, Vector3.Angle(mTargetRotation, Vector3.down));
			if(Input.GetAxis ("RightAnalogHorizontal") < 0f)
			{
				mTargetRotation *= -1f;
			}
			if(Input.GetAxis ("RightAnalogHorizontal") !=0f || Input.GetAxis ("RightAnalogVertical") != 0f)
			{
		

				transform.rotation =Quaternion.Lerp (transform.rotation, Quaternion.Euler (mTargetRotation), 0.001f*mRotateSpeed);
			}
		}
		else
		{
			if(mTargetPlayer == null)
			{
				if(FindObjectOfType<HeroShipAI>() != null)
				{
					mTargetPlayer = FindObjectOfType<HeroShipAI>();
				}
			}
			else
			{

				mTargetPlayerPosition = mTargetPlayer.gameObject.transform.position;
				mTargetRotation = new Vector3(mTargetPlayerPosition.x-transform.position.x,mTargetPlayerPosition.y-transform.position.y,0f);
				//mTargetRotation += Vector3.up*3f;
			}
			
			Vector3.Normalize (mTargetRotation);
			mTargetRotation = new Vector3(0f, 0f, Vector3.Angle(mTargetRotation, Vector3.down));
			if(mTargetPlayerPosition.x < transform.position.x)
			{
				mTargetRotation *= -1f;
			}
			
			transform.rotation =Quaternion.Lerp (transform.rotation, Quaternion.Euler (mTargetRotation), 0.001f*mRotateSpeed * Time.timeScale);

		}
		//transform.rotation = new Quaternion(Input.GetAxis ("RightAnalogHorizontal"), Input.GetAxis ("RightAnalogVertical"), 0, 0);
	}
	
}
