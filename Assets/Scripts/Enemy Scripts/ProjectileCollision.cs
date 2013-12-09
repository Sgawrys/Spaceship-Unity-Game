using UnityEngine;
using System.Collections;

public class ProjectileCollision : MonoBehaviour {

	public Enemy enemyObject;
	
	//Projectile objects only respond to triggers
	//probably not the best way to do this
	void OnTriggerEnter(Collider objCollider){
		if(objCollider.gameObject.CompareTag("Projectile")){
			enemyObject.decrementHealth(10);
			if(enemyObject.getHealth() < 0.0f){
				ParticleExplosion.CreateExplosion(enemyObject.transform.position);
				//enemyObject.transform.position = new Vector3(Mathf.Infinity,Mathf.Infinity,Mathf.Infinity);
				enemyObject.OnEnemyDestory();
				Destroy(enemyObject.gameObject);
			}
		}
	}
}
