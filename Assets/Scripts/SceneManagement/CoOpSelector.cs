using UnityEngine;
using System.Collections;

//For turning on the second player ship when we do Co-Op Mode ~Adam

public class CoOpSelector : MonoBehaviour 
{
	public bool mCoOpEnabled = false;

	
	// Use this for initialization
	void Start () 
	{
	
	}

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	// Update is called once per frame
	void Update () 
	{
		//Once we're actually in-game, enable the sencond player if co-op is enabled. ~Adam
		if(Application.loadedLevel != 0)
		{
			Debug.Log ("Not on title screen!");
			if(mCoOpEnabled)
			{
				//Find the main player ship so we can find the inactive P2 ship ~Adam
				if(FindObjectOfType<PlayerShipController>() != null)
				{
					Debug.Log ("Found Player 1");
					PlayerShipController player = FindObjectOfType<PlayerShipController>();
					//Make sure there's a player 2 ship present and then enable it ~Adam
					if(player.mPlayerTwo != null)
					{
						Debug.Log("Found Player 2");
						player.mPlayerTwo.gameObject.SetActive(true);
						Debug.Log("Activated Player 2");
						GameObject.Find ("P1ShipEmotes").SetActive (false);
						GameObject.Find("P2ShipUI").SetActive (true);
					}
					else
					{
						GameObject.Find ("P1ShipEmotes").SetActive (true);
						GameObject.Find("P2ShipUI").SetActive (false);
					}

				}
			}
			else
			{
				GameObject.Find ("P1ShipEmotes").SetActive (true);
				GameObject.Find("P2ShipUI").SetActive (false);
			}
			//Delete self so that we don't get duplicate copies of the object piling up on the main menu scene ~Adam
			Debug.Log("Destroying CoOp selector");
			Destroy(this.gameObject);
		}
	}
}
