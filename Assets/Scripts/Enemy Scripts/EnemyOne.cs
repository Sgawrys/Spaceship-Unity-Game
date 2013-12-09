using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyOne : Enemy {
	
	private const float shootAngleThreshold = 20.0f;
	private const float mineDropDelay = 5.0f;
	private static GameObject enemyOnePrefab = (GameObject)Resources.Load("Prefabs/Enemy/Enemy_One/Enemy_One");
	private float lastMineDropTime;
	
	private const float playerVelocityStop = 20.0f;
	private bool attackFormation = false;
	
	public static GameObject Create(Vector3 position){
		GameObject newObject = Instantiate(enemyOnePrefab) as GameObject;
		newObject.transform.position = position;
		return newObject;
	}
	
	void Start () {
		base.Start();
		ignorePlayerProximity = 80;
		lastMineDropTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(attackFormation) {
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(moveTowardPlayer().normalized),
				Time.deltaTime * 3);
			
			this.rigidbody.velocity = Vector3.Lerp(this.rigidbody.velocity,
				Vector3.zero,
				Time.deltaTime * 3 );
			
			AttackPlayer();
			
			Debug.DrawRay(this.transform.position, moveAwayFromEnemies().normalized * 20.0f, Color.yellow);
			Debug.DrawRay(this.transform.position, moveTowardPlayer().normalized * 10.0f, Color.cyan);
		}else{
			Vector3 moveVec = transform.forward + moveAwayFromEnemies().normalized  + moveTowardPlayer().normalized;
		
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(moveVec),
					Time.deltaTime);
			
			this.rigidbody.velocity = Vector3.Lerp(this.rigidbody.velocity,
				moveVec * moveSpeed,
				Time.deltaTime );
			
			AttackPlayer();	
			
			Debug.DrawRay(this.transform.position, moveAwayFromEnemies().normalized * 20.0f, Color.yellow);
			Debug.DrawRay(this.transform.position, moveTowardPlayer().normalized * 10.0f, Color.cyan);
			Debug.DrawRay(this.transform.position, moveVec*10.0f, Color.green);
		}
	}
	
	private Vector3 moveTowardPlayer(){
		if(isPlayerNear == true){
			Vector3 vectorToPlayer = player_transform.position - transform.position;
			bool ignorePlayer = Mathf.Sqrt((vectorToPlayer).sqrMagnitude) < ignorePlayerProximity;
			if(!ignorePlayer){
				return vectorToPlayer;
			}
			return -vectorToPlayer * avoidSpeed;
		}
		return Vector3.zero;
	}
	
	private Vector3 matchCollectiveEnemyVel(){
		Transform currTransform;
		Vector3 moveVector = this.rigidbody.velocity + Vector3.one;
		foreach(KeyValuePair<int,Enemy> entry in enemiesTable){
			moveVector += entry.Value.rigidbody.velocity;
		}
		return moveVector/enemiesTable.Count;
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
			float angle = Vector3.Angle(transform.forward,vectorToPlayer);
			
			float vel_sqrt = Mathf.Sqrt((player_body.velocity).sqrMagnitude);
			
			if(vel_sqrt < playerVelocityStop) {
				attackFormation = true;	
			}else{
				attackFormation = false;
			}
			
			if(angle < 20 && (Time.time > timeLastFire + fireRate)){
				Projectile laser = Projectile.Create(this.gameObject);
				laser.rigidbody.AddRelativeForce(new Vector3(0.0f,0.0f,1.0f)* laserSpeed);
				timeLastFire = Time.time;
			}
			else if((Time.time > lastMineDropTime + mineDropDelay) 
				&& (angle > 160) 
				&& (angle < 200)){
				//drop mine
				Mine.Create(this.gameObject);
				lastMineDropTime = Time.time;
			}
		}
	}
}
