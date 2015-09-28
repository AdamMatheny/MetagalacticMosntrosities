using UnityEngine;
using System.Collections;
using InControl;
using XInputDotNetPure;

public class Boss5 : BossGenericScript {

	public RuntimeAnimatorController newAnim;

	public GameObject[] weapons = new GameObject[1];

	public GameObject gemBullet;
	public GameObject eyeBullet;

	public Transform gemBulletTransform1;
	public Transform gemBulletTransform2;
	public Transform gemBulletTransform3;
	public Transform eyeBulletTransform1;
	public Transform eyeBulletTransform2;

	// Use this for initialization
	public override void Start () {
	
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Update () {
	
		base.Update ();

		if (mCurrentHealth == 0) {

			mDying = true;
		}

		if ((Input.GetButtonDown ("FireGun") || InputManager.ActiveDevice.Action1.WasPressed) && mChargeReady && !mOverheated) {

			mChargeReady = false;
			mCurrentCharge = 0;

			if(gemBullet != null && gemBulletTransform1 != null && gemBulletTransform2 != null && gemBulletTransform3 != null){

				Instantiate(gemBullet, gemBulletTransform1.position, Quaternion.identity);
				Instantiate(gemBullet, gemBulletTransform2.position, Quaternion.identity);
				Instantiate(gemBullet, gemBulletTransform3.position, Quaternion.identity);
			}

			if(eyeBullet != null && eyeBulletTransform1 != null && eyeBulletTransform2 != null){
				
				Instantiate(eyeBullet, eyeBulletTransform1.position, Quaternion.identity);
				Instantiate(eyeBullet, eyeBulletTransform1.position, Quaternion.identity);
				Instantiate(eyeBullet, eyeBulletTransform2.position, Quaternion.identity);
				Instantiate(eyeBullet, eyeBulletTransform2.position, Quaternion.identity);
			}
		}

		if (mOverheated) {

			foreach (GameObject weapon in weapons) {

				if(weapon != null){

					weapon.SetActive (false);
				}
			}
		} else {

			foreach (GameObject weapon in weapons) {

				if(weapon != null){

					weapon.SetActive (true);
				}
			}
		}
	}

	public void ChangeAnimation(){

		gameObject.GetComponent<Animator> ().runtimeAnimatorController = newAnim;
		//Debug.Log ("Changed Anim!");
	}
}
