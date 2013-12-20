using UnityEngine;
using System.Collections;

public class NavigationState : State
{
	int width, height;
	private Ship ship;
	private Space space;
	private Planet planetDestination = null;
	private Vector2 worldPosition;
	private Vector3 screenPosition;

	private AuctionHouseState auctionHouseState; 
	private InventoryState inventoryState;
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
		this.inventoryState = new InventoryState(this, balance, cargo);
		worldPosition = new Vector2();
		screenPosition = new Vector3();

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

		float cameraHeight = -Camera.main.transform.position.z;
		screenPosition.Set(Input.mousePosition.x, Input.mousePosition.y, cameraHeight);
		worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

		GUILayout.BeginHorizontal();
		{
			if(GUILayout.Button(inventoryCaption))
			{
				inventoryState.LoadData();
				return inventoryState;
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
			GUI.ModalWindow(1, new Rect(0, height/4, width, height*0.6f), PlanetWindow, planetDestination.name);

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
					auctionHouseState.LoadPlanetaryInfo(planetDestination.economy, planetDestination.name);
					planetDestination = null;
					openAuctionHouse = true;
					inOrbit = false;
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}
