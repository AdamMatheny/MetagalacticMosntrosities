using UnityEngine;
using System.Collections;

public class CreditsCoOpController : MonoBehaviour 
{
	[SerializeField] private GameObject mPlayer2Ship;

	// Use this for initialization
	void Start () 
	{
		if(FindObjectOfType<ScoreManager>()!= null && FindObjectOfType<ScoreManager>().mP2Score >0)
		{
			mPlayer2Ship.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(FindObjectOfType<ScoreManager>()!= null && FindObjectOfType<ScoreManager>().mP2Score >0)
		{
			mPlayer2Ship.SetActive(true);
		}
	}
}
