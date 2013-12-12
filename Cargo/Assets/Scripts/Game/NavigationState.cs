using UnityEngine;
using System.Collections;

public class NavigationState : State
{
	int width, height;
	private Ship ship;
	private Space space;
	private Planet planetDestination = null;
	private Vector2 screenPosition, worldPosition;

	private AuctionHouseState auctionHouseState; 
	private string inventoryCaption = "Inventory";	
	private string buttonStyle = "hudButton";


	public NavigationState(Space space, Ship ship)
	{
		screenPosition = new Vector2();
		worldPosition = new Vector2();
		auctionHouseState = new AuctionHouseState(this);

		width = Screen.width;
		height = Screen.height;

		this.ship = ship;
		this.space = space;
	}

	public State UpdateState()
	{
		screenPosition.Set(Input.mousePosition.x, Input.mousePosition.y);
		worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

		GUI.BeginGroup(new Rect(0, 0, width, 200));
		{
			if(GUI.Button(new Rect(0, 0, 128, 64), inventoryCaption)) return auctionHouseState;
		}
		GUI.EndGroup();
		
		if(Input.GetMouseButtonUp(0))
		{
			planetDestination = space.PlanetAt(worldPosition);
			ship.Destination = worldPosition;
		}

		if (Event.current.type == EventType.Repaint)
		{
			if(planetDestination != null && planetDestination == space.PlanetAt(ship.Position))
			{
				planetDestination = null;
				return auctionHouseState;			
			}
		}

		return this;
	}		
}
