using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
	public GUIStyle style = new GUIStyle();
	
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
}
