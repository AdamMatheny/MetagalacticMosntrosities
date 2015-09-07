using UnityEngine;
using System.Collections;

public class VoiceRcognitionGame : MonoBehaviour {

	public GameObject panel = null;

	#region Functions To Call By Voice
	void PauseGame(){

		if (Time.timeScale == 1) {

			Time.timeScale = 0;
			panel.SetActive(true);
				}
			}

	void ContinueGame(){
		
		if (Time.timeScale == 0) {
			
			Time.timeScale = 1;
			panel.SetActive(false);
				}
			}

	void QuitGame(){

		if (Time.timeScale == 0) {

			Application.Quit();
				}
			}
	#endregion
}
