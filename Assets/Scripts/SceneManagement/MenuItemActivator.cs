using UnityEngine;
using System.Collections;

public class MenuItemActivator : MonoBehaviour 
{
	public GameObject mMenuUIController;

	public float mActivationTimer = 0f;


	bool mUiActivated = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		mActivationTimer += Time.deltaTime;

			transform.position = Vector3.Lerp(transform.position, new Vector3(0,40f,-2f), 0.01f);
		if(!Application.isMobilePlatform)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(9f,9f,9f), 0.01f);
		}
		else
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(6.9f,6.9f,6.9f), 0.01f);
		}
		if(mActivationTimer > 8.25f && !mUiActivated)
		{
			mMenuUIController.SetActive(true);
			mUiActivated = true;
		}

	}
}
