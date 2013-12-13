using UnityEngine;
using System.Collections.Generic;

public class Order
{
	public string name;
	public int quantity, value, volume, weight;

	public Order(string name, int quantity, int value, int volume, int weight)
	{
		this.name = name;
		this.quantity = quantity;
		this.value = value;
		this.volume = volume;
		this.weight = weight;
	}
}

public abstract class OrderState : State
{
	private float quantitySlider = 1.0f;
	private Vector2 scrollPosition;

	private string nameCaption = "Name:",
				   volumeCaption = "Volume:",
				   weightCaption = "Weight:",
				   priceCaption = "Price:",
				   quantityCaption = "<size=24>Quantity: x{0} </size>",
				   balanceCaption = "<size=24>Balance: ${0} </size>";
	
	protected string orderCaption, placeOrder, sumCaption, confirmOrderCaption;
	protected string leftAlignedLabel = "leftAlignedLabel", normalLabel ="normalLabel",
				     confirmationCaption = "Please confirm your transaction.";

	protected State returnToState;
	protected int width, height, orderValue, selectedQuantity;
	protected bool returnToPrevState, orderPlaced = false;
	protected Balance balance;
	protected Order order;
	
	public OrderState(State returnToState, Balance balance, Order order)
	{
		this.balance = balance;
		this.returnToState = returnToState;
		this.order = order;

		width = Screen.width;
		height = Screen.height;
	}

	public abstract State UpdateState();
	protected abstract int UpdateBalance(int orderValue);
	protected abstract void ProcessTransaction(int orderValue);

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
					GUILayout.Label(order.name, leftAlignedLabel, GUILayout.ExpandWidth(true));
					GUILayout.Label(order.volume.ToString(), GUILayout.Width(65));
					GUILayout.Label(order.weight.ToString(), GUILayout.Width(65));
					GUILayout.Label("$" + order.value.ToString(), GUILayout.Width(55));
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();

			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical();
			{
				quantitySlider = GUILayout.HorizontalSlider(quantitySlider, 1, order.quantity);
				selectedQuantity = Mathf.RoundToInt(quantitySlider);
				orderValue = order.value * selectedQuantity;
				int remainder = UpdateBalance(orderValue);

				GUILayout.Label(string.Format(quantityCaption, (int)quantitySlider), normalLabel, GUILayout.ExpandWidth(true));
				GUILayout.Label(string.Format(sumCaption, orderValue), normalLabel);
				GUILayout.Label(string.Format(balanceCaption, remainder), normalLabel);
			}
			GUILayout.EndVertical();

			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) returnToPrevState = true;
				if(GUILayout.Button(placeOrder)) orderPlaced = true; 
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
			GUILayout.Label(string.Format(confirmOrderCaption, selectedQuantity, order.name), normalLabel);
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) orderPlaced = false;
				if(GUILayout.Button(placeOrder))
				{
					orderPlaced = false;
					ProcessTransaction(orderValue);
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}