using UnityEngine;
using System.Collections;

public class BossHitboxFollower : MonoBehaviour 
{
	GameObject mBoss;
	// Use this for initialization
	void Start () 
	{
		mBoss = FindObjectOfType<SkullBossController>().gameObject;
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{
		//Keep the hitboxes centered with the boss ~Adam
		//We're making the hitboxes as separate objects from the boss itself because Unity sometimes registers all hitboxes in a parent/child tree as one ~Adam
		if(mBoss != null)
		{
			transform.position = mBoss.transform.position;
		}
		else
		{
			mBoss = FindObjectOfType<SkullBossController>().gameObject;
		}
	}//END of Update()
}
