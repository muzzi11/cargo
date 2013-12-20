using UnityEngine;
using System.Collections;

public class Planet
{
	public Vector2 position;
	public Economy economy = new Economy();
	public string name;

	public Planet(Vector2 position, string name)
	{
		this.position = position;
		this.name = name;
	}
}
