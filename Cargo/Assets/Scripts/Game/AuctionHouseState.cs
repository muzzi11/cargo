using UnityEngine;
using System.Collections.Generic;

public class AuctionHouseState : State 
{
	private int width, height;

	private State returnToState;

	private string buyCaption = "Buy";
	private string sellCaption = "Sell";
	private string quantityCaption = "Quantity";
	private string auctionHouseCaption = "Auction house";

	private Table table;
	
	private bool returnToPrevState = false;
	private bool isBuying = true;
	
	public AuctionHouseState(State returnToState)
	{
		this.returnToState = returnToState;
		width = Screen.width;
		height = Screen.height;
		table = new Table(width);
		
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
		
		GUI.Window(0, new Rect(0, 0, width, height), AuctionHouseWindow, auctionHouseCaption);

		return this;
	}
	
	public void AuctionHouseWindow(int ID)
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);

			table.Render();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) returnToPrevState = true;
				if(GUILayout.Button(isBuying ? buyCaption : sellCaption)) isBuying = !isBuying;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();			
	}
}
