using UnityEngine;
using System.Collections.Generic;

public class OrderState : State
{
	private int width, height;
	private float quantity = 0.0f;
	private string labelStyle = "leftAlignedLabel";
	public string orderCaption;
	private string nameCaption = "Name:";
	private string volumeCaption = "Volume:";
	private string weightCaption = "Weight:";
	private string priceCaption = "Price:";
	private string quantityCaption = "Quantity: ";
	private string totalPriceCaption = "Total price: ";
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
				GUILayout.Label(nameCaption, labelStyle, GUILayout.ExpandWidth(true));
				GUILayout.Label(volumeCaption, GUILayout.Width(65));
				GUILayout.Label(weightCaption, GUILayout.Width(65));
				GUILayout.Label(priceCaption, GUILayout.Width(55));
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(order.item.Name, labelStyle, GUILayout.ExpandWidth(true));
				GUILayout.Label(order.item.Volume.ToString(), GUILayout.Width(65));
				GUILayout.Label(order.item.Weight.ToString(), GUILayout.Width(65));
				GUILayout.Label("$" + order.value.ToString(), GUILayout.Width(55));
			}
			GUILayout.EndHorizontal();

			quantity = GUILayout.HorizontalSlider(quantity, 0, order.quantity);
			int total = order.value * (int)quantity;
			GUILayout.Label(quantityCaption + 'x' + (int)quantity, labelStyle, GUILayout.ExpandWidth(true));
			GUILayout.Label("<size=50>" + totalPriceCaption + "$" + total + "</size>", "normalLabel");

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) returnToPrevState = true;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}