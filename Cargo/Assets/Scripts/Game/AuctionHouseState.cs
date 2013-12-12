using UnityEngine;
using System.Collections;

public class AuctionHouseState : State 
{
	private int width, height;
	private State returnToState;
	private string buyCaption = "Buy";
	private string sellCaption = "Sell";
	private string quantityCaption = "Quantity";
	private string auctionHouseCaption = "Auction house";
	private string buttonStyle = "hudButton";
	
	private bool returnToPrevState = false;
	
	public AuctionHouseState(State returnToState)
	{
		width = Screen.width;
		height = Screen.height;
		this.returnToState = returnToState;
	}
	
	public State UpdateState()
	{
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}
		
		GUI.Window(0, new Rect(width / 2 - 100, height - 200, 200, 100), AuctionHouseWindow, auctionHouseCaption);
		
		return this;
	}
	
	public void AuctionHouseWindow(int ID)
	{
		GUILayout.BeginArea(new Rect(0, 0, 200, 40));
		{
			if(GUILayout.Button("Back", "hudButton")) returnToPrevState = true;
		}
		
		GUI.DragWindow(new Rect(0, 0, 10000, 10000));
	}
}
