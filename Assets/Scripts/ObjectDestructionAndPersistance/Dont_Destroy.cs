using UnityEngine;
using System.Collections;

public class Dont_Destroy : MonoBehaviour 
{

	void Awake() 
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Application.loadedLevel == 0 || (Application.loadedLevelName == "Credits" && tag != "Player Bullet"))
		{
			Destroy(this.gameObject);
		}
//		DontDestroyOnLoad(transform.gameObject);
	}
}
