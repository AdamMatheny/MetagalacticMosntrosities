using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeIconManager : MonoBehaviour 
{
	[SerializeField] private ScoreManager mScoreManager;
	[SerializeField] private int mIconNumber;


	// Use this for initialization
	void Start () 
	{
		mScoreManager = FindObjectOfType<ScoreManager>();

	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		if(mScoreManager != null)
		{
			if(mScoreManager.mLivesRemaining >= mIconNumber)
			{
				//GetComponent<Image>().enabled = true;
				GetComponent<Animator>().SetBool("LifeLost", false);
				//GetComponent<Animator>().Play("LifeIcon_Idle");
				if(GetComponent<Image>().enabled == false)
				{
					ToggleLifeIcon();
				}
			}
			else
			{
				//GetComponent<Image>().enabled = false;
				GetComponent<Animator>().SetBool("LifeLost", true);
			}
		}
		else
		{
			GetComponent<Image>().enabled = false;
			mScoreManager = FindObjectOfType<ScoreManager>();
		}

	}//END of Update()

	public void ToggleLifeIcon()
	{
		GetComponent<Image>().enabled = !GetComponent<Image>().enabled;
		if(GetComponent<Image>().enabled)
		{
			//GetComponent<Animator>().Play("LifeIcon_Idle");
		}
		else
		{
			GetComponent<Animator>().StopPlayback();
		}

		GetComponent<Animator>().enabled = GetComponent<Image>().enabled;
	}
}
