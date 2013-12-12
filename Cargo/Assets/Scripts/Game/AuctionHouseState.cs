using UnityEngine;
using System.Collections.Generic;

public class AuctionHouseState : State 
{
	private int screenWidth, screenHeight;
	private float windowWidth, windowHeight;		
	private State returnToState;
	private string buyCaption = "Buy";
	private string sellCaption = "Sell";
	private string currentCaption;
	private string quantityCaption = "Quantity";
	private string auctionHouseCaption = "Auction house";
	private string buttonStyle = "hudButton";
	private Table table;
	
	private bool returnToPrevState = false;
	private bool isBuying = true;
	
	public AuctionHouseState(State returnToState)
	{
		this.returnToState = returnToState;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		windowWidth = screenWidth *0.8f;
		windowHeight = screenHeight *0.7f;
		table = new Table(new Rect(10, 60, windowWidth - 20, windowHeight - 40));
		
		table.LoadData(
			new List<Item>()
			{
			new Item()
			{
				Name = "Iron ore"
			},
			new Item()
			{
				Name = "Adamantium"
			},
			new Item()
			{
				Name = "Vibranium"
			}
			},
			new List<int>()
			{
				167, 13, 24
			},
			new List<int>()
			{
				12, 45, 137
			}
		);
	}
	
	public State UpdateState()
	{			
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}
		
		GUI.Window(0, new Rect(screenWidth / 2 - windowWidth /2, screenHeight *0.1f, windowWidth, windowHeight), AuctionHouseWindow, auctionHouseCaption);		
		return this;
	}
	
	public void AuctionHouseWindow(int ID)
	{	
		table.Render();
		
		GUILayout.BeginArea(new Rect(5, windowHeight-40, windowWidth, 40));
		{
			if(GUI.Button(new Rect(0, 0, windowWidth/2, 40),"Back", "hudButton")) returnToPrevState = true;
			
			currentCaption = isBuying ? buyCaption : sellCaption;
			if(GUI.Button(new Rect(windowWidth/2, 0, windowWidth/2, 40),currentCaption, "hudButton")) 
				isBuying = (isBuying) ? false : true;			
		}
		GUILayout.EndArea();				
	}
}
