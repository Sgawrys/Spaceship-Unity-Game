using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	//time to destroy projectile, in seconds
	public static float timeToDestroy = 5;
	private float timeCreated;
	public int CreatorId {get; set;}
	private static GameObject laserPrefab = (GameObject)Resources.Load("Prefabs/laser_projectile");
	
	// Use this for initialization
	public void Start () {
		timeCreated = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > timeCreated + timeToDestroy){
			Destroy(this.gameObject);
		}
	}
	
	public static Projectile Create(GameObject sourceObject){
		GameObject newObject = Instantiate(laserPrefab,sourceObject.transform.position,sourceObject.transform.rotation) as GameObject;
   		Projectile projectile = newObject.GetComponent<Projectile>();
		projectile.CreatorId = sourceObject.GetInstanceID();
		foreach(Collider collider in sourceObject.GetComponentsInChildren<Collider>()){
			Physics.IgnoreCollision(collider,projectile.gameObject.collider);
		}
		return projectile;
	}
	
	void OnTriggerEnter(Collider objCollider){
		//leave for now
		if(objCollider.gameObject.GetInstanceID() != CreatorId 
			&& !objCollider.gameObject.CompareTag("SenseCollider")){
			Destroy(this.gameObject);	
		}
	}
}
