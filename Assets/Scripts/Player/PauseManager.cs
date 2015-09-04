using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PauseManager : MonoBehaviour 
{

	[SerializeField] private GUIStyle mPauseButtonStyle;
	[SerializeField] private GUIStyle mPauseMenuStyle;

	[SerializeField] private Texture2D mContinueTex;
	[SerializeField] private Texture2D mContinueTexHighlight;
	[SerializeField] private Texture2D mReturnTex;
	[SerializeField] private Texture2D mReturnTexHighlight;
	[SerializeField] private Texture2D mOptionsTex;
	[SerializeField] private Texture2D mOptionsTexHighlight;

    [SerializeField] private UnityEngine.UI.Image PrePauseVigniette;
    [SerializeField] private UnityEngine.UI.Button PauseButton;

	List<string> mPauseMenuButtonNames = new List<string>();
	
	public int mPauseButtonFocus = 1;
	
	public float mUIFocusTimer = 0f;

    [HideInInspector] public bool isPaused = false;
	[HideInInspector] public bool isPrePaused = false;

    
	//For opening/closing the volume control menu ~Adam
	VolumeControlSliders mVolumeMenu;

	// Use this for initialization
	void Start () 
	{
		mPauseMenuButtonNames.Add("Pause");
		mPauseMenuButtonNames.Add("Continue");
		mPauseMenuButtonNames.Add("ReturnToMenu");
		mPauseMenuButtonNames.Add("Options"); //Changed to quit because, well, that's what it does. ~ Jonathan //Actually, it doesn't quit any more ~Adam
		mVolumeMenu = FindObjectOfType<VolumeControlSliders>();

	}
	
	// Update is called once per frame
	void Update () 
	{
//		if(Input.GetAxis ("Vertical") < 0)
//		{
//			Debug.Log("Down button pressed");
//		}
//		else if(Input.GetAxis ("Vertical") > 0)
//		{
//			Debug.Log("Up button pressed");
//		}
		Debug.Log ("time scale: " + Time.timeScale);
		if(mVolumeMenu == null)
		{
			mVolumeMenu = FindObjectOfType<VolumeControlSliders>();
		}

		if(mUIFocusTimer > 0f)
		{
			mUIFocusTimer -= 0.01f;
		}

		if (Time.timeScale == 1) 
		{

			//Cursor.visible = false;
		} 
		else if(Time.timeScale == 0 && (mVolumeMenu == null || !mVolumeMenu.mMenuOpen) )
		{

			//if(Input.GetButtonDown("Vertical"))
			//{
				//For using keyboard/Gamepad to navigate pause menu ~Adam
				if(mUIFocusTimer <= 0f)
				{
				if(Input.GetAxisRaw ("Vertical") < 0 || Input.GetAxisRaw ("VerticalP2") < 0 || InputManager.ActiveDevice.DPadDown.WasPressed)
					{
					Debug.Log("Down button pressed");
						switch(mPauseButtonFocus)
						{
						case 1:
							mPauseButtonFocus = 2;
							mUIFocusTimer = 0.2f;
							break;
						case 2:
							mPauseButtonFocus = 3;
							mUIFocusTimer = 0.2f;
							break;
						case 3:
							mPauseButtonFocus = 1;
							mUIFocusTimer = 0.2f;
						break;
						default:
							break;
						}
					}
				else if(Input.GetAxisRaw ("Vertical") > 0 || Input.GetAxisRaw ("VerticalP2") > 0 || InputManager.ActiveDevice.DPadUp.WasPressed)
					{
					Debug.Log("Up button pressed");
					switch(mPauseButtonFocus)
						{
						case 1:
							mPauseButtonFocus = 3;
							mUIFocusTimer = 0.2f;
							break;
						case 2:
							mPauseButtonFocus =1;
							mUIFocusTimer = 0.2f;
							break;
						case 3:
							mPauseButtonFocus =2;
							mUIFocusTimer = 0.2f;
							break;
						default:
							break;
						}
					}
				}
			//}
			
			
			if(Input.GetButtonDown("Thrusters") || Input.GetButtonDown("FireGun") || InputManager.ActiveDevice.Action1.WasPressed)
			{
				switch(mPauseButtonFocus)
				{
				case 1:
					UnPause();
					break;
				case 2:
					Time.timeScale = 1;
					if(FindObjectOfType<PlayerShipController>()!= null)
					{
						Destroy(FindObjectOfType<PlayerShipController>().gameObject);
					}
					if(FindObjectOfType<PlayerTwoShipController>()!= null)
					{
						Destroy(FindObjectOfType<PlayerTwoShipController>().gameObject);
					}
					if(FindObjectOfType<LevelKillCounter>()!= null)
					{
						Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
					}
					if(FindObjectOfType<ScoreManager>()!= null)
					{
						Destroy(FindObjectOfType<ScoreManager>().gameObject);
					}
					Application.LoadLevel(0);
					break;
				case 3:
					if(mVolumeMenu!=null && mUIFocusTimer <=0f)
					{
						mVolumeMenu.mMenuOpen = true;
					}
					break;
				default:
					break;
				}
			}
			//Cursor.visible = true;
		}

		//Press P to Pause/Unpause the game
		if (Input.GetButtonDown ("PauseButton") && (mVolumeMenu == null || !mVolumeMenu.mMenuOpen))// || InputManager.ActiveDevice.MenuWasPressed) 
		{
			
			if(Time.timeScale != 1)
			{
				
				UnPause();
			}
			else 
			{
				Pause();
			}
		}

#if UNITY_ANDROID
        if (Application.loadedLevel > 0 && Application.loadedLevel < 27 )
        {
            if (Input.touchCount == 0 && !isPrePaused)
            {
                PrePause();
            }
            if (Input.touchCount == 1 && (isPaused || isPrePaused))
            {
                UnPause();
            }
        }


#endif


	}


	void OnGUI()
	{
		mPauseMenuStyle.fontSize = Mathf.RoundToInt(Screen.width*0.01f);
		if(!Application.isMobilePlatform)
		{
			if (Time.timeScale == 0)
			{
				GUI.Box(new Rect(0,0, Screen.width,Screen.height),"");
				if(mVolumeMenu == null || !mVolumeMenu.mMenuOpen)
				{
					mPauseMenuStyle.normal.background = mContinueTex;
					mPauseMenuStyle.hover.background = mContinueTexHighlight;
					mPauseMenuStyle.active.background = mContinueTexHighlight;
					GUI.SetNextControlName("Continue");
					if (GUI.Button (new Rect (Screen.width*0.395f, Screen.height*0.21f, Screen.width*0.21f, Screen.height*0.14f), "", mPauseMenuStyle)) 
					{
						UnPause();
					}
					mPauseMenuStyle.normal.background = mReturnTex;
					mPauseMenuStyle.hover.background = mReturnTexHighlight;
					mPauseMenuStyle.active.background = mReturnTexHighlight;
					GUI.SetNextControlName("ReturnToMenu");
					if (GUI.Button (new Rect (Screen.width*0.395f, Screen.height*0.41f, Screen.width*0.21f, Screen.height*0.14f), "", mPauseMenuStyle)) 
					{
						Time.timeScale = 1;
						/*if(FindObjectOfType<PlayerTwoShipController>().gameObject != null)
						{
							Destroy(FindObjectOfType<PlayerTwoShipController>().gameObject);
						}*/ //Causing an error ~ Jonathan
						Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
						Destroy(FindObjectOfType<ScoreManager>().gameObject);
						Destroy(FindObjectOfType<PlayerShipController>().gameObject);
						Application.LoadLevel(0);
					}
					mPauseMenuStyle.normal.background = mOptionsTex;
					mPauseMenuStyle.hover.background = mOptionsTexHighlight;
					mPauseMenuStyle.active.background = mOptionsTexHighlight;
					GUI.SetNextControlName("Options");
					if (GUI.Button (new Rect (Screen.width*0.395f, Screen.height*0.61f, Screen.width*0.21f, Screen.height*0.14f), "", mPauseMenuStyle)) 
					{
						if(mVolumeMenu!=null && mUIFocusTimer <=0f)
						{
							mVolumeMenu.mMenuOpen = true;
						}
					}
	//				GUI.SetNextControlName("Pause");
	//				if (GUI.Button (new Rect (Screen.width * .81f, Screen.height * 0.890f, Screen.width * .09f, Screen.height * .1f), "", mPauseButtonStyle)) 
	//				{
	//					UnPause();
	//				}
				}
			}
	//		else
	//		{
	//			GUI.SetNextControlName("Pause");
	//			if (GUI.Button (new Rect (Screen.width * .91f, Screen.height * 0.890f, Screen.width * .09f, Screen.height * .1f), "", mPauseButtonStyle)) 
	//			{
	//				Pause();
	//			}
	//			//For when we had meters attached to the side
	////			GUI.SetNextControlName("Pause");
	////			if (GUI.Button (new Rect (Screen.width * .81f, Screen.height * 0.890f, Screen.width * .09f, Screen.height * .1f), "", mPauseButtonStyle)) 
	////			{
	////				Pause();
	////			}
	//		}



			GUI.FocusControl(mPauseMenuButtonNames[mPauseButtonFocus]);
		}

		//Change layout slightly for mobile portrait view ~Adam
		else
		{
			if (Time.timeScale == 0)
            {

                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
				if(mVolumeMenu == null || !mVolumeMenu.mMenuOpen)
				{
	                mPauseMenuStyle.normal.background = mContinueTex;
	                mPauseMenuStyle.hover.background = mContinueTexHighlight;
	                mPauseMenuStyle.active.background = mContinueTexHighlight;
	                if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.21f, Screen.width * 0.6f, Screen.height * 0.14f), "", mPauseMenuStyle))
	                {
	                    UnPause();
	                }
	                mPauseMenuStyle.normal.background = mReturnTex;
	                mPauseMenuStyle.hover.background = mReturnTexHighlight;
	                mPauseMenuStyle.active.background = mReturnTexHighlight;
	                if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.41f, Screen.width * 0.6f, Screen.height * 0.14f), "", mPauseMenuStyle))
	                {
	                    Time.timeScale = 1;
	                    Destroy(FindObjectOfType<PlayerShipController>().gameObject);
	                    Destroy(FindObjectOfType<LevelKillCounter>().gameObject);
	                    Destroy(FindObjectOfType<ScoreManager>().gameObject);
	                    Application.LoadLevel(0);
	                }
	                mPauseMenuStyle.normal.background = mOptionsTex;
	                mPauseMenuStyle.hover.background = mOptionsTexHighlight;
	                mPauseMenuStyle.active.background = mOptionsTexHighlight;
	                if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.61f, Screen.width * 0.6f, Screen.height * 0.14f), "", mPauseMenuStyle))
	                {
						if(mVolumeMenu!=null && mUIFocusTimer <=0f)
						{
							mVolumeMenu.mMenuOpen = true;
						}
	                }
				}
            }
		}
	}//END of OnGUI



	public void Pause()
	{
        StopAllCoroutines();
		Time.timeScale = 0;
		mPauseButtonFocus = 1;
        isPaused = true;
        isPrePaused = false;
	}//END of Pause()

    void PrePause()
    {
        isPrePaused = true;
        
        StopCoroutine("SpeedUpTime");
        StopCoroutine("HideVigniette");
        StartCoroutine("SlowTime");
        StartCoroutine("ShowVigniette");
    }
	
	void UnPause()
	{
        isPaused = false;
        isPrePaused = false;
		if(mVolumeMenu!=null)
		{
			mVolumeMenu.CloseVolumeMenu();
		}
        StopCoroutine("SlowTime");
        StopCoroutine("ShowVigniette");
        StartCoroutine("SpeedUpTime");
        StartCoroutine("HideVigniette");
	}//END of UnPause

    private IEnumerator SlowTime()
    {
        while (Time.timeScale > 0.3f)
        {
            Time.timeScale -= 0.1f;

            if (Time.time < 0.3f)
                Time.timeScale = 0.3f;

            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

    private IEnumerator SpeedUpTime()
    {
        while (Time.timeScale < 1)
        {
            Time.timeScale += 0.1f;

            if (Time.time > 1)
                Time.timeScale = 1;

            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

    private IEnumerator ShowVigniette()
    {
        PauseButton.interactable = true;
        while(PrePauseVigniette.color.a < 1)
        {
            if (PrePauseVigniette.color.a + 0.15f < 1)
                PrePauseVigniette.color = new Color(1, 1, 1, PrePauseVigniette.color.a + 0.15f);
            else
                PrePauseVigniette.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

    private IEnumerator HideVigniette()
    {
        while (PrePauseVigniette.color.a > 0)
        {
            if (PrePauseVigniette.color.a - 0.15f < 0)
                PrePauseVigniette.color = new Color(1, 1, 1, PrePauseVigniette.color.a - 0.15f);
            else
            {
                PrePauseVigniette.color = new Color(1, 1, 1, 0);
                PauseButton.interactable = false;
            }
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

}
