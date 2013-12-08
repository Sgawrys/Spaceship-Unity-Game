using UnityEngine;
using System.Collections;

public class ParticleExplosion : MonoBehaviour {
	
	private static GameObject explosionPrefab = (GameObject)Resources.Load("Prefabs/Particles/ParticleExplosion");
	
	public static void CreateExplosion(Vector3 position){
		GameObject newObject = Instantiate(explosionPrefab,position,Quaternion.identity) as GameObject;
	}
	
	void Awake(){
		particleSystem.Emit(300);
	}
	
	// Update is called once per frame
	void Update () {
		if(!particleSystem.IsAlive()){
			Destroy(this);	
		}
	}
}
