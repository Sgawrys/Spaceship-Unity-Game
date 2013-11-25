using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public float speed = 100.0f;
	public Vector3 liftOffset = new Vector3(0.0f, 15.0f, 0.0f);
	
	public Vector3 mouseVecCenter;
	public Vector3 screenCenterVec;
	
	void Start()
	{
		screenCenterVec = new Vector3(0.5f, 0.5f, 0.0f);
		rigidbody.drag = 0.1f;
	}
	
	void Update()
	{
		
	}
	
	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		bool lift = Input.GetKey(KeyCode.Space);
		bool reverseLift = Input.GetKey(KeyCode.LeftControl);
		
		mouseVecCenter.x = (Input.mousePosition.x/Screen.width) - screenCenterVec.x;
		mouseVecCenter.y = (Input.mousePosition.y/Screen.height) - screenCenterVec.y;
		
		//transform.Rotate(0,10.0f,0);
		transform.Rotate(0,-mouseVecCenter.y,mouseVecCenter.x);
		
		
		//Vector3 movement = new Vector3(moveVertical, 0.0f, -moveHorizontal);
		Vector3 movement = new Vector3(moveVertical, moveHorizontal, mouseVecCenter.y * moveVertical);
		
		//TODO: Remove the camera from the player so we could use these easing
		//		turn effects.
		/*
		if(moveHorizontal > 0.5f) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.back, Vector3.down), 0.02f);
		}
		if(moveHorizontal < -0.5f) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, Vector3.up), 0.02f);
		}
		
		if(moveHorizontal == 0.0f) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.up) , 0.05f);
		}
		*/
		
	
		if(lift) {
			rigidbody.AddForce(new Vector3(0.0f, 1.0f, 0.0f) * speed * Time.deltaTime); 
		}
		
		if(reverseLift) {
			rigidbody.AddForce(new Vector3(0.0f, -1.0f, 0.0f) * speed * Time.deltaTime);
		}
		
	
		//rigidbody.AddForce(movement * speed * Time.deltaTime);
		rigidbody.AddRelativeForce(movement * speed * Time.deltaTime);
		
	}
	
	
}
