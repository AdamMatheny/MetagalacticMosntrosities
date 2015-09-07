using UnityEngine;
using System.Collections;

public class SwarmGridSlot : MonoBehaviour 
{
	public bool mOccupied;

	public Transform mFormationPosition;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mFormationPosition != null)
		{
			if(Vector3.Distance(transform.localPosition, mFormationPosition.localPosition) < 1f)
			{
				transform.localPosition = mFormationPosition.localPosition;

			}
			else
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, mFormationPosition.localPosition, Time.deltaTime);
			}

		}

	}
}
