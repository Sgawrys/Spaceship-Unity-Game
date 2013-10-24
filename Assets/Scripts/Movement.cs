using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public float speed = 100.0f;
	public Vector3 liftOffset = new Vector3(0.0f, 15.0f, 0.0f);
	
	public GameObject gunPart;
	
	void Setup()
	{
		
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
		
		Vector3 movement = new Vector3(moveVertical, 0.0f, -moveHorizontal);
		
		if(moveHorizontal > 0.5f) {
			transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(1.0f, 0.0f, 0.0f, 0.0f), 0.02f);
		}
		if(moveHorizontal < -0.5f) {
			transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0.0f, 0.0f, 0.0f, -1.0f), 0.02f);
		}
		
		if(moveHorizontal == 0.0f) {
			transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0.5f, 0.0f, 0.0f, -0.5f) , 0.05f);
		}
		
	
		if(lift) {
			rigidbody.AddForce(new Vector3(0.0f, 1.0f, 0.0f) * speed * Time.deltaTime); 
		}
		
		if(reverseLift) {
			rigidbody.AddForce(new Vector3(0.0f, -1.0f, 0.0f) * speed * Time.deltaTime);
		}
		
	
		rigidbody.AddForce(movement * speed * Time.deltaTime);
		
	}
	
	void OnTriggerEnter(Collider other) {
		Destroy(other.gameObject);
		gunPart.renderer.enabled = false;
		GameObject instantiated = (GameObject)Instantiate(gunPart, transform.position + new Vector3(-2.0f, 0.0f, -6.0f), gunPart.transform.rotation);
		GameObject instantiated2 = (GameObject)Instantiate(gunPart, transform.position + new Vector3(-2.0f, 0.0f, 6.0f), gunPart.transform.rotation);
		instantiated.renderer.enabled = true;
		instantiated.transform.parent = transform;
		
		instantiated2.renderer.enabled = true;
		instantiated2.transform.parent = transform;
	}
}
