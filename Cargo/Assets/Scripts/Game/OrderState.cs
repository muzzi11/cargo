using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
		InsufficientFunds,
		InsufficientSpace,
		Confirmation,
		TransactionSummary,
		None
	}
	private Dialog currentDialog = Dialog.None;

	private readonly string[] 
		titleCaptions = { "Buying {0}", "Selling {0}" },
		orderCaptions = { "Buy", "Sell" },
	    placeOrderCaptions = { "buy", "sell"},
	    priceCaptions = { "Total cost: ${0}", "Profits: ${0}" },
	    transactionCaptions = { "added to", "removed from" },
		windowTitleCaptions = { "Insufficient funds!", "Insufficient cargo space!", "Please confirm your transaction.", "Congratulations!"};
	   
	private const string 
		nameCaption = "Name:",
		volumeCaption = "Volume:",
		weightCaption = "Weight:",
		priceCaption = "Price:",
		quantityCaption = "Quantity: x{0}",
		balanceCaption = "Balance: ${0}",
		totalVolumeCaption = "Total volume: {0}",
		remainingSpaceCaption = "Cargo space: {0}",
		confirmOrderCaption = "Are you sure you want to {0} {1} {2}?",
		dollazShortCaption = "You are {0} dollaz short.",
		cargoSpaceShortCaption = "You need additional {0} cubic meters of free cargo space.",
		summationCaption = "Transaction completed: {0} {1} have been {2} your cargo hold.";

	private const string leftAlignedLabel = "leftAlignedLabel", normalLabel = "normalLabel";

	private float quantitySlider = 1.0f;
	private Vector2 scrollPosition;
	private State returnToState;
	private int width, height, currentBalance, remainingSpace;
	private bool returnToPrevState;
	private AuctionLot lot;
	private OrderListener listener;
	private Order order;
	private GUI.WindowFunction windowFunction;

	public OrderState(State returnToState, OrderListener listener, AuctionLot lot, OrderMode mode, int currentBalance, int remainingSpace)
	{
		this.returnToState = returnToState;
		this.lot = lot;
		this.currentBalance = currentBalance;
		this.remainingSpace = remainingSpace;
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

		GUI.Window(0, new Rect(0, 0, width, height), TransactionWindow, string.Format(titleCaptions[mode], lot.Item.Name));

		if(currentDialog == Dialog.Confirmation)
			windowFunction = ConfirmationWindow;
		else if(currentDialog == Dialog.TransactionSummary)
			windowFunction = SummationWindow;
		else if(currentDialog == Dialog.InsufficientFunds) 
			windowFunction = InsufficientFundsWindow;
		else if(currentDialog == Dialog.InsufficientSpace)
			windowFunction = InsufficientSpaceWindow;

		if(currentDialog != Dialog.None)
			GUI.ModalWindow(1, new Rect(0, height/4, width, height/2), windowFunction, windowTitleCaptions[(int)currentDialog]);

		return this;
	}

	private int UpdateBalance(int price)
	{
		if (mode == (int)OrderMode.Buy) return currentBalance - price;
		return currentBalance + price;
	}

	private int UpdateCargo(int volume)
	{
		if (mode == (int)OrderMode.Buy) return remainingSpace - volume;
		return remainingSpace + volume;
	}

	private void InsufficientFundsWindow(int ID)
	{
		int tooShort = order.Price - currentBalance;
		
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);
			GUILayout.Label(string.Format(dollazShortCaption, tooShort), normalLabel);
			
			GUILayout.FlexibleSpace();
			
			if(GUILayout.Button("Back")) currentDialog = Dialog.None;
		}
		GUILayout.EndVertical();
	}

	private void InsufficientSpaceWindow(int ID)
	{
		int tooShort = (order.Item.Volume * order.Quantity) - remainingSpace;
		
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);
			GUILayout.Label(string.Format(cargoSpaceShortCaption, tooShort), normalLabel);

			GUILayout.FlexibleSpace();
			
			if(GUILayout.Button("Back")) currentDialog = Dialog.None;
		}
		GUILayout.EndVertical();
	}
	
	private void TransactionWindow(int ID)
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
				GUILayout.Label(string.Format(quantityCaption, (int)quantitySlider), normalLabel, GUILayout.ExpandWidth(true));

				quantitySlider = GUILayout.HorizontalSlider(quantitySlider, 1, lot.Availability);

				int quantity = Mathf.RoundToInt (quantitySlider);
				int price = quantity * lot.ItemPrice,
				    volume = quantity * lot.Item.Volume,
				    updatedBalance = UpdateBalance(price),
					updatedSpace = UpdateCargo(volume);

				GUILayout.BeginHorizontal();
				{
					GUILayout.BeginVertical();
					{
						GUILayout.Label(string.Format(priceCaptions[mode], price), normalLabel);
						GUILayout.Label(string.Format(balanceCaption, updatedBalance), normalLabel);
					}
					GUILayout.EndVertical();
					GUILayout.BeginVertical();
					{
						GUILayout.Label(string.Format(totalVolumeCaption, volume), normalLabel);
						GUILayout.Label(string.Format(remainingSpaceCaption, updatedSpace), normalLabel);
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
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

	private void ConfirmationWindow(int ID)
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
					order = new Order(lot.Item, lot.ItemPrice, Mathf.RoundToInt(quantitySlider));

					if (mode == (int)OrderMode.Buy) currentDialog = listener.BuyOrderPlaced(order);
					else currentDialog = listener.SellOrderPlaced(order);
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	private void SummationWindow(int ID)
	{
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);	
			GUILayout.Label(string.Format(summationCaption, Mathf.RoundToInt(quantitySlider), lot.Item.Name, transactionCaptions[mode]), normalLabel);
			GUILayout.FlexibleSpace();
			
			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Ok")) returnToPrevState = true;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}