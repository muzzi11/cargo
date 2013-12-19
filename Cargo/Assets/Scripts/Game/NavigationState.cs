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
	private const string inventoryCaption = "Inventory";
	private const string auctionHouseCaption = "Auction house";
	private const string welcomeCaption = "Welcome to the planet {0}. You honor us with your visit. " +
		"Please visit our auction house. Profit awaits you!.";
	private const string transparentStyle = "transparent";
	private const string normalStyle = "normalLabel";
	private bool inOrbit = false, openAuctionHouse = false;

	public NavigationState(Space space, Ship ship, Balance balance, Cargo cargo)
	{
		this.auctionHouseState = new AuctionHouseState(this, balance, cargo);
		screenPosition = new Vector2();
		worldPosition = new Vector2();

		width = Screen.width;
		height = Screen.height;

		this.ship = ship;
		this.space = space;
	}

	public State UpdateState()
	{
		if(openAuctionHouse)
		{
			openAuctionHouse = false;
			return auctionHouseState;
		}

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

		if(Event.current.type == EventType.Repaint)		
			if(planetDestination != null && planetDestination == space.PlanetAt(ship.Position))			
				inOrbit = true;

		if(inOrbit) 
			GUI.ModalWindow(1, new Rect(0, height/4, width, height*0.55f), PlanetWindow, planetDestination.name);

		return this;
	}

	private void PlanetWindow(int ID)
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);
						
			GUILayout.Label(string.Format(welcomeCaption, planetDestination.name), normalStyle);

			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) 
				{
					planetDestination = null;
					inOrbit = false;
				}
				if(GUILayout.Button(auctionHouseCaption))
				{
					auctionHouseState.LoadEconomy(planetDestination.economy);
					openAuctionHouse = true;
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}
