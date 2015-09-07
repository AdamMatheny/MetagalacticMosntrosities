using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using XInputDotNetPure; // Required in C#

public class GetSome : MonoBehaviour 
{

//	public Sprite mGetSomeSprite1;
//	public Sprite mGetSomeSprite2;

	public GameObject mSuperLaser;
	public GameObject mCoOpLaser;

	public Texture2D mBlueCoinText;
	public Texture2D mBlueCoinTextBig;
	public Texture2D mPinkCoinText;
	public Texture2D mPinkCoinTextBig;

	float mTextColorTimer = 0f;

	[SerializeField] private GUIStyle mGetSomeStyle;
	[SerializeField] private GUIStyle mButtonStyle;
	[SerializeField] private GUIStyle mCoOpStyle;
	[SerializeField] private GUIStyle mShowVolumeMenuStyle;
	//For having a delay to destroy enemies when we start the game ~Adam
	float mGameStartTimer = 3f;
	bool mStartingGame = false;


	//A an object holding other objects that we want to delete ~Adam
	public GameObject mInactiveObjects;



	public MainMenuGUIFocusController mGUIFocusControl;

	//For opening/closing the volume control menu ~Adam
	VolumeControlSliders mVolumeMenu;
	//public float timer = .3f;

	//For showing the controls screen as a "loading screen" during startup ~Adam
	[SerializeField] private GameObject mControlsScreen;
	void Start()
	{
		GetComponent<Renderer>().material.color = new Color(0f,0f,0f,0f);
//		gameObject.GetComponent<SpriteRenderer> ().sprite = mGetSomeSprite1;
//		transform.position = new Vector3(-0.3f, -34f, -16.5f);
		AudioListener.volume =  1f;
		mVolumeMenu = FindObjectOfType<VolumeControlSliders>();
	}

	void Update()
	{
		//Wait for enemies to blow up before actually starting the game
		if (mStartingGame)
		{
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, new Color(0f,0f,0f,1f), 0.04f);

			mGameStartTimer-=Time.deltaTime;


			if(mGameStartTimer <= 0f)
			{

				mControlsScreen.SetActive (true);

				if(Input.anyKeyDown || InputManager.ActiveDevice.AnyButton.IsPressed)
				{
					Time.timeScale = 1f;
					Application.LoadLevel(1);
				}
			}
		}

	

		//Can press Esc to quit game
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.Quit();
		}

		//Press Space or fire toggle button on controller to start the game
//		if( ((Input.GetButtonDown("Thrusters")) || (Input.GetButtonDown("FireGun")))&& mGUIFocusControl.mMainMenuButtonFocus == 0)
//		{
//			mSuperLaser.SetActive(true);
//			//StartGame();
//		}

		if(Input.GetButtonDown("PauseButton")&& !mVolumeMenu.mMenuOpen)
		{
			if(mGUIFocusControl.mMainMenuButtonFocus == 2 || InputManager.ActiveDevice.Meta == "XInput Controller #1")
			{
				Destroy (mSuperLaser);
				FindObjectOfType<CoOpSelector>().mCoOpEnabled = true;
				mCoOpLaser.SetActive(true);
			}

			else
			{
				mSuperLaser.SetActive(true);
			}
		}

	}//END of Update()

	void OnGUI()
	{

		//This is from when we used to flash the color for "Insert Coin"
//		mTextColorTimer+= Time.deltaTime;
//		if(mTextColorTimer >= 1.5f)
//		{
//			mTextColorTimer = 0f;
//			mGetSomeStyle.normal.background = mBlueCoinText;
//			mGetSomeStyle.hover.background = mBlueCoinTextBig;
//			mGetSomeStyle.active.background = mBlueCoinTextBig;
//			mGetSomeStyle.focused.background = mBlueCoinTextBig;
//		}
//		else if(mTextColorTimer >= 0.75f)
//		{
//			mGetSomeStyle.normal.background = mPinkCoinText;
//			mGetSomeStyle.hover.background = mPinkCoinTextBig;
//			mGetSomeStyle.active.background = mPinkCoinTextBig;
//			mGetSomeStyle.focused.background = mPinkCoinTextBig;
//		}

		if(!mStartingGame)
		{

			//Start game button

			if(Application.isMobilePlatform)
			{
				GUI.SetNextControlName("InsertCoin");
				if(GUI.Button(new Rect(Screen.width *0.3f, Screen.height*0.75f, Screen.width*0.4f, Screen.width*0.054f), "",mGetSomeStyle))
				{
					mSuperLaser.SetActive(true);
					//StartGame();
				}
			}
			else
			{
				GUI.SetNextControlName("InsertCoin");
				if(GUI.Button(new Rect(Screen.width *0.3f, Screen.height*0.75f, Screen.width*0.4f, Screen.width*0.054f), "",mGetSomeStyle))
				{
					mSuperLaser.SetActive(true);
					//StartGame();
				}
			}


			//Quit Game button ~Adam
			//mButtonStyle.fontSize = Mathf.RoundToInt(Screen.width*0.01f);
			if(!Application.isMobilePlatform)
			{
				GUI.SetNextControlName("QuitGame");
				if (GUI.Button (new Rect (Screen.width * .85f, Screen.height * 0.890f, Screen.width * .1f, Screen.height * .1f), "", mButtonStyle)) 
				{
					Application.Quit();
				}
			}
			else
			{
				GUI.SetNextControlName("QuitGame");
				if (GUI.Button (new Rect (Screen.width * .95f, Screen.height * 0.870f, Screen.width * .4f, Screen.height * .115f), "", mButtonStyle)) 
				{
					Application.Quit();
				}			
			}
			//Button to Start CoOp Mode ~Adam
			if(!Application.isMobilePlatform)
			{
				GUI.SetNextControlName("StartCoOp");
				if(GUI.Button(new Rect(Screen.width *0.3f, Screen.height*0.89f, Screen.width*0.4f, Screen.width*0.054f), "",mCoOpStyle))
				{
					Destroy (mSuperLaser);
					FindObjectOfType<CoOpSelector>().mCoOpEnabled = true;
					mCoOpLaser.SetActive(true);
					//StartGame();
				}			
			}

			//Volume Menu button ~Adam
			//mButtonStyle.fontSize = Mathf.RoundToInt(Screen.width*0.01f);
			if(!Application.isMobilePlatform)
			{
				/*GUI.SetNextControlName("Options");
				if (GUI.Button (new Rect (Screen.width * .85f, Screen.height * 0.760f, Screen.width * .1f, Screen.height * .1f), "Options", mShowVolumeMenuStyle)) 
				{
					mVolumeMenu.mMenuOpen = true;
				}*/ //Old Position
				GUI.SetNextControlName("Options");
				if (GUI.Button (new Rect (Screen.width * 0.05f, Screen.height * 0.890f, Screen.width * .1f, Screen.height * .1f), "", mShowVolumeMenuStyle)) 
				{
					mVolumeMenu.mMenuOpen = true;
				}
			}
			else
			{
				GUI.SetNextControlName("Options");
				if (GUI.Button (new Rect (Screen.width * .59f, Screen.height * 0.750f, Screen.width * .41f, Screen.height * .115f), "", mShowVolumeMenuStyle)) 
				{
					mVolumeMenu.mMenuOpen = true;
				}			
			}
		}

		GUI.FocusControl(mGUIFocusControl.mMainMenuButtonNames[mGUIFocusControl.mMainMenuButtonFocus]);

	}//END of OnGUI()


	public void StartGame()
	{
		GetComponent<Renderer>().enabled = true;
//		if(mSuperLaser != null)
//		{
//			mSuperLaser.SetActive(true);
//		}
		//Time.timeScale = 0.25f;
		EnemyShipAI[] startScreenShips = FindObjectsOfType<EnemyShipAI>();

		foreach(EnemyShipAI startShip in startScreenShips)
		{
			startShip.EnemyShipDie();
		}
		FindObjectOfType<ResetScore>().enabled = false;
		GameObject.Find("MainMenuHighScoreCanvas").gameObject.SetActive (false);
		Destroy(FindObjectOfType<PlayerShipController>().gameObject);
		Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
		Destroy(FindObjectOfType<ScoreManager>().gameObject);
		mStartingGame = true;
	

	}//END of StartGame();
}