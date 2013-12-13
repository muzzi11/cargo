using UnityEngine;
using System.Collections.Generic;

public class Table
{
	private List<List<string>> table;
	private Vector2 scrollPosition;
	
	public Table(int width)
	{
		table = new List<List<string>>();
	}
	
	public void LoadData(List<Item> items, List<int> quantities, List<int> values)
	{
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
	
	public void Render()
	{		
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		{
			foreach(List<string> row in table)
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Button(row[0], "tableItem", GUILayout.ExpandWidth(true));
					GUILayout.Label(row[1], GUILayout.Width(50));
					GUILayout.Label(row[2], GUILayout.Width(50));
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();
	}
}