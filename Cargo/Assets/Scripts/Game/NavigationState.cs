using UnityEngine;
using System.Collections;

public class NavigationState : State
{
	int width, height;
	private Ship ship;
	private Vector3 cursorPos = new Vector3();
	private AuctionHouseState auctionHouseState; 
	private string inventoryCaption = "Inventory";	
	private string buttonStyle = "hudButton";
	
	public NavigationState(Ship ship)
	{
		width = Screen.width;
		height = Screen.height;
		this.ship = ship;
		auctionHouseState = new AuctionHouseState(this);
	}

	public State UpdateState()
	{
		GUI.BeginGroup(new Rect(0, 0, width, 100));
		{
			if(GUI.Button(new Rect(0, 0, 200, 40), inventoryCaption, buttonStyle)) return auctionHouseState;
		}
		GUI.EndGroup();
		
		cursorPos.Set(Input.mousePosition.x, Input.mousePosition.y, 0);
		cursorPos = Camera.main.ScreenToWorldPoint(cursorPos);
		
		if(Input.GetMouseButtonUp(0)) ship.SetDestination(cursorPos.x, cursorPos.y);

		return this;
	}
}
