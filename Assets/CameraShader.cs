using UnityEngine;
using System.Collections;

public class CameraShader : MonoBehaviour {

	public CameraFilterPack_TV_Vcr shader1;
	public CameraFilterPack_TV_VHS shader2;

	public void Start(){

		shader1 = GetComponent<CameraFilterPack_TV_Vcr> ();
		shader2 = GetComponent<CameraFilterPack_TV_VHS> ();
	}

}
