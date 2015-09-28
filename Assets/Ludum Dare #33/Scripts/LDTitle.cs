using UnityEngine;
using System.Collections;
using InControl;

public class LDTitle : MonoBehaviour 
{

	float mTimer = 0f;
	[SerializeField] private float mThreshhold = 0f;
	
	// Update is called once per frame
	void Update () 
	{
		mTimer += Time.deltaTime;
		if(mTimer > mThreshhold && (Input.anyKey || InputManager.ActiveDevice.AnyButton.WasPressed) )
		{
			Application.LoadLevel(Application.loadedLevel+1);
		}
	}
}
