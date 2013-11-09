using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	
	private ArrayList inventoryList;
	
	public float widthScaler = 0.4f;
	public float heightScaler = 0.4f;
	
	public int horizontalTileSpacing = 100;
	public int verticalTileSpacing = 0;
	
	public int tileSizeX = 100;
	public int tileSizeY = 100;
	
	public int inventoryWidth;
	public int inventoryHeight;
	
	public GUIStyle style = new GUIStyle();
	
	public Texture buttonTexture;
	
	public bool visible;
	
	// Use this for initialization
	void Start () {
		/*Initialize the arraylist of the item ids that are currently located in this array list*/
		//inventoryList = new ArrayList<GameObject>();
		inventoryWidth = (int)(Screen.width * widthScaler);
		inventoryHeight = (int)(Screen.height * heightScaler);
		
		inventoryList = new ArrayList();
		
	}
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.I))
		{
			if(visible) {
				visible = false;
			}else{
				visible = true;
			}
		}
		
		if(Input.GetKeyDown(KeyCode.H)) {
			StartCoroutine("FieldView");
		}
		if(visible) {
			Camera.main.fieldOfView =  Mathf.Lerp(Camera.main.fieldOfView, 90, 0.05f);
		}else{
			Camera.main.fieldOfView =  Mathf.Lerp(Camera.main.fieldOfView, 179, 0.05f);
		}
	}
	
	void OnGUI() {
		if(visible) {
			GUI.Box(new Rect(0,0,inventoryWidth,inventoryHeight),"Inventory", style);
			drawTiles();
		}
	}
	
	void drawTiles() {
		for(int i = 0; i < inventoryList.Count; i++) {
			GameObject inventoryPart = (GameObject)inventoryList[i];
			
			Description partDescription = (Description)inventoryPart.GetComponent("Description");
			Debug.Log("Data found in the hashTable: " + partDescription.table["test"]);
			GUI.Button (new Rect((i*horizontalTileSpacing),0,tileSizeX,tileSizeY), partDescription.name);	
		}
			
	}
	
	void OnTriggerEnter(Collider other) {
		Destroy(other.gameObject);
		
		GameObject[] items = GameObject.FindGameObjectsWithTag(Tags.shipPart);
		
		int randChoice = Random.Range(0, items.Length - 1);
		
		Debug.Log("Length of parts found within game is : " + items.Length);
		Debug.Log ("Random choice is : " + randChoice);
		GameObject item = items[randChoice];
		
		Description partDescription = (Description)item.GetComponent("Description");
		partDescription.insert("test", "data");
		item.renderer.enabled = false;
		
		inventoryList.Add(item);
		
		GameObject ins = (GameObject)Instantiate(item, transform.position + partDescription.offset, item.transform.rotation);
		GameObject ins2 = (GameObject)Instantiate(item, transform.position + partDescription.changeOrientation(), item.transform.rotation);
		
		ins.renderer.enabled = true;
		ins.transform.parent = transform;
		
		ins2.renderer.enabled = true;
		ins2.transform.parent = transform;
	}
	
}
