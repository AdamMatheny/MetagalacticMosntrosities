using UnityEngine;
using System.Collections;

public class MobileCameraZoomAdjust : MonoBehaviour 
{
	[SerializeField] private float mZoomSize = 38;
	// Use this for initialization
	void Start () 
	{
		if(System.Math.Round (Camera.main.aspect,2) == System.Math.Round (9f/16f,2))
		{
			GetComponent<Camera>().orthographicSize = mZoomSize;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
