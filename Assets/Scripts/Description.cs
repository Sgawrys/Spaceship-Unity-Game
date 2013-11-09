using UnityEngine;
using System.Collections;

/*
 * This script will ask a descriptor component for ship parts to describe their offset on the player's ship, note
 * ship parts can have either a right or left orientation on the ship, and in this case one of the descriptor's
 * positional components will be flipped
 */
public class Description : MonoBehaviour {
	
	/*Hashtable for additional key, value data description about the object*/
	public Hashtable table;
	
	public Vector3 offset;
	public Vector3 rotation;
	public string name;
	
	public void Start() {
		table = new Hashtable();	
	}
	
	/* Used for quickly mirroring ship parts placed onto the ship*/
	public Vector3 changeOrientation() {
		offset.z *= -1;
		return offset;
	}
	
	public void insert(object key, object val) {
		if(table.ContainsKey(key)) {
			table[key] = val;
		}else{
			table.Add(key, val);	
		}
	}
}
