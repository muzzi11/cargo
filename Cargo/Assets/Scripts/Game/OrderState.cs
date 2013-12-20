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
	private string origin;

	public Order(Item item, int itemPrice, int quantity, string origin)
	{
		this.itemPrice = itemPrice;
		this.item = item;
		this.quantity = quantity;
		this.origin = origin;
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

	public string Origin
	{ 
		get { return origin; }
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

		GUI.Window(0, new Rect(0, 0, width, height), TransactionWindow, string.Format(StringTable.orderWindowTitles[mode], lot.Item.Name));

		if(currentDialog == Dialog.Confirmation)
			windowFunction = ConfirmationWindow;
		else if(currentDialog == Dialog.TransactionSummary)
			windowFunction = SummationWindow;
		else if(currentDialog == Dialog.InsufficientFunds) 
			windowFunction = InsufficientFundsWindow;
		else if(currentDialog == Dialog.InsufficientSpace)
			windowFunction = InsufficientSpaceWindow;

		if(currentDialog != Dialog.None)
			GUI.ModalWindow(1, new Rect(0, height/4, width, height/2), windowFunction, StringTable.orderDialogTitles[(int)currentDialog]);

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
			GUILayout.Label(string.Format(StringTable.dollazShortCaption, tooShort), StringTable.normalLabelStyle);
			
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
			GUILayout.Label(string.Format(StringTable.cargoSpaceShortCaption, tooShort), StringTable.normalLabelStyle);

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
					GUILayout.Label(StringTable.nameHeaderCaption, StringTable.leftAlignedLabelStyle, GUILayout.ExpandWidth(true));
					GUILayout.Label(StringTable.volumeHeaderCaption, GUILayout.Width(65));
					GUILayout.Label(StringTable.weightHeaderCaption, GUILayout.Width(65));
					GUILayout.Label(StringTable.priceHeaderCaption, GUILayout.Width(55));
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(lot.Item.Name, StringTable.leftAlignedLabelStyle, GUILayout.ExpandWidth(true));
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
				GUILayout.Label(string.Format(StringTable.quantityCaption, (int)quantitySlider), StringTable.normalLabelStyle, GUILayout.ExpandWidth(true));

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
						GUILayout.Label(string.Format(StringTable.priceCaptions[mode], price), StringTable.normalLabelStyle);
						GUILayout.Label(string.Format(StringTable.balanceCaption, updatedBalance), StringTable.normalLabelStyle);
					}
					GUILayout.EndVertical();
					GUILayout.BeginVertical();
					{
						GUILayout.Label(string.Format(StringTable.totalVolumeCaption, volume), StringTable.normalLabelStyle);
						GUILayout.Label(string.Format(StringTable.remainingSpaceCaption, updatedSpace), StringTable.normalLabelStyle);
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
				if(GUILayout.Button(StringTable.orderCaptions[mode])) currentDialog = Dialog.Confirmation; 
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
			GUILayout.Label(string.Format(StringTable.confirmOrderCaption, StringTable.orderCaptions[mode].ToLower(), 
			                              Mathf.RoundToInt(quantitySlider), lot.Item.Name), StringTable.normalLabelStyle);
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) currentDialog = Dialog.None;
				if(GUILayout.Button(StringTable.orderCaptions[mode]))
				{
					order = new Order(lot.Item, lot.ItemPrice, Mathf.RoundToInt(quantitySlider), lot.Origin);

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
			GUILayout.Label(string.Format(StringTable.summationCaption, Mathf.RoundToInt(quantitySlider), lot.Item.Name,
			                              StringTable.transactionCaptions[mode]), StringTable.normalLabelStyle);
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