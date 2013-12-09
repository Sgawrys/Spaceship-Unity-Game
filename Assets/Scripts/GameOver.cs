using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	GameObject text;
	
	// Use this for initialization
	void Start () {
		text = GameObject.FindGameObjectWithTag("GameOverText");
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * 2);
	}
	
	void OnGUI() {
		if(GUI.Button(new Rect(0.0f,0.0f,640.0f,128.0f),"Rescue the universe once more"))
		{
			Application.LoadLevel("Menu");
		}
	}
}
