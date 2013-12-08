using UnityEngine;
using System.Collections;

public class Ship
{
	private float speed = 3.0f;
	private Vector2 position, destination;

	public Ship(Vector2 position)
	{
		this.position = new Vector2(position.x, position.y);
		destination = new Vector2(position.x, position.y);
	}

	public Vector2 GetPosition()
	{
		return new Vector2(position.x, position.y);
	}

	public void SetDestination(float x, float y)
	{
		destination.Set(x, y);
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
