using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyOne : Enemy {
	
	private const float shootAngleThreshold = 20.0f;
	
	void Start () {
		base.Start();
		ignorePlayerProximity = 80;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveVec = transform.forward + moveAwayFromEnemies().normalized  + moveTowardPlayer().normalized;
		transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(moveVec),
				Time.deltaTime);
		this.rigidbody.velocity = Vector3.Lerp(this.rigidbody.velocity,
			moveVec * moveSpeed,
			Time.deltaTime * 100);
		ShootPlayer();
	}
	
	private Vector3 moveTowardPlayer(){
		if(isPlayerNear == true){
			Vector3 vectorToPlayer = player_transform.position - transform.position;
			bool ignorePlayer = Mathf.Sqrt((vectorToPlayer).sqrMagnitude) < ignorePlayerProximity;
			if(!ignorePlayer){
				return vectorToPlayer;
			}
			return -vectorToPlayer/4 * avoidSpeed;
		}
		return Vector3.zero;
	}
	
	private Vector3 matchCollectiveEnemyVel(){
		Transform currTransform;
		Vector3 moveVector = this.rigidbody.velocity + Vector3.one;
		foreach(KeyValuePair<int,GameObject> entry in enemiesTable){
			moveVector += entry.Value.rigidbody.velocity;
		}
		return moveVector/enemiesTable.Count;
	}
	
	//TODO: if enemies get too close, make them turn another way.
	private Vector3 moveAwayFromEnemies(){
		Transform currTransform;
		Vector3 moveVector = Vector3.zero;
		float offset;
		foreach(KeyValuePair<int,GameObject> entry in enemiesTable){
			//TODO: if the enemy is destroyed it is not cleared from 
			//the list change this behaviour.
			if(entry.Value != null){
				currTransform = entry.Value.transform;
				offset = (currTransform.position - transform.position).sqrMagnitude;
				if(Mathf.Sqrt(offset) < proximityThreshold){
					moveVector = moveVector - (currTransform.position - transform.position);
				}
			}
		}
		return moveVector;
	}
	
	private void ShootPlayer(){
		if(isPlayerNear == true){
			Vector3 vectorToPlayer = player_transform.position - transform.position;
			float angle = Vector3.Angle(transform.forward,vectorToPlayer);
			if(angle < 20 && (Time.time > timeLastFire + fireRate)){
				Projectile laser = Projectile.Create(this.gameObject);
				laser.rigidbody.AddRelativeForce(new Vector3(0.0f,0.0f,1.0f)* laserSpeed);
				timeLastFire = Time.time;
			}
		}
	}
}
