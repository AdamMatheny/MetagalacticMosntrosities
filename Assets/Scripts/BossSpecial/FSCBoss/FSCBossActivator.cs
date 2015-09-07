using UnityEngine;
using System.Collections;

public class FSCBossActivator : MonoBehaviour 
{
	[SerializeField] private float mBossStartTimer = 19f;
	[SerializeField] private GameObject mBoss;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		mBossStartTimer -= Time.deltaTime;
		if(mBossStartTimer <= 0f)
		{
			mBoss.SetActive (true);
		}
	}
}
