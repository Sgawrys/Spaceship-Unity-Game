using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	public GameObject player;
	public float delay = 0.8f;
	public Vector3 offset = new Vector3(-2.5f, 2.0f, 0.0f);
	
	public Vector3 playerTransform;
	
	public Quaternion rotationalOffset;
	
	void Setup() {
		
	}
	
	void Update(){
		/*if(Input.GetKeyDown(KeyCode.H)) {
			StartCoroutine("FieldView");
		}
		if(visible) {
			Camera.main.fieldOfView =  Mathf.Lerp(Camera.main.fieldOfView, 90, 0.05f);
		}else{
			Camera.main.fieldOfView =  Mathf.Lerp(Camera.main.fieldOfView, 179, 0.05f);
		}*/	
	}
	
	void FixedUpdate () {
		//playerTransform = player.transform.position;
		//transform.position = player.transform.position + offset ;
	}
}
