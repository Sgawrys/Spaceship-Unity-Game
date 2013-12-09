using UnityEngine;
using System.Collections;
using LibNoise.Unity.Generator;
using LibNoise.Unity;

public class WorldGenerator : MonoBehaviour {
	
	/*Size of the universe in terms of chunks, and a 3 dimensional chunk array which stores seeds*/
	public int size = 10;
	public int[,,] chunks;
	/*How big is the chunk?(In each x,y,z direction*/
	public float CHUNK_SIZE = 10000.0f;
	public float QUADRANT_SIZE = 5000.0f;
	
	/*Holds the various textures used by planets*/
	public Texture2D[] texturePool; 
	public Noise2D[] noisePool;
	public int NUM_TEXTURES = 5;
	
	/*How many planets in a solar system*/
	public int MAX_PLANETS = 15;
	public int MIN_PLANETS = 10;
	
	/*Sizes of these planets*/
	public float[] planetSize;
	
	/*Used for gathering the player's current location in 3D Universe*/
	public struct IntVector3 {
		public int x, y, z;
		
		public IntVector3(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
	
	/*Player's current chunk*/
	public IntVector3 player_pos;
	
	/*The seed of the current chunk that determines planet allocation*/
	public int currentSeed;
	
	// Use this for initialization
	void Start () { 
		planetSize = new float[4];
		planetSize [0] = 128.0f;
		planetSize [1] = 256.0f;
		planetSize [2] = 512.0f;
		planetSize [3] = 1024.0f;
		/*Generate textures for the planets to take from*/

		noisePool = new Noise2D[NUM_TEXTURES];
		for(int i = 0; i < noisePool.Length; i++) {
			noisePool[i] = generateNoise();	
		}
		
		Vector3 pos;
		
		int numPlanetsInChunk = Random.Range (MIN_PLANETS, MAX_PLANETS);
		
		/*Seed the initial universe*/
		chunks = new int[size, size, size];
		for(int x = 0; x < size; x++) {
			for(int y = 0; y < size; y++) {
				for(int z = 0; z < size; z++) {
					chunks[x,y,z] = (int)(Random.value * int.MaxValue);
				}
			}
		}
		/*Player starts out in center of universe, which loops around when reaches end*/
		player_pos = new IntVector3(size/2, size/2, size/2);

		/*Whats the current seed?*/
		currentSeed = chunks[player_pos.x, player_pos.y, player_pos.z];
		
		Vector3[] planetLocations = generatePlanetLocations(numPlanetsInChunk);
		
		for(int i = 0; i < numPlanetsInChunk; i++) {
			createPlanet(planetLocations[i]);
		}

		//createPlanet(new Vector3(1300.0f, 0.0f, -1100.0f));
	}
	
	Vector3[] generatePlanetLocations(int numPlanets) {

		Vector3[] planetLocations = new Vector3[numPlanets];

		/*Sun always in the middle of the solar system*/
		planetLocations [0] = new Vector3 (0.0f, -2000.0f, 0.0f);

		/*Parameter numPlanets determines the frequency of the spheres*/
		Spheres orbitGenerator = new Spheres(numPlanets);
		ModuleBase moduleBase = orbitGenerator;
		
		Noise2D orbitProjection = new Noise2D(256, moduleBase);
		orbitProjection.GeneratePlanar(Noise2D.Left, Noise2D.Right, Noise2D.Top, Noise2D.Bottom);
		
		/*Perlin noise for determining the height of the planet on it's orbit*/
		Perlin heightMap = new Perlin();
		heightMap.Seed = currentSeed;
		moduleBase = heightMap;
		
		Noise2D planetaryHeightMap = new Noise2D(256, moduleBase);
		planetaryHeightMap.GeneratePlanar(Noise2D.Left, Noise2D.Right, Noise2D.Top, Noise2D.Bottom);

		float offset = QUADRANT_SIZE / numPlanets;
		float x = 0.0f;
		float z = 0.0f;

		int ratio = (int)(QUADRANT_SIZE / 256.0f);
		for (int i = 1; i < planetLocations.Length; i++) {
			x+= offset;
			z+= offset;

			int xPos = (int)(x/ratio);
			int zPos = (int)(z/ratio);

			planetLocations[i] = new Vector3(x, planetaryHeightMap[xPos,zPos] * QUADRANT_SIZE, z);
		}
		return planetLocations;
	}
	
	void checkNewChunk() {
		GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
		if(Mathf.Abs(player.transform.position.x) >= QUADRANT_SIZE || Mathf.Abs(player.transform.position.y) >= QUADRANT_SIZE || Mathf.Abs(player.transform.position.z) >= QUADRANT_SIZE) {

			if(player.transform.position.x >= QUADRANT_SIZE) {
				player_pos.x++;
			}

			if(player.transform.position.y >= QUADRANT_SIZE) {
				player_pos.y++;
			}

			if(player.transform.position.z >= QUADRANT_SIZE) {
				player_pos.z++;
			}

			if(player.transform.position.x <= -QUADRANT_SIZE) {
				player_pos.x--;
			}
			
			if(player.transform.position.y <= -QUADRANT_SIZE) {
				player_pos.y--;
			}
			
			if(player.transform.position.z <= -QUADRANT_SIZE) {
				player_pos.z--;
			}

			player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
			
			GameObject[] destroy = GameObject.FindGameObjectsWithTag(Tags.planets);
			foreach(GameObject go in destroy) {
				Destroy(go);	
			}

			/*Player starts out in center of universe, which loops around when reaches end*/
			player_pos = new IntVector3(size/2, size/2, size/2);
			int numPlanetsInChunk = Random.Range (MIN_PLANETS, MAX_PLANETS);
			
			/*Whats the current seed?*/
			currentSeed = chunks[player_pos.x, player_pos.y, player_pos.z];
			
			Vector3[] planetLocations = generatePlanetLocations(numPlanetsInChunk);
			
			for(int i = 0; i < numPlanetsInChunk; i++) {
				createPlanet(planetLocations[i]);
			}

		}	
	}
	
	void Update()	{ 
		checkNewChunk();

		GameObject[] planets = GameObject.FindGameObjectsWithTag (Tags.planets);
		foreach (GameObject planet in planets) {
			StartCoroutine("RotateAroundSun", planet);
		}
	}

	IEnumerator RotateAroundSun(GameObject planet) {
		Description planetDesc = (Description)planet.GetComponent ("Description");

		planet.transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime);
		planet.transform.RotateAround(Vector3.zero, planet.transform.up, planetDesc.speed * Time.deltaTime);
		yield return new WaitForSeconds(0.1f);
	}
	
	void createPlanet(Vector3 position) {
		GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		planet.transform.position = position;
		
		float size = planetSize[Random.Range(0, planetSize.Length)];
		
		planet.transform.localScale = new Vector3(size,size,size);
		
		planet.tag = Tags.planets;
		planet.transform.parent = GameObject.FindGameObjectWithTag(Tags.generated).transform;
		planet.transform.Rotate (new Vector3 ((180 * Random.value) + 180.0f, 360.0f * Random.value, (180 * Random.value) + 180.0f));
		planet.transform.RotateAround (Vector3.zero, Vector3.up, 360.0f * Random.value);
		planet.collider.isTrigger = true;

		Noise2D randomNoise = noisePool[Random.Range(0,noisePool.Length)];
		generateGradient ();
		Texture2D planetTexture = randomNoise.GetTexture (LibNoise.Unity.Gradient.Custom);

		planet.renderer.material.mainTexture = planetTexture;

		Description planetDescription = (Description)planet.AddComponent("Description");
		planetDescription.table = new Hashtable ();
		planetDescription.angle = 360.0f * Random.value;
		planetDescription.speed = 10.0f * Random.value;
	}
	
	Noise2D generateNoise() {
		Perlin perl = new Perlin();
		perl.Seed = (int)(Random.value * int.MaxValue);
		ModuleBase mb = perl;

		Noise2D n2d = new Noise2D(256, 256, mb);

		n2d.GenerateSpherical(Noise2D.South, Noise2D.North, Noise2D.West, Noise2D.East);

		return n2d;
	}

	void generateGradient() {
		LibNoise.Unity.Gradient.Custom.EmptyGradient();
		
		Color initial = new Color (Random.value, Random.value, Random.value, 1.0f);
		
		for (double i = -1.0; i <= 1.0; i+= 0.2) {
			LibNoise.Unity.Gradient.Custom.AddToGradient(i, initial);
			initial = randomOffset(initial);
		}
	}

	Color randomOffset(Color color) {
		float offset = Random.value;
		Color newColor = new Color();
		newColor.r = (color.r + offset) % 1.0f;
		newColor.g = (color.g + offset) % 1.0f;
		newColor.b = (color.b + offset) % 1.0f;
		newColor.a = 1.0f;
		return newColor;
	}

}
