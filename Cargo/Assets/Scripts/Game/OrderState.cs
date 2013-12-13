using UnityEngine;
using System.Collections.Generic;

public abstract class OrderState : State
{
	private float quantity = 1.0f;
	private int orderValue;

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
	protected int width, height;
	protected bool returnToPrevState, orderPlaced = false;
	protected Balance balance;

	public Order order;
	
	public OrderState(State returnToState, Balance balance)
	{
		this.balance = balance;
		this.returnToState = returnToState;
		width = Screen.width;
		height = Screen.height;
	}

	abstract public State UpdateState ();
	abstract protected int UpdateBalance(int orderValue);	
	abstract protected void ProcessTransaction(int orderValue);

	protected void TransactionWindow(int ID)
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);

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
				GUILayout.Label(order.stack.item.name, leftAlignedLabel, GUILayout.ExpandWidth(true));
				GUILayout.Label(order.stack.item.volume.ToString(), GUILayout.Width(65));
				GUILayout.Label(order.stack.item.weight.ToString(), GUILayout.Width(65));
				GUILayout.Label("$" + order.value.ToString(), GUILayout.Width(55));
			}
			GUILayout.EndHorizontal();

			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical();
			{
				quantity = GUILayout.HorizontalSlider(quantity, 1, order.stack.quantity);
				orderValue = order.value * (int)quantity;
				int remainder = UpdateBalance(orderValue);

				GUILayout.Label(string.Format(quantityCaption, (int)quantity), normalLabel, GUILayout.ExpandWidth(true));
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
			GUILayout.Label(string.Format(confirmOrderCaption, (int)quantity, order.stack.item.name), normalLabel);
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) orderPlaced = false;
				if(GUILayout.Button(placeOrder)) ProcessTransaction(orderValue);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}