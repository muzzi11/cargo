﻿using UnityEngine;
using System.Collections.Generic;

public class AuctionLot
{
	private Item item;
	private int itemPrice, availability;
	private string origin;

	public AuctionLot(Item item, int availability, int itemPrice, string origin)
	{
		this.item = item;
		this.itemPrice = itemPrice;
		this.availability = availability;
		this.origin = origin;
	}

	public Item Item
	{
		get { return item; }
	}
	public int ItemPrice
	{
		get { return itemPrice; }
	}
	public int Availability
	{
		get { return availability; }
	}
	public string Origin
	{ 
		get { return origin; }
	}
}

public class AuctionHouseState : State, ItemTableListener, OrderListener
{
	private int width, height;
	private OrderState.OrderMode orderMode;

	private State returnToState;

	private const string backCaption = "Back";
	private const string buyCaption = "Buy";
	private const string sellCaption = "Sell";
	private const string auctionHouseCaption = "Auction house";
	private const string balanceCaption = "Balance: {0}";
	private const string cargoCaption = "Cargo space: {0}";
	private const string normalLabel = "normalLabel";
	private string planetName;

	private ItemTable table = new ItemTable();
	private Economy economy;
	private OrderState orderState = null;
	private Balance balance;
	private Cargo cargo;

	private bool returnToPrevState = false;

	private OrderState.OrderMode OrderMode
	{
		set { orderMode = value; LoadTable(); }
	}

	public AuctionHouseState(State returnToState, Balance balance, Cargo cargo)
	{
		this.returnToState = returnToState;
		this.balance = balance;
		this.cargo = cargo;

		width = Screen.width;
		height = Screen.height;
		orderMode = OrderState.OrderMode.Buy; 
		table.AddListener(this);
	}

	public void LoadPlanetaryInfo(Economy economy, string planetName)
	{
		this.economy = economy;
		this.planetName = planetName;
		LoadTable();
	}

	public void ItemClicked(Item item)
	{
		int availability;
		if (orderMode == OrderState.OrderMode.Buy) availability = economy.GetQuantity(item);
		else availability = cargo.GetQuantity(item);

		AuctionLot lot = new AuctionLot(item, availability, economy.GetPrice(item), planetName);
		orderState = new OrderState(this, this, lot, orderMode, balance.GetBalance(), cargo.GetRemainingSpace());
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
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);

			string[] toolbarStrings = {buyCaption, sellCaption};
			OrderMode = (OrderState.OrderMode)GUILayout.Toolbar((int)orderMode, toolbarStrings);

			table.Render();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(string.Format(balanceCaption, balance.GetBalance()), normalLabel);
				GUILayout.Label(string.Format(cargoCaption, cargo.GetRemainingSpace()), normalLabel);
			}
			GUILayout.EndHorizontal();

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
		if (orderMode == OrderState.OrderMode.Buy)
		{
			List<Item> items = economy.GetAllPlanetaryItems();
			table.LoadData(items, economy.GetQuantities(), economy.GetPrices(items));
		}
		else
		{
			List<Item> items = cargo.GetItems();
			table.LoadData(items, cargo.GetQuantities(), economy.GetPrices(items));
		}
	}

	public OrderState.Dialog BuyOrderPlaced(Order order)
	{
		if (!balance.Withdraw(order.Price)) return OrderState.Dialog.InsufficientFunds;
		if (!cargo.AddItem(order.Item, order.Quantity, order.ItemPrice, order.Origin)) return OrderState.Dialog.InsufficientSpace;
		economy.Consume(order.Item, order.Quantity);
		return OrderState.Dialog.TransactionSummary;
	}
	
	public OrderState.Dialog SellOrderPlaced(Order order)
	{
		balance.Deposit(order.Price);
		cargo.RemoveItem(order.Item, order.Quantity, order.Origin);
		economy.Supply(order.Item, order.Quantity);
		return OrderState.Dialog.TransactionSummary;
	}
}

