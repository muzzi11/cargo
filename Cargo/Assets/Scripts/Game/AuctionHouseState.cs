using UnityEngine;
using System.Collections;

public class AuctionHouseState : State 
{
	private int screenWidth, screenHeight;
	private float windowWidth, windowHeight;
	private State returnToState;
	private string buyCaption = "Buy";
	private string sellCaption = "Sell";
	private string quantityCaption = "Quantity";
	private string auctionHouseCaption = "Auction house";
	private string buttonStyle = "hudButton";
	
	private bool returnToPrevState = false;
	
	public AuctionHouseState(State returnToState)
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		windowWidth = (float)(screenWidth *0.8);
		windowHeight = (float)(screenHeight *0.7);
		this.returnToState = returnToState;
	}
	
	public State UpdateState()
	{			
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}

		GUI.Window(0, new Rect(screenWidth / 2 - windowWidth /2, (float)(screenHeight *0.1), windowWidth, windowHeight), AuctionHouseWindow, auctionHouseCaption);
		
		return this;
	}
	
	public void AuctionHouseWindow(int ID)
	{		
		GUILayout.BeginArea(new Rect(5, windowHeight-50, 200, 40));
		{
			if(GUILayout.Button("Back", "hudButton")) returnToPrevState = true;
		}
		GUILayout.EndArea();
		
		GUI.DragWindow(new Rect(0, 0, 10000, 10000));
	}
}
