using UnityEngine;
using System.Collections.Generic;
using System;

public class AuctionHouseState : State 
{
	private int width, height;

	private State returnToState;
	private BuyState buyState;
	private SellState sellState;

	private string buyCaption = "Buy";
	private string sellCaption = "Sell";
	private string auctionHouseCaption = "Auction house";

	private Table table;
	private OrderListener listener;

	private bool returnToPrevState = false;
	private bool isBuying = true;
	private bool orderPlaced = false;

	public AuctionHouseState(State returnToState)
	{
		this.returnToState = returnToState;
		buyState = new BuyState(this);
		sellState = new SellState(this);

		width = Screen.width;
		height = Screen.height;
		table = new Table();
		listener = new OrderListener();
		listener.Subscribe(table);

		table.LoadData(
			new List<Item>()
			{
				new Item()
				{
					ID = 1, Name = "Iron ore", Quantity = 123
				},
				new Item()
				{
					ID = 2, Name = "Adamantium", Quantity = 42
				},
				new Item()
				{
					ID = 3, Name = "Vibranium", Quantity = 31
				}				
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

		if (orderPlaced)
		{
			orderPlaced = false;
			sellState.order = buyState.order = listener.order;
			return isBuying ? (State)buyState : (State)sellState;
		}
		
		GUI.Window(0, new Rect(0, 0, width, height), AuctionHouseWindow, auctionHouseCaption);

		return this;
	}
	
	public void AuctionHouseWindow(int ID)
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);

			orderPlaced = table.Render();

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

public class Order : EventArgs
{
	public Item item;
	public int value;
}

public class OrderListener
{
	public Order order;

	public void Subscribe(Table table)
	{
		table.orderPlaced += new Table.OrderHandler (ReceivedOrder);
	}

	private void ReceivedOrder(Order order)
	{
		this.order = order;
	}	
}

