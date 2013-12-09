using UnityEngine;
using System.Collections;

public class AutomaticTurret : MonoBehaviour {
	
	public Enemy enemyObject;
	private Vector3 initialForward;
	public float fireRate = 0.5f; 
	private float timeLastFire;
	private float laserSpeed = 10000.0f;
	
	void Start(){
		initialForward = transform.forward;	
		timeLastFire = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(enemyObject.isPlayerNear){
			Vector3 playerVector = enemyObject.player_transform.position - this.transform.position;
			float angle = Vector3.Angle(initialForward,playerVector);
			if(angle < 90){
				transform.rotation = Quaternion.Slerp(transform.rotation,
					Quaternion.LookRotation(playerVector),
					Time.deltaTime); 
				shootPlayer();
			}
			else{
				transform.rotation = Quaternion.Slerp(transform.rotation,
					Quaternion.LookRotation(initialForward),
					Time.deltaTime);		
			}
		}
	}
	
	private void shootPlayer(){
		if(Time.time > timeLastFire + fireRate){
			Projectile laser = Projectile.Create(this.gameObject);
			laser.rigidbody.AddForce(transform.forward.normalized * laserSpeed);
			timeLastFire = Time.time;
		}
	}
}
