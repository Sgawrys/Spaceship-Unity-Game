using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
        
    private ArrayList inventoryList;
    
    
    /*Inventory variables*/
    public Rect inventoryRect = new Rect(0.0f, 0.0f, 320.0f, 320.0f);
    
    public float widthScaler = 0.4f;
    public float heightScaler = 0.4f;
    
    public int horizontalTileSpacing = 85;
    public int verticalTileSpacing = 0;
    
    public int tileSizeX = 80;
    public int tileSizeY = 80;
    
    public int inventoryWidth;
    public int inventoryHeight;
    
    public GUIStyle style = new GUIStyle();
    
    public Texture buttonTexture;
    
    public bool visible;
    
    /*Scroll Panel in the inventory view*/
    private Vector2 scrollVector = Vector2.zero;
    
    /*Customization variables*/
    public bool customization = false;
    
    public Rect customizationRect = new Rect(Screen.width*0.4f, 0.0f, 320.0f, 320.0f);
    
    public GameObject selectedPart; /*The GameObject we are currently modifying*/
    
    public float redValue = 1.0f;
    public float blueValue = 0.0f;
    public float greenValue = 0.0f;
    
    // Use this for initialization
    void Start () {
            /*Initialize the arraylist of the item ids that are currently located in this array list*/
            //inventoryList = new ArrayList<GameObject>();
            inventoryWidth = (int)(Screen.width * widthScaler);
            inventoryHeight = (int)(Screen.height * heightScaler);
            
            inventoryList = new ArrayList();
            
            selectedPart = null;
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
    }
    
    void OnGUI() {
            if(visible) {
                    inventoryRect = GUI.Window(1, inventoryRect, InventoryFunction, "");
                    
                    if(customization) {
                            drawCustomization();
                    }
            }else{
                    selectedPart = null;        
            }
    }
    
    void InventoryFunction(int windowId) {
            drawTiles();
            
            GUI.DragWindow(new Rect(0,0,1000,1000));
    }
    
    
    void changePartColor(float r, float g, float b) {
            
            if(selectedPart != null) {
                    selectedPart.renderer.material.color = new Color(r, g, b, 1.0f);
            }
    }
    
    void drawTiles() {
            /*Inventory objects will be drawn inside the scroll view , which size is determined by number of objects in the inventory*/
            
            int scrollViewHeight = ((inventoryList.Count / 3) + 1) * 256;
            
            scrollVector = GUI.BeginScrollView(new Rect(20,20,280,280), scrollVector, new Rect(20,20,260,scrollViewHeight));
            
            for(int i = 0; i < inventoryList.Count; i++) {
                    GameObject inventoryPart = (GameObject)inventoryList[i];
                    
                    Description partDescription = (Description)inventoryPart.GetComponent("Description");
                    
                    
                    int currentLevelX = i%3;
                    int currentLevelY = i/3;
                    
                    int buttonX = (currentLevelX * horizontalTileSpacing) + 20;
                    int buttonY = (verticalTileSpacing * currentLevelY) + 20;
                    
                    Rect buttonPosition = new Rect(buttonX, buttonY, 80, 40);
                    
                    
                    if(GUI.Button (buttonPosition, partDescription.name)) {
                            customization = true;
                            
                            selectedPart = inventoryPart;
                            
                            redValue = selectedPart.renderer.material.color.r;
                            greenValue = selectedPart.renderer.material.color.g;
                            blueValue = selectedPart.renderer.material.color.b;
                    }
            }
            
            GUI.EndScrollView();
    }
    
    void drawCustomization() {
            customizationRect = GUI.Window(0, customizationRect, CustomizationFunction, "");
            changePartColor(redValue, greenValue, blueValue);
    }
    
    void CustomizationFunction(int windowId) {
            
            redValue = GUI.HorizontalSlider(new Rect(16,100,200,20), redValue, 0.0f, 1.0f);
            greenValue = GUI.HorizontalSlider(new Rect(16,120,200,20), greenValue, 0.0f, 1.0f);
            blueValue = GUI.HorizontalSlider(new Rect(16,140,200,20), blueValue, 0.0f, 1.0f);
            
            GUI.Label(new Rect(220,100,64,32), "Red");
            GUI.Label(new Rect(220,120,64,32), "Green");
            GUI.Label(new Rect(220,140,64,32), "Blue");
            
            
            if(GUI.Button(new Rect(16,16,60,60), "Ok")){
                    customization = false;        
            }
            
            GUI.DragWindow(new Rect(0,0,1000,1000));
    }
    
    void OnTriggerEnter(Collider other) {
        if(other.tag == Tags.capsule) {
            Destroy(other.gameObject);
            
            GameObject[] items = GameObject.FindGameObjectsWithTag(Tags.shipPart);
            
            int randChoice = Random.Range(0, items.Length);
            
            Debug.Log("Length of parts found within game is : " + items.Length);
            Debug.Log ("Random choice is : " + randChoice);
            GameObject item = items[randChoice];
            
            Description partDescription = (Description)item.GetComponent("Description");
            item.renderer.enabled = false;
            
            GameObject ins = (GameObject)Instantiate(item, transform.position + partDescription.offset, item.transform.rotation);
            ins.tag = Tags.shipPartInventory;
            inventoryList.Add(ins);
            
            ins.renderer.enabled = true;
            ins.transform.parent = transform;
        }
    }
        
}