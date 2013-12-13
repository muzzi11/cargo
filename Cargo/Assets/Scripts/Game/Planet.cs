using UnityEngine;
using System.Collections;

public class Planet
{
	public Vector2 position;
	public Economy economy = new Economy();

	public Planet(Vector2 position)
	{
		this.position = position;
	}
}
