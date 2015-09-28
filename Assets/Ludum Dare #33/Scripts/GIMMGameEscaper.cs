using UnityEngine;
using System.Collections;

//Press Esc to quit the game ~Adam

public class GIMMGameEscaper : MonoBehaviour 
{


	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.Escape))
		{
				Application.Quit ();
		}
	}
}
