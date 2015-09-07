using UnityEngine;
using System.Collections;

public class PlayerBulletController : MonoBehaviour 
{
	
	public float bulletSpeed = 30.0f;
	private float selfDestructTimer = 0.0f;
	public bool mSideBullet = false;
	public int mPlayerBulletNumber = 1;

	public void Start()
	{
		bulletSpeed+= (30f/25f*Application.loadedLevel);
		Vector3 bulletForce;
		bulletForce = new Vector3(0.0f,bulletSpeed, 0f);
		bulletForce = Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * bulletForce;
		
		GetComponent<Rigidbody>().velocity = bulletForce;
		selfDestructTimer = Time.time + 5.0f;
	}
	
	void Update()
	{
		if(selfDestructTimer < Time.time || transform.position.y >= 24f)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other){

		//Debug.Log (other.gameObject.tag);

//		if (other.gameObject.tag == "Horn") {
//
//			//Debug.Log("Hit Horn");
//
//			other.GetComponent<LDBossHorn> ().TakeDamage();
//			Destroy (this.gameObject);
//		}
//
//		if (other.gameObject.tag == "Eye") {
//
//			other.GetComponent<BossEye> ().TakeDamage();
//			Destroy (this.gameObject);
//		}

		if(other.gameObject.GetComponent<BossWeakPoint>() != null)
		{

			other.gameObject.GetComponent<BossWeakPoint>().TakeDamage ();
			Destroy (this.gameObject);
		}
	}


	//For getting hit by boss beams ~Adam
//	void OnParticleCollision(GameObject other)
//	{
//		Debug.Log("The bullet hit a by a particle");
//		//FindObjectOfType<ScoreManager>().LoseALife();
//		Destroy(gameObject);
//	}

}
