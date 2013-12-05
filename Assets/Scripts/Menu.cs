using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	Rect mainWindow;
	
	Vector3 goalFirst = new Vector3(18.0f, 1.0f, 10.0f);
	Vector3 goalSecond = new Vector3(3.0f, 1.0f, 10.0f);
	bool lightMove;
	
	public float creationChance = 0.02f;
	
	public float initialPositionX = -80.0f;
	public float cutoffDistanceX = 200.0f;
	public float deletionDistanceX = 175.0f;
	
	public float yVariety = 40.0f;
	public float zVariety = 50.0f;
	
	ArrayList gameObjLists = new ArrayList();
	
	// Use this for initialization
	void Start () {
		lightMove = true;
	}
	
	void OnGUI() {
		mainWindow = GUI.Window(0, new Rect(0,0,Screen.width/2,Screen.height),MainMenuFunc,"");
	}
	
	void MainMenuFunc(int windowId) {
		GUI.Label(new Rect((mainWindow.width/2)-75,(Screen.height/2)-300,200,100),"Spaceship Explorer");
		GUI.Label(new Rect((mainWindow.width/2)-100,(Screen.height/2)-200,200,100),"Made by Vincent Gaviria and Stefan Gawrys");
		if(GUI.Button(new Rect((mainWindow.width/2)-100, (Screen.height/2)-100, 200, 100), "Play Game")) {
			Application.LoadLevel("Space");	
		}
		if(GUI.Button (new Rect((mainWindow.width/2)-100, (Screen.height/2), 200, 100), "Instructions")) {

		}
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine("MoveLight");
		StartCoroutine("MoveModel");
		StartCoroutine("EnemyCreation");
	}
	
	IEnumerator MoveLight() {
		GameObject light = GameObject.FindGameObjectWithTag("MenuLight");
		if(lightMove) {
			light.transform.position = Vector3.Lerp(light.transform.position, goalFirst, Time.deltaTime);
			if(Vector3.Distance(light.transform.position, goalFirst) <= 0.5f) {
				lightMove = false;
			}
		}else{
			light.transform.position = Vector3.Lerp(light.transform.position, goalSecond, Time.deltaTime);
			if(Vector3.Distance(light.transform.position, goalSecond) <= 0.5f) {
				lightMove = true;
			}
		}
		
		yield return new WaitForSeconds(.1f);
	}
	
	IEnumerator MoveModel() {
		GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
		player.transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f));
		yield return new WaitForSeconds(.1f);
	}
	
	IEnumerator EnemyCreation() {
		if(Random.value <= creationChance) {
			GameObject[] obj = GameObject.FindGameObjectsWithTag(Tags.enemy);
			int random = (int)(Random.value*obj.Length);
			GameObject clone = (GameObject)GameObject.Instantiate(obj[random]);
			clone.transform.position = new Vector3(initialPositionX,(Random.value*yVariety)-(yVariety/2),(Random.value*zVariety)+zVariety);
			clone.tag = "";
			gameObjLists.Add(clone);
			StartCoroutine(CreateEnemy(clone));
		}
		ArrayList removeReference = new ArrayList();
		foreach(GameObject obj in gameObjLists) {
			if(obj.transform.position.x >= deletionDistanceX) {
				removeReference.Add(obj);
				DestroyObject(obj);
			}else{
				StartCoroutine(CreateEnemy(obj));
			}
		}
		
		foreach(GameObject obj in removeReference) {
			gameObjLists.Remove(obj);	
		}
		
		yield return new WaitForSeconds(.1f);
	}
	
	IEnumerator CreateEnemy(GameObject obj) {
		obj.transform.position = Vector3.Lerp(obj.transform.position, new Vector3(cutoffDistanceX,obj.transform.position.y,obj.transform.position.z), Time.deltaTime);
		yield return new WaitForSeconds(.1f);
	}
	
}
