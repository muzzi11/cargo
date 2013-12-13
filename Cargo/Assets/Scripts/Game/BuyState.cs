using UnityEngine;
using System.Collections.Generic;

public class OrderState : State
{
	private float quantity = 1.0f;
	
	private string nameCaption = "Name:",
				   volumeCaption = "Volume:",
				   weightCaption = "Weight:",
				   priceCaption = "Price:",
				   quantityCaption = "<size=24>Quantity: x{0} </size>",
				   balanceCaption = "<size=24>Balance: ${0} </size>";

	public string orderCaption, confirmOrderCaption, sumCaption;
	public string leftAlignedLabel = "leftAlignedLabel", normalLabel ="normalLabel";

	public State returnToState;
	public int width, height, orderValue;
	public bool returnToPrevState;
	public Order order;
	public Balance balance;

	public OrderState(State returnToState, Balance balance)
	{
		this.balance = balance;
		this.returnToState = returnToState;
		width = Screen.width;
		height = Screen.height;
	}

	public virtual State UpdateState()
	{	
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}

		if (order != null)
		{
			GUI.Window(0, new Rect(0, 0, width, height), BuyWindow, orderCaption + order.stack.item.name);
		}
		return this;
	}

	public void BuyWindow(int ID)
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
				if(GUILayout.Button(confirmOrderCaption)) ProcessTransaction(orderValue);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	public virtual int UpdateBalance(int orderValue)
	{
		return orderValue;
	}

	public virtual void ProcessTransaction(int orderValue)
	{
	}
}