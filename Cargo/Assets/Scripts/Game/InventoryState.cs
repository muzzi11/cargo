using UnityEngine;
using System.Collections.Generic;

public class InventoryState : State
{
	private State returnToState;
	private int width, height;
	private bool returnToPrevState = false;
	private Vector2 scrollPosition;
	private List<string[]> table = new List<string[]> ();

	private Balance balance;
	private Cargo cargo;

	public InventoryState(State returnToState, Balance balance, Cargo cargo)
	{
		this.returnToState = returnToState;
		this.cargo = cargo;
		this.balance = balance;

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

		GUI.Window(0, new Rect(0, 0, width, height), InventoryWindow, StringTable.inventoryCaption);

		return this;
	}

	public void LoadData()
	{
		table.Clear ();

		List<Item> items = cargo.GetItems();
		List<int> quantities = cargo.GetQuantities();
		List<int> purchasePrices = cargo.GetPrices();
		List<string> origins = cargo.GetOrigins();

		for(int i = 0; i < items.Count; ++i)
		{
			table.Add(new string[]
        	{
				items[i].Name,
				origins[i],
				'x' + quantities[i].ToString(),
				'$' + purchasePrices[i].ToString()
			});
		}
	}

	private void InventoryWindow(int ID)
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);

			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(StringTable.nameHeaderCaption, StringTable.leftAlignedLabelStyle, GUILayout.ExpandWidth(true));
					GUILayout.Label(StringTable.originHeaderCaption, StringTable.leftAlignedLabelStyle, GUILayout.Width(80));
					GUILayout.Label(StringTable.quantityHeaderCaption, GUILayout.Width(50));
					GUILayout.Label(StringTable.priceHeaderCaption, GUILayout.Width(50));
				}
				GUILayout.EndHorizontal();

				for(int i = 0; i < table.Count; i++)
				{
					string[] row = table[i];
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(row[0], GUILayout.ExpandWidth(true));
						GUILayout.Label(row[1], GUILayout.Width(80));
						GUILayout.Label(row[2], GUILayout.Width(50));
						GUILayout.Label(row[3], GUILayout.Width(50));
					}
					GUILayout.EndHorizontal();
				}
			}
			GUILayout.EndScrollView();

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(string.Format(StringTable.balanceCaption, balance.GetBalance()), StringTable.normalLabelStyle);
				GUILayout.Label(string.Format(StringTable.cargoCaption, cargo.GetRemainingSpace()), StringTable.normalLabelStyle);
			}
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button(StringTable.backCaption)) returnToPrevState = true;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();			
	}
}