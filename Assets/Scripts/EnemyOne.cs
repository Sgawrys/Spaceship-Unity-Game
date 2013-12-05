using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyOne : MonoBehaviour {
	
	private Transform player_transform;
	private Dictionary<int,GameObject> enemiesTable;
	private bool isPlayerNear;
	private float moveSpeed = 20;
	private float rotationSpeed = 3;
	private float proximityThreshold = 50;
	
	// Use this for initialization
	void Start () {
		isPlayerNear = false;
		enemiesTable = new Dictionary<int, GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveVec = transform.forward + moveAwayFromEnemies()  + moveTowardPlayer();
		transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(moveVec),
				Time.deltaTime);
		this.rigidbody.velocity = moveVec * moveSpeed * Time.deltaTime;
	}
	
	private Vector3 moveTowardPlayer(){
		if(isPlayerNear == true){
			//follow the player
//			transform.rotation = Quaternion.Slerp(transform.rotation,
//				Quaternion.LookRotation(player_transform.position - transform.position),
//				rotationSpeed * Time.deltaTime);
			return player_transform.position - transform.position;
			//this.rigidbody.velocity = transform.forward * moveSpeed;
		}
		else return Vector3.zero;
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
	
	void OnTriggerEnter(Collider other){
		if(other.tag == Tags.player){
			//The player is in sight
			isPlayerNear = true;
			player_transform = other.transform;
		}
		else if(other.tag == Tags.enemy){
			int key = other.gameObject.GetInstanceID();
			GameObject tempout;
			if(!enemiesTable.TryGetValue(key, out tempout)){
				enemiesTable.Add(other.gameObject.GetInstanceID(),other.gameObject);		
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.CompareTag(Tags.player)){
			//isPlayerNear = false;
		}
		else if(other.CompareTag(Tags.enemy)){
			enemiesTable.Remove(other.gameObject.GetInstanceID());
		}
	}
	
}
