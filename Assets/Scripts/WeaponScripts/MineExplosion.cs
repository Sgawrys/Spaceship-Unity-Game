using UnityEngine;
using System.Collections;

public class MineExplosion : MonoBehaviour {
	
	private static GameObject explosionPrefab = (GameObject)Resources.Load("Prefabs/Particles/MineExplosion");
	
	public static void CreateExplosion(Vector3 position){
		GameObject newObject = Instantiate(explosionPrefab,position,Quaternion.identity) as GameObject;
	}
	
	void Awake(){
		particleSystem.Emit(200);
	}
	
	// Update is called once per frame
	void Update () {
		if(!particleSystem.IsAlive()){
			Destroy(this);	
		}
	}
}
