using UnityEngine;
using System.Collections.Generic;

public interface ItemTableListener
{
	void ItemClicked(Item item);
}

public class ItemTable
{
	private const string tableItemStyle = "tableItem";

	private List<string[]> table = new List<string[]>();
	private List<Item> items = new List<Item>();

	private List<ItemTableListener> listeners = new List<ItemTableListener>();

	private Vector2 scrollPosition;

	public void AddListener(ItemTableListener listener)
	{
		listeners.Add(listener);
	}
	
	public void LoadData(List<Item> items, List<int> quantities, List<int> values)
	{
		table.Clear();
		this.items = items;

		for(int i = 0; i < items.Count; ++i)
		{			
			table.Add(new string[]
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
			for(int i = 0; i < table.Count; i++)
			{
				string[] row = table[i];
				GUILayout.BeginHorizontal();
				{
					if (GUILayout.Button(row[0], tableItemStyle, GUILayout.ExpandWidth(true)))
				    {
						foreach(var listener in listeners)
						{
							listener.ItemClicked(items[i]);
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