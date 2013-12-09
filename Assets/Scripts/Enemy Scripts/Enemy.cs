using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	
	protected const float moveSpeed = 40;
	protected const float avoidSpeed = 20;
	protected const float rotationSpeed = 3;
	protected const float proximityThreshold = 50;
	protected const float laserSpeed = 10000.0f;
	
	public Transform player_transform;
	public Rigidbody player_body;
	public Dictionary<int,Enemy> enemiesTable;
	public bool isPlayerNear;
	protected float fireRate = 0.5f; //laser/sec
	protected float timeLastFire;
	protected float ignorePlayerProximity = 100;
	protected float health = 100.0f;
	
	
	
	// Use this for initialization
	public void Start () {
		isPlayerNear = false;
		enemiesTable = new Dictionary<int, Enemy>();
		timeLastFire = Time.time;
	}
	
	public void decrementHealth(float decAmount){
		health -= decAmount;
	}
	
	public float getHealth(){
		return health;
	}
	
	public virtual void OnEnemyDestory(){
		foreach(KeyValuePair<int,Enemy> pair in enemiesTable){
			pair.Value.enemiesTable.Remove(this.GetInstanceID());	
		}
	}
}
