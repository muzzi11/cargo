using UnityEngine;
using System.Collections.Generic;

public class InventoryState : State
{
	private State returnToState;
	private int width, height;
	private bool returnToPrevState = false;

	private const string backCaption = "Back";
	private const string inventoryCaption = "Inventory";
	private const string balanceCaption = "Balance: {0}";
	private const string cargoCaption = "Cargo space: {0}";
	private const string normalStyle = "normalLabel";

	private Balance balance;
	private Cargo cargo;
	private ItemTable table = new ItemTable();

	public InventoryState(State returnToState, Cargo cargo, Balance balance)
	{
		this.returnToState = returnToState;
		this.cargo = cargo;
		this.balance = balance;

		width = Screen.width;
		height = Screen.height;

		table.LoadData(cargo.GetItems(), cargo.GetQuantities(), cargo.GetPrices(), cargo.GetOrigins());
	}

	public State UpdateState()
	{
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}

		GUI.Window(0, new Rect(0, 0, width, height), InventoryWindow, inventoryCaption);

		return this;
	}

	public void InventoryWindow(int ID)
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);
									
			table.Render();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(string.Format(balanceCaption, balance.GetBalance()), normalStyle);
				GUILayout.Label(string.Format(cargoCaption, cargo.GetRemainingSpace()), normalStyle);
			}
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button(backCaption)) returnToPrevState = true;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();			
	}
}