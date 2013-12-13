using UnityEngine;
using System.Collections.Generic;

public class Economy
{
	private Dictionary<int, int> items = new Dictionary<int, int>();
	private Dictionary<int, int> itemValues = new Dictionary<int, int>();

	private const float minValueMultiplier = 0.2f;
	private const float maxValueMultiplier = 5.0f;

	public Economy()
	{
		var itemIDs = ItemTable.GetItemIDs();

		foreach(int id in itemIDs)
		{
			float multiplier = Random.Range(minValueMultiplier, maxValueMultiplier);
			int value = Mathf.RoundToInt(multiplier * ItemTable.GetBaseValue(id));
			itemValues.Add(id, value);

			if(Random.value > 0.5f)
			{
				items.Add(id, Random.Range(1, 201));
			}
		}
	}

	public List<Item> GetItems()
	{
		var list = new List<Item>();

		foreach(var pair in items)
		{
			var item = new Item();

			item.id = pair.Key;
			item.name = ItemTable.GetName(pair.Key);

			list.Add(item);
		}

		return list;
	}

	public int GetQuantity(int id)
	{
		return items[id];
	}

	public int GetValue(int id)
	{
		return itemValues[id];
	}

	public void Consume(int id, int amount)
	{
		if(items.ContainsKey(id))
		{
			if(items[id] >= amount) items[id] -= amount;
			if(items[id] == 0) items.Remove(id);
		}
	}

	public void Supply(int id, int amount)
	{
		if(items.ContainsKey(id)) items[id] += amount;
		else items.Add(id, amount);
	}
}