using UnityEngine;
using System.Collections.Generic;

public interface OrderListener
{
	bool buyOrderPlaced(Order order);
	void sellOrderPlaced(Order order);
}

public class Order
{
	private int quantity, sum, availability, itemPrice;
	private Item item;

	public Order(Item item, int availability, int itemPrice)
	{
		this.availability = availability;
		this.itemPrice = itemPrice;
		this.item = item;
	}

	public int Quantity
	{
		get { return quantity; }
		set
		{
			quantity = value;
			sum = quantity * itemPrice;
		}
	}

	public int Sum
	{
		get { return sum; }
	}	

	public int Availability
	{
		get { return availability; }
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
	private bool returnToPrevState, orderPlaced = false, sufficientFunds = true;
	private Order order;
	private OrderListener listener;

	public OrderState(State returnToState, OrderListener listener, Order order, int currentBalance, int mode)
	{
		this.returnToState = returnToState;
		this.order = order;
		this.currentBalance = currentBalance;
		this.mode = mode;
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
		
		if(!sufficientFunds) GUI.ModalWindow(1, new Rect(0, height/4, width, height/2), InsufficientFundsWindow, insufficientFundsCaption);
		if(orderPlaced) GUI.ModalWindow(2, new Rect(0, height/4, width, height/2), ConfirmationWindow, confirmTitleCaption);
		
		if(order != null) GUI.Window(3, new Rect(0, 0, width, height), TransactionWindow, string.Format(titleCaptions[mode], order.Item.Name));
		
		return this;
	}

	private int UpdateBalance()
	{
		if (mode == (int)OrderMode.Buy) return currentBalance - order.Sum;
		return currentBalance + order.Sum;
	}

	private void InsufficientFundsWindow(int ID)
	{
		int toShort = orderValue - currentBalance;
		
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);
			GUILayout.Label(string.Format(dollazShortCaption, toShort), normalLabel);
			GUILayout.FlexibleSpace();
			
			if(GUILayout.Button("Back")) sufficientFunds = false;
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
					GUILayout.Label(order.Item.Name, leftAlignedLabel, GUILayout.ExpandWidth(true));
					GUILayout.Label(order.Item.Volume.ToString(), GUILayout.Width(65));
					GUILayout.Label(order.Item.Weight.ToString(), GUILayout.Width(65));
					GUILayout.Label("$" + order.ItemPrice.ToString(), GUILayout.Width(55));
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();

			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical();
			{
				quantitySlider = GUILayout.HorizontalSlider(quantitySlider, 1, order.Availability);
				order.Quantity = Mathf.RoundToInt(quantitySlider);
				int remainder = UpdateBalance();

				GUILayout.Label(string.Format(quantityCaption, (int)quantitySlider), normalLabel, GUILayout.ExpandWidth(true));
				GUILayout.Label(string.Format(sumCaptions[mode], order.Sum), normalLabel);
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
				if(GUILayout.Button(orderCaptions[mode])) orderPlaced = true; 
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
			GUILayout.Label(string.Format(confirmOrderCaption, placeOrderCaptions[mode], Mathf.RoundToInt(quantitySlider), order.Item.Name), normalLabel);
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) orderPlaced = false;
				if(GUILayout.Button(orderCaptions[mode]))
				{
					orderPlaced = false;

					if (mode == (int)OrderMode.Buy) returnToPrevState = sufficientFunds = listener.buyOrderPlaced(order);
					else listener.sellOrderPlaced(order);
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}