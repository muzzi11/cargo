using UnityEngine;
using System.Collections.Generic;

public class AuctionHouseState : State, ItemTableListener
{
	private int width, height, orderMode;

	private State returnToState;

	private const string backCaption = "Back";
	private const string buyCaption = "Buy";
	private const string sellCaption = "Sell";
	private const string auctionHouseCaption = "Auction house";
	
	private ItemTable table = new ItemTable();
	private Economy economy;
	private OrderState orderState = null;
	private OrderListener listener;
	private Balance balance;
	private Cargo cargo;

	private bool returnToPrevState = false;

	public AuctionHouseState(State returnToState, OrderListener listener, Balance balance, Cargo cargo)
	{
		this.returnToState = returnToState;
		this.listener = listener;
		this.balance = balance;
		this.cargo = cargo;

		width = Screen.width;
		height = Screen.height;
		orderMode = (int)OrderState.OrderMode.Buy; 
		table.AddListener(this);
	}

	public void LoadEconomy(Economy economy)
	{
		this.economy = economy;
	}

	public void ItemClicked(Item item)
	{
		Order order = new Order(item,
		                  economy.GetQuantity(item),
		                  economy.GetPrice(item));
		orderState = new OrderState (this, listener, order, balance.GetBalance(), orderMode);
	}
	
	public State UpdateState()
	{			
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}

		if (orderState != null)
		{
			var state = orderState.UpdateState();
			orderState = null;
			return state;
		}

		GUI.Window(0, new Rect(0, 0, width, height), AuctionHouseWindow, auctionHouseCaption);

		return this;
	}
	
	public void AuctionHouseWindow(int ID)
	{
		LoadTable();
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);

			string[] toolbarStrings = {buyCaption, sellCaption};
			orderMode = GUILayout.Toolbar(orderMode, toolbarStrings);

			table.Render();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button(backCaption)) returnToPrevState = true;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();			
	}

	private void LoadTable()
	{
		if (orderMode == (int) OrderState.OrderMode.Buy)
			table.LoadData(economy.GetItems(), economy.GetQuantities(), economy.GetPrices());
		else
		{
			List<Item> items = cargo.GetItems();
			List<int> prices = new List<int>();

			foreach(Item item in items)
				prices.Add(economy.GetPrice(item));

			table.LoadData(items, cargo.GetQuantities(), prices);
		}
	}
}

