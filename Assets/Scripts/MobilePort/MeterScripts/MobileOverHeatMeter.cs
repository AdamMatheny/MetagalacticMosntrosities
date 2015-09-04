using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobileOverHeatMeter : MonoBehaviour 
{

	[SerializeField] private Image mOverHeatBar;
	[SerializeField] private Image mOverHeatOverlay;
	PlayerShipController mPlayer;

	// Use this for initialization
	void Start () 
	{
		//Find the player ship -Adam
		mPlayer = FindObjectOfType<PlayerShipController>();
	}		

	
	// Update is called once per frame
	void Update () 
	{
		//Safety in case the player ship connection is lost -Adam
		if(mPlayer == null)
		{
			mPlayer = FindObjectOfType<PlayerShipController>();
		}

		else if(mOverHeatBar.canvas.isActiveAndEnabled) //Only do stuff when the canvas is actually turned on
		{
			//Make the bar move up and down
			mOverHeatBar.GetComponent<RectTransform>().localScale = new Vector3(mPlayer.heatLevel/mPlayer.maxHeatLevel, 1, 1f);
			
			//Display overlay when overheated
			if(mPlayer.isOverheated)
			{
				mOverHeatOverlay.enabled = true;
			}
			else
			{
				mOverHeatOverlay.enabled = false;
			}
			
		}
	}
}
