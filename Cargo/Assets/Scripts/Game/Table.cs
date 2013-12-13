﻿using UnityEngine;
using System.Collections.Generic;

public class Table
{
	private List<List<string>> table;
	private List<ItemStack> stacks;
	private List<int> values;

	private Vector2 scrollPosition;

	public event OrderHandler orderPlaced;
	public delegate void OrderHandler(Order order);

	public Table()
	{
		table = new List<List<string>>();
	}
	
	public void LoadData(List<ItemStack> stacks, List<int> values)
	{
		this.stacks = stacks;
		this.values = values;

		for(int i = 0; i < stacks.Count; ++i)
		{			
			table.Add(new List<string>()
			{
				stacks[i].item.name,
				'x' + stacks[i].quantity.ToString(),
				'$' + values[i].ToString()
			});
		}
	}
	
	public bool Render()
	{		
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		{
			for(int i = 0; i < table.Count; i++)
			{
				List<string> row = table[i];
				GUILayout.BeginHorizontal();
				{
					if (GUILayout.Button(row[0], "tableItem", GUILayout.ExpandWidth(true)))
				    {
						Order order = new Order()
						{
							stack = stacks[i],
							value = values[i]
						};
						orderPlaced(order);
						return true;
					}
					GUILayout.Label(row[1], GUILayout.Width(50));
					GUILayout.Label(row[2], GUILayout.Width(50));
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();

		return false;
	}
}