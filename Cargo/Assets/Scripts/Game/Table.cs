using UnityEngine;
using System.Collections.Generic;

public class Table
{
	private List<List<string>> table;
	private List<float> colWidth;
	private int width, height;
	private Vector2 scrollPosition;
	
	public Table(int width, int height)
	{
		this.width = width - 10;
		this.height = height;
		
		table = new List<List<string>>();
		
		colWidth = new List<float>()
		{
			0.55f, 0.15f, 0.15f
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
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(width), GUILayout.Height(height));
		
		foreach(List<string> row in table)
		{
			int counter = 0;
			GUILayout.BeginHorizontal();
			
			foreach(string cell in row)	
			{		
				GUILayout.Label(cell, GUILayout.Width(width * colWidth[counter]));
				++counter;
			}
			GUILayout.EndHorizontal();			
		}		
		GUILayout.EndScrollView();
	}
}