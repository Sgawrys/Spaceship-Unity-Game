using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	//hold laser prefab here for testing. This game object should
	//be loaded when the item is picked up and cached in the player
	//somewhere. It should also change per pickup.
	private GameObject laserPrefab;
	float laserSpeed = 10000.0f;
	
	// Use this for initialization
	void Start () {
		laserPrefab = (GameObject)Resources.Load("Prefabs/Weapons/laser_projectile");
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: check if we are allowed to fire or not
		if(Input.GetMouseButtonDown(0)){
			Projectile laser = Projectile.Create(this.gameObject);
			laser.rigidbody.AddRelativeForce( (new Vector3(0,0,1)) * laserSpeed);
		}
	}
}
