using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	private static GameObject minePrefab = (GameObject)Resources.Load("Prefabs/Weapons/Mine");
	public int CreatorId {get; set;}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider collider){
		if(collider.gameObject.CompareTag("Player")){
			//reducePlayer health eventually
			MineExplosion.CreateExplosion(this.transform.position);
			Destroy(this.gameObject);
		}
	}
	
	public static Mine Create(GameObject sourceObject){
		GameObject newObject = Instantiate(minePrefab,sourceObject.transform.position,sourceObject.transform.rotation) as GameObject;
   		Mine mine = newObject.GetComponent<Mine>();
		mine.CreatorId = sourceObject.GetInstanceID();
		foreach(Collider collider in sourceObject.GetComponentsInChildren<Collider>()){
			Physics.IgnoreCollision(collider,mine.gameObject.collider);
		}
		return mine;
	}
}
