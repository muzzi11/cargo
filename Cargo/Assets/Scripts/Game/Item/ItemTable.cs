using UnityEngine;
using System.Collections.Generic;

public interface ItemTableListener
{
	void ItemClicked(Item item);
}

public class ItemTable
{
	private List<string[]> table = new List<string[]>();
	private List<Item> items = new List<Item>();
	private List<ItemTableListener> listeners = new List<ItemTableListener>();

	private Vector2 scrollPosition;

	public void AddListener(ItemTableListener listener)
	{
		listeners.Add(listener);
	}
	
	public void LoadData(List<Item> items, List<int> quantities, List<int> prices, List<int> purchasePrices = null, List<string> origins = null)
	{
		table.Clear();
		this.items = items;

		for(int i = 0; i < items.Count; ++i)
		{	
			if(origins != null && purchasePrices != null)
			{
				table.Add(new string[]
				          {
					items[i].Name,
					'x' + quantities[i].ToString(),
					'$' + prices[i].ToString(),
					'$' + purchasePrices[i].ToString(),
					origins[i]
				});
				continue;	
			}
			table.Add(new string[]
			{
				items[i].Name,
				'x' + quantities[i].ToString(),
				'$' + prices[i].ToString()
			});
		}
	}

	public void Render()
	{	
		if (table.Count == 0) return;

		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(StringTable.nameHeaderCaption, StringTable.leftAlignedLabelStyle, GUILayout.ExpandWidth(true));
				GUILayout.Label(StringTable.quantityHeaderCaption, GUILayout.Width(50));
				GUILayout.Label(StringTable.priceHeaderCaption, GUILayout.Width(50));
				if (table[0].Length == 5)
				{
					GUILayout.Label(StringTable.purchasePriceHeaderCaption, GUILayout.Width(130));
					GUILayout.Label(StringTable.originHeaderCaption, GUILayout.Width(80));
				}
			}
			GUILayout.EndHorizontal();

			for(int i = 0; i < table.Count; i++)
			{
				string[] row = table[i];
				GUILayout.BeginHorizontal();
				{
					if (GUILayout.Button(row[0], StringTable.tableItemStyle, GUILayout.ExpandWidth(true)))
				    {
						foreach(var listener in listeners)
						{
							listener.ItemClicked(items[i]);
						}
					}
					GUILayout.Label(row[1], GUILayout.Width(50));
					GUILayout.Label(row[2], GUILayout.Width(50));
					if (row.Length == 5)
					{
						GUILayout.Label(row[3], GUILayout.Width(130));
						GUILayout.Label(row[4], GUILayout.Width(80));
					}
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();
	}
}