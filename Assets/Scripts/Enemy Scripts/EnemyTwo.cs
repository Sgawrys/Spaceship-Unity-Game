using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTwo : Enemy {
	
	private bool isAttachedToPlayer;
	private static GameObject enemyTwoPrefab = (GameObject)Resources.Load("Prefabs/Enemy/Enemy_Three/Enemy_Three"); 
	
	public static GameObject Create(Vector3 position){
		GameObject newObject = Instantiate(enemyTwoPrefab) as GameObject;
		newObject.transform.position = position;
		return newObject;
	}
	
	void Start (){
		base.Start();
		isAttachedToPlayer = false;
		fireRate = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isAttachedToPlayer || !Movement.IsEnemyAttached){
			Vector3 moveVec = Vector3.forward + moveAwayFromEnemies().normalized  + moveTowardPlayer().normalized;
			this.rigidbody.velocity = Vector3.Lerp(this.rigidbody.velocity,
				moveVec * moveSpeed,
				Time.deltaTime * 100);
			AttackPlayer();
		}
		else {
			this.transform.rotation = player_transform.rotation;
			this.transform.position = new Vector3(
				player_transform.transform.position.x, 
				player_transform.transform.position.y + 10.0f, 
				player_transform.transform.position.z);
		}
	}
			
//	private Vector3 moveTowardPlayer(){
//		if(isPlayerNear == true){
//			Vector3 vectorToPlayer = player_transform.position - transform.position;
//			transform.rotation = Quaternion.Slerp(transform.rotation,
//				Quaternion.LookRotation(player_transform.position),
//				Time.deltaTime);
//			//bool ignorePlayer = Mathf.Sqrt((vectorToPlayer).sqrMagnitude) < ignorePlayerProximity;
////			if(!ignorePlayer){
////				return vectorToPlayer;
////			}
//			return vectorToPlayer;
//		}
//		return Vector3.zero;
//	}
	
	private Vector3 moveTowardPlayer(){
		if(isPlayerNear == true){
			Vector3 vectorToPlayer = player_transform.position - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(player_transform.position),
				Time.deltaTime);
			bool ignorePlayer = Mathf.Sqrt((vectorToPlayer).sqrMagnitude) < ignorePlayerProximity;
			if(!Movement.IsEnemyAttached || !ignorePlayer){
				return vectorToPlayer;
			}
			return -vectorToPlayer * avoidSpeed;
		}
		return Vector3.zero;	
	}
	
	//TODO: if enemies get too close, make them turn another way.
	private Vector3 moveAwayFromEnemies(){
		Transform currTransform;
		Vector3 moveVector = Vector3.zero;
		float offset;
		foreach(KeyValuePair<int,Enemy> entry in enemiesTable){
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
			if((Time.time > timeLastFire + fireRate)){
				Projectile laser = Projectile.Create(this.gameObject);
				laser.rigidbody.AddForce(vectorToPlayer.normalized* laserSpeed);
				timeLastFire = Time.time;
			}
		}
	}
	
	public override void OnEnemyDestory()
	{
		base.OnEnemyDestory();
		if(isAttachedToPlayer){
			isAttachedToPlayer = false;
			Movement.IsEnemyAttached = false;
		}
	}
	
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.CompareTag("Player") && !Movement.IsEnemyAttached){
			isAttachedToPlayer = true;
			Movement.IsEnemyAttached = true;
			foreach(Collider collider in player_transform.gameObject.GetComponentsInChildren<Collider>()){
				Physics.IgnoreCollision(collider,this.transform.collider);
			}
		}
	}
}
