using UnityEngine;
using System.Collections;

public class EnemyAwareness : MonoBehaviour {
	
	public Enemy enemyObject;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == Tags.player){
			//The player is in sight
			enemyObject.isPlayerNear = true;
			enemyObject.player_transform = other.transform;
		}
		else if(other.tag == Tags.enemy){
			int key = other.gameObject.GetInstanceID();
			GameObject tempout;
			if(!enemyObject.enemiesTable.TryGetValue(key, out tempout)){
				enemyObject.enemiesTable.Add(other.gameObject.GetInstanceID(),other.gameObject);		
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.CompareTag(Tags.player)){
			//isPlayerNear = false;
		}
		else if(other.CompareTag(Tags.enemy)){
			enemyObject.enemiesTable.Remove(other.gameObject.GetInstanceID());
		}
	}
}
