using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//For updating score/HighScore during the credits from blowing up credits blocks

public class CreditsScoreUpdater : MonoBehaviour 
{
	[SerializeField] private ScoreManager mScoreManager;

	public int mScore = 0;

	[SerializeField] private Text mScoreMeterText;
	[SerializeField] private Text mHighScoreText;


	// Use this for initialization
	void Start () 
	{
		if(FindObjectOfType<ScoreManager>() != null)
		{
			mScoreManager = FindObjectOfType<ScoreManager>();
			mScore = mScoreManager.mScore;
			mHighScoreText = mScoreManager.mHighScoreText;
			mScoreMeterText = mScoreManager.mPowerUpMeterScoreDisplay;

		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mHighScoreText != null)
		{
			mHighScoreText.text = "High Score:\n" + PlayerPrefs.GetInt("highscore", 0);
		}
		if(mScoreMeterText!= null)
		{
			if(mScore < 0)
			{
				mScoreMeterText.text = "Loser. Try Shooting.";
			}
			else
			{
				
				mScoreMeterText.text = "Score: " + mScore;

			}
		}
		StoreHighScore(mScore);
	}

	void StoreHighScore(int newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
		if(newHighscore > oldHighscore)
			PlayerPrefs.SetInt("highscore", newHighscore);
	}

}
