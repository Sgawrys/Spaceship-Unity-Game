using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void OnGUI() {
		GUI.Window(0, new Rect(0,0,Screen.width,Screen.height),MainMenuFunc,"");
	}
	
	void MainMenuFunc(int windowId) {
		GUI.Label(new Rect((Screen.width/2)-75,(Screen.height/2)-300,200,100),"Spaceship Explorer");
		GUI.Label(new Rect((Screen.width/2)-100,(Screen.height/2)-200,200,100),"Made by Vincent Gaviria and Stefan Gawrys");
		if(GUI.Button(new Rect((Screen.width/2)-100, (Screen.height/2)-100, 200, 200), "Play Game")) {
			Application.LoadLevel("Space");	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
