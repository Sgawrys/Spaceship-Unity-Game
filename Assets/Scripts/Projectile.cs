using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	//time to destroy projectile, in seconds
	public static float timeToDestroy = 5;
	private float timeCreated;
	
	// Use this for initialization
	void Start () {
		timeCreated = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > timeCreated + timeToDestroy){
			Destroy(this.gameObject);
		}
	}
	
	void OnTriggerEnter(Collider objCollider){
		//leave for now
		if(!objCollider.gameObject.CompareTag("Player")){
			Destroy(this.gameObject);		
		}
	}
}
