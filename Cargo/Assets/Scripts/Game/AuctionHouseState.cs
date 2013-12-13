using UnityEngine;
using System.Collections.Generic;

public class AuctionHouseState : State, OrderListener
{
	private int width, height;

	private State returnToState;
	private BuyState buyState;
	private SellState sellState;

	private string buyCaption = "Buy";
	private string sellCaption = "Sell";
	private string auctionHouseCaption = "Auction house";

	private Table table;
	private Order order;

	private bool returnToPrevState = false;
	private bool isBuying = true;
	public bool orderPlaced = false;

	public AuctionHouseState(State returnToState, Balance balance)
	{
		this.returnToState = returnToState;
		buyState = new BuyState(this, balance);
		sellState = new SellState(this, balance);

		width = Screen.width;
		height = Screen.height;
		table = new Table(this);

		table.LoadData(
			new List<ItemStack>()
			{
				new ItemStack()
				{
					item = new Item()
					{
						ID = 1, name = "Iron ore", weight = 50, volume = 29
					},
					quantity = 123
				},
				new ItemStack()
				{
					item = new Item()
					{
						ID = 2, name = "Adamantium", weight = 29, volume = 92
					},
					quantity = 123
				},
				new ItemStack()
				{
					item = new Item()
					{
						ID = 3, name = "Vibranium", weight = 134, volume = 2
					},
					quantity = 34
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
			sellState.order = buyState.order = order;
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

	public void ReceivedOrder(Order order)
	{
		orderPlaced = true;
		this.order = order;
	}
}