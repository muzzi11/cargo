using UnityEngine;
using System.Collections;

public class Ship
{
	private int damage;
	private int hull, maxHull, shield, maxShield;
	private float speed = 3.0f;
	private Vector2 position, destination;

	public Ship(Vector2 position)
	{
		this.position = new Vector2(position.x, position.y);
		destination = new Vector2(position.x, position.y);

		damage = 100;
		hull = maxHull = 300;
		shield = maxShield = 150;
	}

	public static Ship GenerateRandomShip(int level)
	{
		Ship ship = new Ship(Vector2.zero);
		int points = (level + 10) * 500 / 10;
		float damageFraction = Random.Range(0.1f, 0.4f);
		float damagePenalty = 0.3f;

		ship.damage = Mathf.RoundToInt(damageFraction * damagePenalty * points);
		ship.shield = ship.maxShield = Mathf.RoundToInt((1.0f - damageFraction) * points / 3.0f);
		ship.hull = ship.maxHull = 2 * ship.shield;

		return ship;
	}

	public int TakeDamage(int damage)
	{
		if(shield >= damage)
		{
			shield -= damage;
			return 0;
		}
		else
		{
			damage -= shield;
			shield = 0;
			hull -= damage;
			if(hull < 0) hull = 0;
			return damage;
		}
	}

	public bool Alive
	{
		get{ return hull > 0; }
	}

	public int Damage
	{
		get{ return damage; }
	}

	public int Hull
	{
		get{ return hull; }
	}

	public int MaxHull
	{
		get{ return maxHull; }
	}

	public int Shield
	{
		get{ return shield; }
	}

	public int MaxShield
	{
		get{ return maxShield; }
	}

	public void ReplenishShield()
	{
		shield = maxShield;
	}

	public void Stop()
	{
		destination.Set(position.x, position.y);
	}

	// Returns a copy of the ship's position
	public Vector2 Position
	{
		get{ return new Vector2(position.x, position.y); }
	}

	public Vector2 Destination
	{
		set{ destination.Set(value.x, value.y);}
	}

	private float Travel(float distance)
	{
		Vector2 delta = destination - position;

		if(delta.sqrMagnitude > 0)
		{
			Vector2 direction = delta.normalized;

			// Make sure we don't overshoot our target destination
			if(distance > delta.magnitude) distance = delta.magnitude;

			position += direction * distance;
		}
		else
		{
			distance = 0.0f;
		}

		return distance;
	}
	
	// Returns the distance traveled
	public float Update()
	{
		return Travel(speed * Time.deltaTime);
	}
}
