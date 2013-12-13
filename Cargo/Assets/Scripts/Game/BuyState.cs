using UnityEngine;
using System.Collections.Generic;

public class BuyState : State
{
	private int width, height;
	private float quantity = 0.0f;
	private State returnToState;
	public Order order;

	private bool returnToPrevState;

	public BuyState(State returnToState)
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
			GUI.Window(0, new Rect(0, 0, width, height), BuyWindow, "Buying: " + order.item.Name);
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
				GUILayout.Label("Name:", GUILayout.ExpandWidth(true));
				GUILayout.Label("Volume:", GUILayout.Width(100));
				GUILayout.Label("Weight:", GUILayout.Width(100));
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(order.item.Name, GUILayout.ExpandWidth(true));
				GUILayout.Label(order.item.Volume.ToString(), GUILayout.Width(100));
				GUILayout.Label(order.item.Weight.ToString(), GUILayout.Width(100));
			}
			GUILayout.EndHorizontal();

			quantity = GUILayout.HorizontalSlider(quantity, 0, order.quantity);

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Back")) returnToPrevState = true;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}