using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobileLifeMeter : MonoBehaviour 
{
	[SerializeField] private Image mLifeBar;
	ScoreManager mScoreManager;

	// Use this for initialization
	void Start () 
	{
		//Find the player ship -Adam
		mScoreManager = FindObjectOfType<ScoreManager>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mScoreManager == null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>();
		}
		else if(mLifeBar.canvas.isActiveAndEnabled) //Only do stuff when the canvas is actually turned on
		{

			mLifeBar.GetComponent<RectTransform>().localScale = new Vector3((float)mScoreManager.mLivesRemaining/mScoreManager.mMaxLives, 1, 1f);

		}
	}
}
