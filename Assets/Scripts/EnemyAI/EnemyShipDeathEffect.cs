using UnityEngine;
using System.Collections;

public class EnemyShipDeathEffect : MonoBehaviour 
{
	[SerializeField] float mDeathTimer = 3f;

	// Use this for initialization
	void Start () 
	{
		//Make explosion noises quieter as the game goes on
		if(GetComponent<AudioSource>()!=null)
		{
			GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume * (1f - (Application.loadedLevel/50f));
		}
	}//END of Start()
	
	// Update is called once per frame
	void Update () 
	{




		//Decrement Timer ~Adam
		mDeathTimer -= Time.deltaTime;


		//Destroy object if timer has run out ~Adam
		if(mDeathTimer <= 0f)
		{
			Destroy(this.gameObject);
		}
	}//END of Update()
}
