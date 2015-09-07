using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsBlock : MonoBehaviour 
{
	public string mCreditText;
	[SerializeField] private GUIStyle mCreditBlockStyle;
	float mHorzSize =0.1f;
	float mVertSize = 0.05f;
	float mFontSizeMod = 1f;
	[SerializeField] private GameObject mExplosion;

	[SerializeField] private Text mCanvasText;

	// Use this for initialization
	void Start () 
	{
	
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Drift upwards `Adam
		transform.Translate(0f,0.075f,0f);
		//Explode if big and red enough `Adam
		if(mHorzSize > 0.2f)
		{
			Instantiate(mExplosion,transform.position,Quaternion.identity);
			if(FindObjectOfType<CreditsScoreUpdater>() != null)
			{
				FindObjectOfType<CreditsScoreUpdater>().mScore+=10;
			}
			Destroy(this.gameObject);
		}

		mCanvasText.text = mCreditText;
	}//END of Update()

//	void OnGUI()
//	{
//		Vector3 creditsScreenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
//		mCreditBlockStyle.fontSize = Mathf.RoundToInt(Screen.height*0.02f*mFontSizeMod);
//		GUI.Box(new Rect(creditsScreenPos.x-(Screen.height*mHorzSize*0.5f), Screen.height*0.98f-creditsScreenPos.y, Screen.height*mHorzSize, Screen.width*mVertSize), mCreditText, mCreditBlockStyle); 
//
//	}//END of OnGUI()

	void OnTriggerEnter(Collider other)
	{
		//Get bigger and redder when shot `Adam
		if(other.GetComponent<PlayerBulletController>() != null && ! other.GetComponent<PlayerBulletController>().mSideBullet)
		{
			mHorzSize+=0.002f;
			mVertSize+=0.001f;
			mFontSizeMod+=0.02f;
			mCanvasText.color = Color.Lerp(mCanvasText.color, Color.red, 0.02f);
			transform.localScale += new Vector3(0.002f,0.002f,0.002f);
			mCanvasText.GetComponent<RectTransform>().localScale += new Vector3(0.00002f,0.0002f,0.0002f);
		}
	}//END of OnTriggerEnter()
}
