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
	
	private float paddingTop = 60, paddingLeft = 5, paddingBottom = 10;
	private float buttonAreaHeight = 40;
	private float tableHeight;
	
	private bool returnToPrevState = false;
	private bool isBuying = true;
	
	public AuctionHouseState(State returnToState)
	{
		this.returnToState = returnToState;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		windowWidth = screenWidth;
		windowHeight = screenHeight;
		tableHeight = windowHeight - (buttonAreaHeight + paddingTop + paddingBottom);
		table = new Table((int)windowWidth, (int)tableHeight);
		
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
				},
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
				},
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
				},
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
				},
			},
			new List<int>()
			{
				167, 13, 24,167, 13, 24,167, 13, 24,167, 13, 24,
			},
			new List<int>()
			{
				12, 45, 137,12, 45, 137,12, 45, 137,12, 45, 137,
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
		
		GUI.Window(0, new Rect(screenWidth / 2 - windowWidth /2, 0, windowWidth, windowHeight), AuctionHouseWindow, auctionHouseCaption);		
		return this;
	}
	
	public void AuctionHouseWindow(int ID)
	{	
		GUILayout.BeginArea(new Rect(paddingLeft, paddingTop, windowWidth, tableHeight));
		{
			table.Render();
		}
		GUILayout.EndArea();				
		
		GUILayout.BeginArea(new Rect(paddingLeft, windowHeight-(buttonAreaHeight+paddingBottom), windowWidth, buttonAreaHeight));
		{
			if(GUI.Button(new Rect(0, 0, windowWidth/2, 40),"Back")) returnToPrevState = true;
			
			currentCaption = isBuying ? buyCaption : sellCaption;
			if(GUI.Button(new Rect(windowWidth/2, 0, windowWidth/2, 40),currentCaption)) 
				isBuying = (isBuying) ? false : true;			
		}
		GUILayout.EndArea();				
	}
}
