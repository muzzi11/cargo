using UnityEngine;
using System.Collections.Generic;

public class OrderState : State
{
	private int width, height;
	private float quantity = 1.0f;
	private string leftAlignedLabel = "leftAlignedLabel", normalLabel ="normalLabel";
	public string orderCaption, confirmOrderCaption;
	private string nameCaption = "Name:",
				   volumeCaption = "Volume:",
				   weightCaption = "Weight:",
				   priceCaption = "Price:",
				   quantityCaption = "Quantity: x",
		   		   totalPriceCaption = "Total price: $";
	private State returnToState;
	public Order order;

	private bool returnToPrevState;

	public OrderState(State returnToState)
	{
		this.returnToState = returnToState;
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

		if (order != null)
		{
			GUI.Window(0, new Rect(0, 0, width, height), BuyWindow, orderCaption + order.item.Name);
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
				GUILayout.Label(order.item.Name, leftAlignedLabel, GUILayout.ExpandWidth(true));
				GUILayout.Label(order.item.Volume.ToString(), GUILayout.Width(65));
				GUILayout.Label(order.item.Weight.ToString(), GUILayout.Width(65));
				GUILayout.Label("$" + order.value.ToString(), GUILayout.Width(55));
			}
			GUILayout.EndHorizontal();

			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical();
			{
				quantity = GUILayout.HorizontalSlider(quantity, 1, order.quantity);
				int total = order.value * (int)quantity;
				GUILayout.Label("<size=24>" + quantityCaption + (int)quantity + "</size>", normalLabel, GUILayout.ExpandWidth(true));
				GUILayout.Label("<size=24>" + totalPriceCaption + total + "</size>", normalLabel);
			}
			GUILayout.EndVertical();

			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) returnToPrevState = true;
				GUILayout.Button(confirmOrderCaption);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}