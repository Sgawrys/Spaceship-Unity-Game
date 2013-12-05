using UnityEngine;
using System.Collections;
using LibNoise.Unity.Generator;
using LibNoise.Unity;

public class WorldGenerator : MonoBehaviour {
	
	public int size = 10;
	public int[,,] chunks;
	
	public int currentChunk;
	
	public Texture2D[] texturePool; 
	
	public float[] planetSize = {128.0f, 256.0f, 512.0f, 1024.0f};
	
	// Use this for initialization
	void Start () { 
		
		texturePool = new Texture2D[1];
		for(int i = 0; i < texturePool.Length; i++) {
			texturePool[i] = generateTexture();	
		}
		
		Vector3 pos;
		
		int numPlanetsInChunk = Random.Range (10,25);
		int currentSeed;
		
		for(int i = 0; i < numPlanetsInChunk; i++) {
			float deltaX = Random.value;
			float deltaY = Random.value;
			float deltaZ = Random.value;
			
			float posX = (deltaX * 10000.0f) - 5000.0f;
			float posY = (deltaY * 10000.0f) - 5000.0f;
			float posZ = (deltaZ * 10000.0f) - 5000.0f;
			
			pos = new Vector3(posX,posY,posZ);
			createPlanet(pos);
		}

		createPlanet(new Vector3(1300.0f, 0.0f, -1100.0f));
	}
	
	void checkNewChunk() {
		GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
		if(Mathf.Abs(player.transform.position.x) >= 5000.0f || Mathf.Abs(player.transform.position.y) >= 5000.0f || Mathf.Abs(player.transform.position.y) >= 5000.0f) {
			player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
			
			GameObject[] destroy = GameObject.FindGameObjectsWithTag(Tags.planets);
			foreach(GameObject go in destroy) {
				Destroy(go);	
			}
			
			Vector3 pos;
			
			int numPlanetsInChunk = Random.Range (20,35);
			int currentSeed;
			
			for(int i = 0; i < numPlanetsInChunk; i++) {
				float deltaX = Random.value;
				float deltaY = Random.value;
				float deltaZ = Random.value;
				
				float posX = (deltaX * 10000.0f) - 5000.0f;
				float posY = (deltaY * 10000.0f) - 5000.0f;
				float posZ = (deltaZ * 10000.0f) - 5000.0f;
				
				pos = new Vector3(posX,posY,posZ);
				createPlanet(pos);
			}
		}	
	}
	
	void Update()	{ 
		checkNewChunk();
	}
	
	void createPlanet(Vector3 position) {
		GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		planet.transform.position = position;
		
		float size = planetSize[Random.Range(0, planetSize.Length)];
		
		planet.transform.localScale = new Vector3(size,size,size);
		
		planet.tag = Tags.planets;
		planet.transform.parent = GameObject.FindGameObjectWithTag(Tags.generated).transform;
		planet.collider.isTrigger = true;
		planet.renderer.material.mainTexture = texturePool[Random.Range(0,texturePool.Length)];
	}
	
	Texture2D generateTexture() {
		/*
		var texture = new Texture2D(1024, 1024, TextureFormat.ARGB32, true);
		
		float[,] perlin = GeneratePerlinNoise(GenerateWhiteNoise(1024,1024), 6);
		
		Color[] pix = new Color[texture.width * texture.height];
		
		Color c;
		
		Color color_one = new Color(Random.value, Random.value, Random.value, 1.0f);
		Color color_two = new Color(Random.value, Random.value, Random.value, 1.0f);
		Color color_three = new Color(Random.value, Random.value, Random.value, 1.0f);
		Color color_four = new Color(Random.value, Random.value, Random.value, 1.0f);
		
		for(int x = 0; x < texture.width; x++)
		{
			for(int y = 0; y < texture.height; y++)
			{
				if(perlin[x,y] >= 0.5f) {
					c = GetColor (color_one, color_two, perlin[x,y] - 0.5f, 0.25f);	
				}else{
					c = GetColor(color_three, color_four, perlin[x,y], 0.7f);
				}
				
				int pos = (int)(x * (texture.height-1) + y);
				pix[pos] = c;
			}
		}
		
		texture.SetPixels(pix);
		
		 // Apply all SetPixel calls
	    texture.Apply();
		*/
		ModuleBase mb = new Perlin();
		//LibNoise.Unity.Gradient gradient = new LibNoise.Unity.Gradient(new Color(1.0f, 0.0f, 0.0f, 1.0f), new Color(0.0f, 1.0f, 0.0f, 1.0f));
		Noise2D n2d = new Noise2D(256, 256, mb);
		n2d.GenerateSpherical(0.0, 511.0, 0.0, 511.0);
		return n2d.GetTexture(LibNoise.Unity.Gradient.Terrain);
		
		//return texture;
	}
	
	//Perlin Noise with Blending
	
	float[,] GenerateWhiteNoise(int width, int height)
	{
	    float[,] noise = new float[width, height];
	    for (int i = 0; i < width; i++)
	    {
	        for (int j = 0; j < height; j++)
	        {
	            noise[i,j] = ((float)Random.value) % 1;
	        }
	    }
	    return noise;
	}
	
	float[,] GenerateSmoothNoise(float[,] baseNoise, int octave)
	{
	   int width = 1024;
	   int height = 1024;
	 
	   float[,] smoothNoise = new float[width, height];
	 
	   int samplePeriod = 1 << octave; // calculates 2 ^ k
	   float sampleFrequency = 1.0f / samplePeriod;
	 
	   for (int i = 0; i < width; i++)
	   {
	      //calculate the horizontal sampling indices
	      int sample_i0 = (i / samplePeriod) * samplePeriod;
	      int sample_i1 = (sample_i0 + samplePeriod) % width; //wrap around
	      float horizontal_blend = (i - sample_i0) * sampleFrequency;
	 
	      for (int j = 0; j < height; j++)
	      {
	         //calculate the vertical sampling indices
	         int sample_j0 = (j / samplePeriod) * samplePeriod;
	         int sample_j1 = (sample_j0 + samplePeriod) % height; //wrap around
	         float vertical_blend = (j - sample_j0) * sampleFrequency;
	 
	         //blend the top two corners
	         float top = Interpolate(baseNoise[sample_i0,sample_j0],
	            baseNoise[sample_i1,sample_j0], horizontal_blend);
	 
	         //blend the bottom two corners
	         float bottom = Interpolate(baseNoise[sample_i0,sample_j1],
	            baseNoise[sample_i1,sample_j1], horizontal_blend);
	 
	         //final blend
	         smoothNoise[i,j] = Interpolate(top, bottom, vertical_blend);
	      }
	   }
	 
	   return smoothNoise;
	}
	
	float Interpolate(float x0, float x1, float alpha)
	{
	   return (x0 * (1 - alpha)) + (alpha * x1);
	}
	
	float[,] GeneratePerlinNoise(float[,] baseNoise, int octaveCount)
	{
	   int width = 1024;
	   int height = 1024;
	 
	   float[][,] smoothNoise = new float[octaveCount][,]; //an array of 2D arrays containing
	 
	   float persistance = 0.9f;
	 
	   //generate smooth noise
	   for (int i = 0; i < octaveCount; i++)
	   {
	       smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
	   }
	 
	    float[,] perlinNoise = new float[width, height];
	    float amplitude = 1.0f;
	    float totalAmplitude = 0.0f;
	 
	    //blend noise together
	    for (int octave = octaveCount - 1; octave >= 0; octave--)
	    {
	       amplitude *= persistance;
	       totalAmplitude += amplitude;
	 
	       for (int i = 0; i < width; i++)
	       {
	          for (int j = 0; j < height; j++)
	          {
	             perlinNoise[i,j] += smoothNoise[octave][i,j] * amplitude;
	          }
	       }
	    }
	 
	   //normalisation
		Debug.Log(totalAmplitude);
	   for (int i = 0; i < width; i++)
	   {
	      for (int j = 0; j < height; j++)
	      {
	         perlinNoise[i,j] /= totalAmplitude;
	      }
		}
		
	   return perlinNoise;
	}
	
	Color GetColor(Color gradientStart, Color gradientEnd, float t, float range) {
		t = t/range;
		
		float u = 1 - t;
		
		float rc = gradientStart.r * u + gradientEnd.r * t;
		float gc = gradientStart.g * u + gradientEnd.g * t;
		float bc = gradientStart.b * u + gradientEnd.b * t;
		
		Color color = new Color(rc, gc, bc, 1.0f);
		
		return color;
	}
}
