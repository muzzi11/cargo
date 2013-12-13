using UnityEngine;
using System.Collections.Generic;

public class Table
{
	private List<List<string>> table;
	private List<Item> items;
	private List<int> quantities;
	private List<int> values;

	private Vector2 scrollPosition;

	public event OrderHandler orderPlaced;
	public delegate void OrderHandler(Order order);

	public Table()
	{
		table = new List<List<string>>();
	}
	
	public void LoadData(List<Item> items, List<int> quantities, List<int> values)
	{
		this.items = items;
		this.quantities = quantities;
		this.values = values;
		for(int i = 0; i < items.Count; ++i)
		{			
			table.Add(new List<string>()
			{
				items[i].Name,
				'x' + quantities[i].ToString(),
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
							item = items[i],
							quantity = quantities[i],
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