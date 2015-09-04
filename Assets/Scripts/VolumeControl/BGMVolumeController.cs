using UnityEngine;
using System.Collections;

public class BGMVolumeController : MonoBehaviour 
{
	float mStartingVolume;

	// Use this for initialization
	void Start () 
	{
		mStartingVolume = GetComponent<AudioSource>().volume;
//		PlayerPrefs.SetFloat("SFXVolume", 0.8f);
//		PlayerPrefs.SetFloat("BGMVolume", 0.8f);
		GetComponent<AudioSource>().ignoreListenerVolume = true;
		GetComponent<AudioSource>().volume = mStartingVolume * PlayerPrefs.GetFloat("BGMVolume");
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetComponent<AudioSource>().volume = mStartingVolume * PlayerPrefs.GetFloat("BGMVolume");
		AudioListener.volume = PlayerPrefs.GetFloat("SFXVolume");

		//Mostly temporary, some people were complaining that the SFX were too loud and they were too lazy to turn them down ~ Jonathan
		if (Application.loadedLevel == 0) { //If on main menu ~ Jonathan

			//Debug.Log(PlayerPrefs.GetFloat("SFXVolume"));
			if(PlayerPrefs.GetFloat("SFXVolume") > .8f)
				AudioListener.volume = PlayerPrefs.GetFloat("SFXVolume") - .9f;
		}
	}
}
