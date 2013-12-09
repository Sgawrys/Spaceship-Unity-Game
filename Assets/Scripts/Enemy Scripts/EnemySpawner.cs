<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	
	public EnemyThree enemyObject;
	public float spawnDelayNear = 25; //sec
	public float spawnDelayFar = 60;
	private float timeSinceLastSpawn;
	
	// Use this for initialization
	void Start () {
		timeSinceLastSpawn = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		bool spawn = false;
		if((enemyObject.isPlayerNear && Time.time > timeSinceLastSpawn + spawnDelayNear)
			|| (Time.time > timeSinceLastSpawn + spawnDelayFar))
		{
			spawn = true;
		}
		if(spawn){
			float rand = Random.Range(0,1);
			if(rand < 0.50f){
				EnemyOne.Create(this.transform.position);
			}
			else{
				EnemyTwo.Create(this.transform.position);
			}
			timeSinceLastSpawn = Time.time;	
		}
	}
}
=======
﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	
	public EnemyThree enemyObject;
	public float spawnDelayNear = 25; //sec
	public float spawnDelayFar = 60;
	private float timeSinceLastSpawn;
	
	// Use this for initialization
	void Start () {
		timeSinceLastSpawn = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		bool spawn = false;
		if((enemyObject.isPlayerNear && Time.time > timeSinceLastSpawn + spawnDelayNear)
			|| (Time.time > timeSinceLastSpawn + spawnDelayFar))
		{
			spawn = true;
		}
		if(spawn){
			float rand = Random.Range(0,1);
			if(rand < 0.50f){
				EnemyOne.Create(this.transform.position);
			}
			else{
				EnemyTwo.Create(this.transform.position);
			}
			timeSinceLastSpawn = Time.time;	
		}
	}
}
>>>>>>> 1d2c46a8e2ebbf6900b0c682472ac356d0c626db
