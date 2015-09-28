using UnityEngine;
using System.Collections;

//For making the final boss death laser rotate seperately from the boss but still stay aligned with it ~Adam

public class Boss5DeathLaserFollow : MonoBehaviour 
{
	[SerializeField] private Transform mFollowPoint;
	[SerializeField] private BossGenericScript mBossCentral;
	// Use this for initialization
	void Start () 
	{
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(mFollowPoint != null)
		{
			transform.position = mFollowPoint.position;
		}

		else
		{
			Destroy (this.gameObject);
		}
		if(mBossCentral == null || mBossCentral.mDying)
		{
			Destroy (this.gameObject);
		}
	}
}
