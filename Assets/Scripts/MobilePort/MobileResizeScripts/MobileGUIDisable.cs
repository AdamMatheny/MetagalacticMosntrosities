using UnityEngine;
using System.Collections;

public class MobileGUIDisable : MonoBehaviour 
{
	[SerializeField] private bool mMobileUI = false;
	[SerializeField] private bool mNonMobileUI = true;
	// Use this for initialization
	void Start () 
	{
		if(Application.isMobilePlatform && !mMobileUI)
		{
			gameObject.SetActive(false);
		}
		if(!Application.isMobilePlatform && !mNonMobileUI)
		{
			gameObject.SetActive(false);
		}
	}
	

}
