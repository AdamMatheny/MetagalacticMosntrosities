using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour 
{

	public GameObject evilLaugh;

	public float waitSeconds = 12f;
	public GameObject ship;

	void Update () 
	{

		//Cursor.visible = true;

		waitSeconds -= Time.deltaTime;

		if (waitSeconds <= 4) {

			evilLaugh.SetActive(true);
		}

		if (waitSeconds <= -0.5f) 
		{

			ReloadGame();
		}

		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse1)) 
		{
			ReloadGame();
		}
	}

	void ReloadGame()
	{
		evilLaugh.SetActive(false);
		Destroy(ship);
//		if(FindObjectOfType<ScoreManager>().gameObject != null)
//		{
//			Destroy(FindObjectOfType<ScoreManager>().gameObject);
//		}
		Application.LoadLevel("Credits");

	}
}
