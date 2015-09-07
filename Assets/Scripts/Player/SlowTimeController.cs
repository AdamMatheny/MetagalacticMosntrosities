using UnityEngine;
using System.Collections;

public class SlowTimeController : MonoBehaviour 
{
	float mSlowTimeTimer = 0f; //how many FRAMES of slow time are left -Adam
	bool mSlowTimeActive = false;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mSlowTimeTimer <= 0f && mSlowTimeActive == true)
		{
			Time.timeScale = 1f;
			mSlowTimeActive = false;
		}
		else if (mSlowTimeActive)
		{
			mSlowTimeTimer -= 1f;
		}
	}

	//Slow down the time scale for a certain number of FRAMES/Update() calls
	public void SlowDownTime(float timeScaling, float slowDuration)
	{
		Time.timeScale = timeScaling;
		mSlowTimeTimer = slowDuration;
		mSlowTimeActive = true;
	}
}
