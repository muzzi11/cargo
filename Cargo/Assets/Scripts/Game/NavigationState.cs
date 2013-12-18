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
	private string transparentStyle = "transparent";

	public NavigationState(Space space, Ship ship, OrderListener listener, Balance balance, Cargo cargo)
	{
		this.auctionHouseState = new AuctionHouseState(this, listener, balance, cargo);
		screenPosition = new Vector2();
		worldPosition = new Vector2();

		width = Screen.width;
		height = Screen.height;

		this.ship = ship;
		this.space = space;
	}

	public State UpdateState()
	{
		screenPosition.Set(Input.mousePosition.x, Input.mousePosition.y);
		worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

		GUILayout.BeginHorizontal();
		{
			if(GUILayout.Button(inventoryCaption))
			{
				return this;
			}
			else
			{
				if(GUI.Button(new Rect(0, 0, width, height), string.Empty, transparentStyle))
				{
					planetDestination = space.PlanetAt(worldPosition);
					ship.Destination = worldPosition;
				}
			}
		}
		GUILayout.EndHorizontal();

		if (Event.current.type == EventType.Repaint)
		{
			if(planetDestination != null && planetDestination == space.PlanetAt(ship.Position))
			{
				auctionHouseState.LoadEconomy(planetDestination.economy);
				planetDestination = null;
				return auctionHouseState;			
			}
		}

		return this;
	}
}
