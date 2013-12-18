using UnityEngine;
using System.Collections;

public class Ship
{
	private int damage;
	private int health, maxHealth, shield, maxShield;
	private float speed = 3.0f;
	private Vector2 position, destination;

	public Ship(Vector2 position)
	{
		this.position = new Vector2(position.x, position.y);
		destination = new Vector2(position.x, position.y);

		damage = 100;
		health = maxHealth = 300;
		shield = maxShield = 150;
	}

	// Returns a copy of the ship's position
	public Vector2 Position
	{
		get
		{
			return new Vector2(position.x, position.y); 
		}
	}

	public Vector2 Destination
	{
		set
		{
			destination.Set(value.x, value.y);
		}
	}

	private void Travel(float distance)
	{
		Vector2 delta = destination - position;
		if(delta.sqrMagnitude > 0)
		{
			Vector2 direction = delta.normalized;

			// Make sure we don't overshoot our target destination
			if(distance > delta.magnitude) distance = delta.magnitude;

			position += direction * distance;
		}
	}

	public void Update()
	{
		Travel(speed * Time.deltaTime);
	}
}
