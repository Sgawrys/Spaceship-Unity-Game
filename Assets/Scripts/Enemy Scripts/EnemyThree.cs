using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyThree : Enemy {
	
	//private ArrayList<GameObject> turrets;
	
	void Start(){
		base.Start();
		health = 500;
		//turrets = new ArrayList<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isPlayerNear == true){
			//shootPlayer();
		}
	}
}
