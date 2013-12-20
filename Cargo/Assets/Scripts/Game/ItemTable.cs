using UnityEngine;
using System.Collections.Generic;

public interface ItemTableListener
{
	void ItemClicked(Item item);
}

public class ItemTable
{
	private const string tableItemStyle = "tableItem", leftAlignedStyle = "leftAlignedLabel";
	private const string nameCaption = "Name:";
	private const string quantityCaption = "Qnty:";
	private const string priceCaption = "Price:";
	private const string purchasePriceCaption = "Purchase price:";
	private const string originCaption = "Origin:";

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
				GUILayout.Label(nameCaption, leftAlignedStyle, GUILayout.ExpandWidth(true));
				GUILayout.Label(quantityCaption, GUILayout.Width(50));
				GUILayout.Label(priceCaption, GUILayout.Width(50));
				if (table[0].Length == 5)
				{
					GUILayout.Label(purchasePriceCaption, GUILayout.Width(100));
					GUILayout.Label(originCaption, GUILayout.Width(80));
				}
			}
			GUILayout.EndHorizontal();

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
					if (row.Length == 5)
					{
						GUILayout.Label(row[3], GUILayout.Width(100));
						GUILayout.Label(row[4], GUILayout.Width(80));
					}
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();
	}
}