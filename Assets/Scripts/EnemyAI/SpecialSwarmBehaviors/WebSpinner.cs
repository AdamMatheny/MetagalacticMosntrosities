using UnityEngine;
using System.Collections;

//For continuously rotating an entire set of swarms ~Adam

public class WebSpinner : MonoBehaviour 
{
	[SerializeField] private  float mSpinDir = 10f;

	[SerializeField] private float mSpinTimer = 0f;

	[SerializeField] private float mReverseTime = 20f;

	// Use this for initialization
	void Start () 
	{
		mReverseTime = Random.Range(10,40);
	}
	
	// Update is called once per frame
	void Update () 
	{
		mSpinTimer += Time.deltaTime;

		//Spin the Web ~Adam
		if(mSpinTimer >= 20f)
		{
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler (transform.localRotation.eulerAngles+ Vector3.back*mSpinDir), 0.05f);
		}
		//Reverse Direction ~Adam
		if(mSpinTimer > 60f+mReverseTime)
		{
			mSpinDir *= -1f;
			mSpinTimer = 20f;
			mReverseTime = Random.Range(10,40);
		}

		//Keep swarm grid slots all facing the same way ~Adam
		foreach(SwarmGridSlot gridSlot in FindObjectsOfType<SwarmGridSlot>())
		{
			gridSlot.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
		}
	}
}
