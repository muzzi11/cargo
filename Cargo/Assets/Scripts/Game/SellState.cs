using UnityEngine;
using System.Collections.Generic;

public class SellState : State
{
	private int width, height;
	private State returnToState;
	private bool returnToPrevState;

	public Item item;

	public SellState(State returnToState)
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
		
		if (item != null)
		{
			GUI.Window(0, new Rect(0, 0, width, height), BuyWindow, "Selling: " + item.Name);
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
				if(GUILayout.Button("Back")) returnToPrevState = true;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}