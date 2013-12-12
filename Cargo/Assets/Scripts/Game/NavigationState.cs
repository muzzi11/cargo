using UnityEngine;
using System.Collections;

public class NavigationState : State
{
	private Ship ship;
	private Space space;
	private Planet planetDestination = null;
	private Vector2 screenPosition, worldPosition;


	public NavigationState(Space space, Ship ship)
	{
		screenPosition = new Vector2();
		worldPosition = new Vector2();

		this.ship = ship;
		this.space = space;
	}

	public State UpdateState()
	{
		screenPosition.Set(Input.mousePosition.x, Input.mousePosition.y);
		worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
		
		if(Input.GetMouseButtonUp(0))
		{
			planetDestination = space.PlanetAt(worldPosition);
			ship.Destination = worldPosition;
		}

		if(planetDestination != null && planetDestination == space.PlanetAt(ship.Position))
		{
			// open auction house
		}

		return this;
	}
}
