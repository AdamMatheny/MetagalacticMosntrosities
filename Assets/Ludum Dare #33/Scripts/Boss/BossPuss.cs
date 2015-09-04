using UnityEngine;
using System.Collections;

public class BossPuss : BossGenericScript {

	public Sprite mouthOpen;
	public Sprite mouthClosed;
	public GameObject mouth;
	public float timer;
	float timerTemp;

	public int health;

	public bool isSpewing;

	public override void Start ()
	{
		timerTemp = timer;

		base.Start ();
	}

	public override void Update ()
	{
		if (isSpewing) {

			mouth.GetComponent<SpriteRenderer> ().sprite = mouthOpen;
			//Do Spew Stuff
		} else {

			mouth.GetComponent<SpriteRenderer> ().sprite = mouthClosed;
		}

		if (timerTemp > 0) {

			timerTemp -= Time.deltaTime;
		} else {

			SpewEnemies();
			timerTemp = timer;
		}

		base.Update ();
	}

	public void SpewEnemies(){

		isSpewing = true;
		StartCoroutine ("SpewEnemiesEnum");
	}

	private IEnumerator SpewEnemiesEnum(){

		yield return new WaitForSeconds(3);
		isSpewing = false;
	}

	public void TakeDamage(){

		health --;
	
		if (health <= 0) {

			Debug.Log("Killed Boss");
			Destroy(gameObject);
		}
	}
}