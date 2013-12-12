using UnityEngine;
using System.Collections.Generic;

public class Table
{
	private List<List<string>> table;
	private List<float> colWidth;
	private Rect rect;
	
	public Table(Rect rect)
	{
		this.rect = rect;
		table = new List<List<string>>();
		
		colWidth = new List<float>()
		{
			0.6f, 0.18f, 0.18f
		};
	}
	
	public void LoadData(List<Item> items, List<int> quantities, List<int> values)
	{
		int counter = 0;
		foreach (Item item in items)
		{			
			table.Add(new List<string>()
			{
				item.Name,
				quantities[counter].ToString() + 'x',
				'$' + values[counter].ToString()
			});
			++counter;
		}
	}
	
	public void Render()
	{		
		GUILayout.BeginArea(rect);
		
		foreach(List<string> row in table)
		{
			int counter = 0;
			GUILayout.BeginHorizontal();
			foreach(string cell in row)	
			{		
				GUILayout.TextField(cell, GUILayout.Width(rect.width * colWidth[counter]));
				++counter;
			}
			GUILayout.EndHorizontal();			
		}		
		GUILayout.EndArea();
	}
}