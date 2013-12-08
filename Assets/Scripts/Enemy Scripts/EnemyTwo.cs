using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTwo : Enemy {
	
	// Update is called once per frame
	void Update () {
		Vector3 moveVec = transform.forward + moveAwayFromEnemies().normalized  + moveTowardPlayer().normalized;
		transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(moveVec),
				Time.deltaTime);
		this.rigidbody.velocity = Vector3.Lerp(this.rigidbody.velocity,
			moveVec * moveSpeed,
			Time.deltaTime * 100);
	}
	
	private Vector3 moveTowardPlayer(){
		bool ignorePlayer = Mathf.Sqrt((transform.position - player_transform.position).sqrMagnitude) < ignorePlayerProximity;
		if(isPlayerNear == true && !ignorePlayer){
			//follow the player
//			transform.rotation = Quaternion.Slerp(transform.rotation,
//				Quaternion.LookRotation(player_transform.position - transform.position),
//				rotationSpeed * Time.deltaTime);
			return player_transform.position - transform.position;
			//this.rigidbody.velocity = transform.forward * moveSpeed;
		}
		else return Vector3.one;
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
			currTransform = entry.Value.transform;
			offset = (currTransform.position - transform.position).sqrMagnitude;
			if(Mathf.Sqrt(offset) < proximityThreshold){
				moveVector = moveVector - (currTransform.position - transform.position);
			}
		}
		return moveVector;
	}
}
