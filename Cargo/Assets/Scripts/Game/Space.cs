using UnityEngine;
using System.Collections.Generic;

public class Space : MonoBehaviour
{
	public int seed;
	public int planetCount;

	public GameObject planetPrefab;
	public Sprite[] planetSprites;

	private float planetRadiusSq;

	private List<Planet> planets = new List<Planet>();

	// Use this for initialization
	void Start()
	{
		planetRadiusSq = Mathf.Pow(planetSprites[0].bounds.extents.x, 2);
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	// Returns the planet at the given position, if it's within the planet's radius, otherwise returns null.
	public Planet PlanetAt(Vector2 position)
	{
		foreach(Planet planet in planets)
		{
			Vector2 diff = position - planet.position;
			if(diff.sqrMagnitude <= planetRadiusSq) return planet;
		}

		return null;
	}

	// TODO: remove seed
	public void GeneratePlanets(Rect bounds)
	{
		planets.Clear();

		Random.seed = seed;

		for(int i = 0; i < planetCount; ++i)
		{
			Vector2 position = new Vector2();
			
			do
			{
				position.x = Random.Range(bounds.xMin, bounds.xMax);
				position.y = Random.Range(bounds.yMin, bounds.yMax);
			} while(PlanetAt(position) != null);	// prevent planets from spawning on top of eachother
			
			var gameObject = Instantiate(planetPrefab, new Vector3(position.x, position.y), Quaternion.identity) as GameObject;
			// Cycle through planet sprites
			gameObject.GetComponent<SpriteRenderer>().sprite = planetSprites[i % planetSprites.Length];
			
			planets.Add(new Planet(position));
		}
	}
}