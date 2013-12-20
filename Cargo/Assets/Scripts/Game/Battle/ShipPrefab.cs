using UnityEngine;
using System.Collections.Generic;

public class ShipPrefab : MonoBehaviour
{
	private Transform[] laserbeamTransforms;

	// Use this for initialization
	void Start()
	{
		laserbeamTransforms = GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	public List<Vector2> GetLaserbeamPositions()
	{
		var list = new List<Vector2>();

		foreach(var transform in laserbeamTransforms)
		{
			list.Add(transform.position);
		}

		return list;
	}
}
