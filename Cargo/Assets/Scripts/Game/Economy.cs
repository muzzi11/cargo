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
			int value = Mathf.RoundToInt(multiplier * ItemTable.GetBaseValue());
			itemValues.Add(id, value);

			if(Random.value > 0.5f)
			{
				items.Add(id, Random.Range(1, 201));
			}
		}
	}

	public List<IDQuantityPair> GetItemQuantities()
	{
		var list = new List<IDQuantityPair>();

		foreach(var pair in items)
		{
			list.Add(new IDQuantityPair(pair.Key, pair.Value));
		}

		return list;
	}

	public void Consume(int id, int amount)
	{
		if(items[id] >= amount) items[id] -= amount;
		if(items[id] == 0) items.Remove(id);
	}

	public void Supply(int id, int amount)
	{
		if(items.ContainsKey(id)) items[id] += amount;
		else items.Add(id, amount);
	}
}