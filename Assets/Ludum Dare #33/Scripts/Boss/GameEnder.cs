using UnityEngine;
using System.Collections;

public class GameEnder : MonoBehaviour 
{

	float mTimer = 5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		mTimer-= Time.deltaTime;
		if(mTimer<= Time.deltaTime)
		{
			if(FindObjectOfType<HeroShipAI>()!= null)
			{
				Destroy(FindObjectOfType<HeroShipAI>().gameObject);
			}
			if(FindObjectOfType<BossGenericScript>()!= null)
			{
				Destroy(FindObjectOfType<BossGenericScript>().gameObject);
			}
			Application.LoadLevel (0);
		}
	}
}
