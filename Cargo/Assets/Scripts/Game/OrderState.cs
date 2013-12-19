using UnityEngine;
using System.Collections.Generic;

public interface OrderListener
{
	OrderState.Dialog BuyOrderPlaced(Order order);
	OrderState.Dialog SellOrderPlaced(Order order);
}

public class Order
{
	private int quantity, itemPrice;
	private Item item;

	public Order(Item item, int itemPrice, int quantity)
	{
		this.itemPrice = itemPrice;
		this.item = item;
		this.quantity = quantity;
	}

	public int Quantity
	{
		get { return quantity; }
	}

	public int Price
	{
		get { return quantity * itemPrice; }
	}

	public int ItemPrice
	{
		get { return itemPrice; }
	}

	public Item Item
	{
		get { return item; }
	}		
}

public class OrderState : State
{
	public enum OrderMode : int
	{
		Buy,
		Sell
	}
	private int mode;

	public enum Dialog : int
	{
		None,
		InsufficientFunds,
		InsufficientSpace,
		Confirmation,
		TransactionSummary
	}
	private Dialog currentDialog = Dialog.None;

	private readonly string[] titleCaptions = { "Buying {0}", "Selling {0}" },
						   orderCaptions = { "Buy", "Sell" },
						   placeOrderCaptions = { "buy", "sell"},
						   sumCaptions = { "Total cost: ${0}", "Profits: ${0}" };
	
	private const string nameCaption = "Name:",
				  		 volumeCaption = "Volume:",
				   	     weightCaption = "Weight:",
				   		 priceCaption = "Price:",
				   		 quantityCaption = "Quantity: x{0}",
				   		 balanceCaption = "Balance: ${0}",
						 confirmOrderCaption = "Are you sure you want to {0} {1} {2}?",
						 confirmTitleCaption = "Please confirm your transaction.",
						 insufficientFundsCaption = "Insufficient funds!",
						 dollazShortCaption = "You are {0} dollaz short.";

	private const string leftAlignedLabel = "leftAlignedLabel", normalLabel ="normalLabel";

	private float quantitySlider = 1.0f;
	private Vector2 scrollPosition;
	private State returnToState;
	private int width, height, orderValue, currentBalance;
	private bool returnToPrevState;
	private AuctionLot lot;
	private OrderListener listener;

	public OrderState(State returnToState, OrderListener listener, AuctionLot lot, int currentBalance, OrderMode mode)
	{
		this.returnToState = returnToState;
		this.lot = lot;
		this.currentBalance = currentBalance;
		this.mode = (int)mode;
		this.listener = listener;

		width = Screen.width;
		height = Screen.height;
	}

	public State UpdateState()
	{	
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}

		if(lot != null) GUI.Window(0, new Rect(0, 0, width, height), TransactionWindow, string.Format(titleCaptions[mode], lot.Item.Name));

		if(currentDialog == Dialog.Confirmation) GUI.ModalWindow(1, new Rect(0, height/4, width, height/2), ConfirmationWindow, confirmTitleCaption);
		else if(currentDialog == Dialog.InsufficientFunds) GUI.ModalWindow(2, new Rect(0, height/4, width, height/2), InsufficientFundsWindow, insufficientFundsCaption);
		
		return this;
	}

	private int UpdateBalance(int price)
	{
		if (mode == (int)OrderMode.Buy) return currentBalance - price;
		return currentBalance + price;
	}

	private void InsufficientFundsWindow(int ID)
	{
		int tooShort = orderValue - currentBalance;
		
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(string.Format(dollazShortCaption, tooShort), normalLabel);
			}
			GUILayout.FlexibleSpace();
			
			if(GUILayout.Button("Back")) currentDialog = Dialog.None;
		}
		GUILayout.EndVertical();
	}

	protected void TransactionWindow(int ID)
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);

			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(nameCaption, leftAlignedLabel, GUILayout.ExpandWidth(true));
					GUILayout.Label(volumeCaption, GUILayout.Width(65));
					GUILayout.Label(weightCaption, GUILayout.Width(65));
					GUILayout.Label(priceCaption, GUILayout.Width(55));
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(lot.Item.Name, leftAlignedLabel, GUILayout.ExpandWidth(true));
					GUILayout.Label(lot.Item.Volume.ToString(), GUILayout.Width(65));
					GUILayout.Label(lot.Item.Weight.ToString(), GUILayout.Width(65));
					GUILayout.Label("$" + lot.ItemPrice.ToString(), GUILayout.Width(55));
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();

			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical();
			{
				quantitySlider = GUILayout.HorizontalSlider(quantitySlider, 1, lot.Availability);
				int price = Mathf.RoundToInt (quantitySlider) * lot.ItemPrice;
				int remainder = UpdateBalance(price);

				GUILayout.Label(string.Format(quantityCaption, (int)quantitySlider), normalLabel, GUILayout.ExpandWidth(true));
				GUILayout.Label(string.Format(sumCaptions[mode], price), normalLabel);
				GUILayout.Label(string.Format(balanceCaption, remainder), normalLabel);
			}
			GUILayout.EndVertical();

			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back"))
				{	
					returnToPrevState = true;
				}
				if(GUILayout.Button(orderCaptions[mode])) currentDialog = Dialog.Confirmation; 
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	protected void ConfirmationWindow(int ID)
	{
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);	
			GUILayout.Label(string.Format(confirmOrderCaption, placeOrderCaptions[mode], Mathf.RoundToInt(quantitySlider), lot.Item.Name), normalLabel);
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) currentDialog = Dialog.None;
				if(GUILayout.Button(orderCaptions[mode]))
				{
					Order order = new Order(lot.Item, lot.ItemPrice, Mathf.RoundToInt(quantitySlider));

					if (mode == (int)OrderMode.Buy) currentDialog = listener.BuyOrderPlaced(order);
					else currentDialog = listener.SellOrderPlaced(order);
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}