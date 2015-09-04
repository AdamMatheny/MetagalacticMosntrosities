using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResetScore : MonoBehaviour 
{

	public bool canResetScore = false;

	//A GUI Style to make the font not look gross ~Adam
	[SerializeField] private GUIStyle mHighScoreStyle;
	[SerializeField] private GUIStyle mHighScoreDisplayStyle;
	//For asking the player if they're sure they want to reset the high score ~Adam
	public bool mAskingForConfirm = false;


	[SerializeField] private Texture2D mResetTexStart;
	[SerializeField] private Texture2D mResetTexHighlightStart;
	[SerializeField] private Texture2D mResetTexConfirm;
	[SerializeField] private Texture2D mResetTexHighlightConfirm;
	[SerializeField] private Texture2D mResetTexCancel;
	[SerializeField] private Texture2D mResetTexHighlightCancel;
	[SerializeField] private Texture2D mResetTexAsk;
	[SerializeField] private Texture2D mResetTexHighlightAsk;


	public MainMenuGUIFocusController mGUIFocusControl;

	//For using the new UI canvas for the image-based font ~Adam
	public Text mHighScoreUIText;

	void Update(){

		if (Input.GetKeyDown (KeyCode.Delete) && canResetScore) 
			ConfirmScoreReset ();
	}

	void OnGUI()
	{
		if(!Application.isMobilePlatform)
		{
			mHighScoreStyle.fontSize = Mathf.RoundToInt(Screen.width*0.01f);
			mHighScoreDisplayStyle.fontSize = Mathf.RoundToInt(Screen.width*0.01f);
	//		GUI.Box(new Rect (Screen.width * 0.01f, Screen.height * 0.01f, Screen.width * 0.12f, Screen.height * .065f), "High Score: " + PlayerPrefs.GetInt ("highscore", 0), mHighScoreDisplayStyle);
			mHighScoreUIText.text = "High Score: " + PlayerPrefs.GetInt ("highscore", 0);
			if(!mAskingForConfirm)
			{
				mHighScoreStyle.normal.background = mResetTexStart;
				mHighScoreStyle.hover.background = mResetTexHighlightStart;
				mHighScoreStyle.focused.background = mResetTexHighlightStart;
				GUI.SetNextControlName("ResetStart");

				if (GUI.Button (new Rect (Screen.width * 100f, Screen.height * 0.890f, Screen.width * .1f, Screen.height * .1f), "", mHighScoreStyle)) 
				{
					StartScoreReset();
				}

			}
			else
			{
				mHighScoreStyle.normal.background = mResetTexAsk;
				mHighScoreStyle.hover.background = mResetTexAsk;
				mHighScoreStyle.focused.background = mResetTexAsk;
				GUI.SetNextControlName("ResetAskText");
				GUI.Box(new Rect (Screen.width * 0.01f, Screen.height * 0.5f, Screen.width * 0.2f, Screen.height * 0.2f), "", mHighScoreStyle);
				mHighScoreStyle.normal.background = mResetTexConfirm;
				mHighScoreStyle.hover.background = mResetTexHighlightConfirm;
				mHighScoreStyle.focused.background = mResetTexHighlightConfirm;
				GUI.SetNextControlName("ResetConfirm");
				if (GUI.Button (new Rect (Screen.width * .05f, Screen.height * 0.65f, Screen.width * .05f, Screen.height * .05f), "", mHighScoreStyle)) 
				{
					ConfirmScoreReset();
				}

				mHighScoreStyle.normal.background = mResetTexCancel;
				mHighScoreStyle.hover.background = mResetTexHighlightCancel;
				mHighScoreStyle.focused.background = mResetTexHighlightCancel;
				GUI.SetNextControlName("ResetCancel");
				if (GUI.Button (new Rect (Screen.width * .125f, Screen.height * 0.65f, Screen.width * .05f, Screen.height * .05f), "", mHighScoreStyle))
				{
					CancelScoreReset();
				}



			}


			//GUI.FocusControl(mGUIFocusControl.mMainMenuButtonNames[mGUIFocusControl.mMainMenuButtonFocus]);
		}
	}//END of OnGUI()

	//Functions to be called by GUI buttons so we can fake pressing buttons with a Game Pad

	public void StartScoreReset()
	{
		mAskingForConfirm = true;
		
		mGUIFocusControl.mMainMenuButtonFocus = 4;
		mGUIFocusControl.mUIFocusTimer = 1f;
	}

	public void CancelScoreReset()
	{
		mAskingForConfirm = false;
		mGUIFocusControl.mMainMenuButtonFocus = 2;
	}
	
	public void ConfirmScoreReset()
	{
		PlayerPrefs.SetInt("highscore", 0);
		PlayerPrefs.SetInt("AsteroidCount", 0);
		PlayerPrefs.SetInt("AsteroidRequiredCount", 100);
		PlayerPrefs.SetInt("SpawnLaserFist",0);
		mAskingForConfirm = false;
		mGUIFocusControl.mMainMenuButtonFocus = 2;

	}
	
}
