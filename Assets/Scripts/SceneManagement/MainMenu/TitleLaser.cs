using UnityEngine;
using System.Collections;

public class TitleLaser : MonoBehaviour 
{
	public GameObject mMenuUI;

	public void DestroyShips()
	{
		Camera.main.GetComponent<CameraShaker> ().RumbleController(.3f, 1f);

		mMenuUI.GetComponent<GetSome>().StartGame();
	}
}
