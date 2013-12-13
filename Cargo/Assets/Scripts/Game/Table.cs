using UnityEngine;
using System.Collections.Generic;

public interface TableListener
{
	void ItemClicked(int id);
}

public class Table
{
	private const string tableItemStyle = "tableItem";

	private List<string[]> table = new List<string[]>();
	private List<int> itemIDs = new List<int>();

	private List<TableListener> listeners = new List<TableListener>();

	private Vector2 scrollPosition;


	public void AddListener(TableListener listener)
	{
		listeners.Add(listener);
	}
	
	public void LoadData(List<Item> items, List<int> quantities, List<int> values)
	{
		table.Clear();
		itemIDs.Clear();

		for(int i = 0; i < items.Count; ++i)
		{			
			table.Add(new string[]
			{
				items[i].name,
				'x' + quantities[i].ToString(),
				'$' + values[i].ToString()
			});

			itemIDs.Add(items[i].id);
		}
	}
	
	public void Render()
	{		
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		{
			for(int i = 0; i < table.Count; i++)
			{
				string[] row = table[i];
				GUILayout.BeginHorizontal();
				{
					if (GUILayout.Button(row[0], tableItemStyle, GUILayout.ExpandWidth(true)))
				    {
						foreach(var listener in listeners)
						{
							listener.ItemClicked(itemIDs[i]);
						}
					}
					GUILayout.Label(row[1], GUILayout.Width(50));
					GUILayout.Label(row[2], GUILayout.Width(50));
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();
	}
}