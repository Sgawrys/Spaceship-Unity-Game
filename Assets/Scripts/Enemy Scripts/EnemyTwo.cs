using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTwo : Enemy {
	
	private bool isAttachedToPlayer;
	
	void Start (){
		base.Start();
		isAttachedToPlayer = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isAttachedToPlayer){	
			Vector3 moveVec = moveAwayFromEnemies().normalized  + moveTowardPlayer().normalized;
			this.rigidbody.velocity = Vector3.Lerp(this.rigidbody.velocity,
				moveVec * moveSpeed,
				Time.deltaTime * 100);
			AttackPlayer();
		}
		else{
			this.transform.rotation = player_transform.rotation;
			this.transform.position = new Vector3(
				player_transform.transform.position.x, 
				player_transform.transform.position.y + 10.0f, 
				player_transform.transform.position.z);
		}
	}
	
	private Vector3 moveTowardPlayer(){
		if(isPlayerNear == true){
			Vector3 vectorToPlayer = player_transform.position - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(player_transform.position),
				Time.deltaTime);
			//bool ignorePlayer = Mathf.Sqrt((vectorToPlayer).sqrMagnitude) < ignorePlayerProximity;
//			if(!ignorePlayer){
//				return vectorToPlayer;
//			}
			return vectorToPlayer;
		}
		return Vector3.zero;
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
	
	private void AttackPlayer(){
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
	
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.CompareTag("Player")){
			isAttachedToPlayer = true;
			Physics.IgnoreCollision(player_transform.collider,this.transform.collider);
		}
	}
}
