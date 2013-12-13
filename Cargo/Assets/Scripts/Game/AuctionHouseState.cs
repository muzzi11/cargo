using UnityEngine;
using System.Collections.Generic;

public class AuctionHouseState : State, TableListener
{
	private int width, height;

	private State returnToState;

	private const string backCaption = "Back";
	private const string buyCaption = "Buy";
	private const string sellCaption = "Sell";
	private const string auctionHouseCaption = "Auction house";

	private Table table = new Table();
	private Order order = null;
	private Balance balance;
	private Economy economy;

	private bool returnToPrevState = false;
	private bool isBuying = true;

	public AuctionHouseState(State returnToState, Balance balance)
	{
		this.returnToState = returnToState;
		this.balance = balance;

		width = Screen.width;
		height = Screen.height;

		table.AddListener(this);
	}

	public void LoadEconomy(Economy economy)
	{
		this.economy = economy;

		var items = economy.GetItems();
		var quantities = new List<int>();
		var values = new List<int>();
		
		foreach(Item item in items)
		{
			quantities.Add(economy.GetQuantity(item.id));
			values.Add(economy.GetValue(item.id));
		}

		table.LoadData(items, quantities, values);
	}

	public void ItemClicked(int id)
	{
		order = new Order(ItemTable.GetName(id),
		                  economy.GetQuantity(id),
		                  economy.GetValue(id),
		                  ItemTable.GetVolume(id),
		                  ItemTable.GetWeight(id));
	}
	
	public State UpdateState()
	{			
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}

		if (order != null)
		{
			var tmpOrder = order;
			order = null;
			if(isBuying) return new BuyState(this, balance, tmpOrder);
			else return new SellState(this, balance, tmpOrder);
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
				if(GUILayout.Button(backCaption)) returnToPrevState = true;
				if(GUILayout.Button(isBuying ? buyCaption : sellCaption)) isBuying = !isBuying;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();			
	}
}
