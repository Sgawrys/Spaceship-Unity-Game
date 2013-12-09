using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
	public GUIStyle style = new GUIStyle();
	
	public GUITexture healthBar;
	public float health = 100;
	public float bullet_damage = 0.2f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		Color tmpColor = GUI.color;
		GUI.color = new Color(1,0.5f,0.5f,0.5f);
		GUIUtility.RotateAroundPivot(45.0f, new Vector2((Screen.width/2),(Screen.height/2))); 
		GUI.Box(new Rect((Screen.width/2) - 10,(Screen.height/2) - 10,20,20),"", style);
		GUI.color = tmpColor;
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Projectile")) {
			this.health -= bullet_damage;
			
			float newWidth = 2.56f * health;
			
			/*Update GUI texture*/
			Rect healthPixels = healthBar.pixelInset;
			healthPixels.width = newWidth;
			healthBar.pixelInset = healthPixels;
			
			if(health <= 0.0f) {
				Application.LoadLevel("GameOver");
			}
		}
	}
}
