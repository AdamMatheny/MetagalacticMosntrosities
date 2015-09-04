using UnityEngine;
using System.Collections;

public class BulletBomb : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<EnemyShipAI>() != null)
		{
			if(!other.name.Contains("Red"))
			{
				other.GetComponent<EnemyShipAI>().EnemyShipDie();
			}
		}
		if(other.GetComponent<PlayerBulletController>() != null || other.GetComponent<EnemyBulletController>() != null)
		{
			Destroy (other.gameObject);
		}
		if(other.GetComponent<PlayerShipController>() != null && FindObjectOfType<ScoreManager>() != null)
		{
			FindObjectOfType<ScoreManager>().LoseALife();
		}
	}
}
