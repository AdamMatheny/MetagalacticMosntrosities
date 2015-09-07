using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobileHideOnUnpause : MonoBehaviour 
{


	
	// Update is called once per frame
	void Update () 
	{
		//Hide the level info box and high score box when unpaused on mobile -Adam
		if(Application.isMobilePlatform)
		{
			if(Time.timeScale == 0)
			{
				GetComponent<Image>().enabled = true;
				if(GetComponentInChildren<Text>() != null)
				{
					GetComponentInChildren<Text>().enabled = true;
				}
			}
			else
			{
				GetComponent<Image>().enabled = false;
				if(GetComponentInChildren<Text>() != null)
				{
					GetComponentInChildren<Text>().enabled = false;
				}			
			}
		}
		//Turn this script off when not mobile
		else
		{
			this.enabled = false;
		}
	}//END of Update()
}
