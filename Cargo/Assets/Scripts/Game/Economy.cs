using UnityEngine;
using System.Collections.Generic;

public class Economy
{
	private Dictionary<Item, int> itemStacks = new Dictionary<Item, int>();
	private Dictionary<Item, int> itemPrices = new Dictionary<Item, int>();

	private const float minValueMultiplier = 0.2f;
	private const float maxValueMultiplier = 5.0f;

	public Economy()
	{
		var items = ItemDatabase.GetAllItems();

		foreach(Item item in items)
		{
			// Randomize item price
			float multiplier = Random.Range(minValueMultiplier, maxValueMultiplier);
			int value = Mathf.RoundToInt(multiplier * item.BaseValue);
			itemPrices.Add(item, value);

			// Randomly choose an item to be available in this economy
			if(Random.value > 0.5f)
			{
				itemStacks.Add(item, Random.Range(1, 201));
			}
		}
	}

	public List<Item> GetItems()
	{
		var items = new List<Item>();

		foreach(var pair in itemStacks)
		{
			items.Add(pair.Key);
		}

		return items;
	}

	public int GetQuantity(Item item)
	{
		return itemStacks.ContainsKey(item) ? itemStacks[item] : 0;
	}

	public List<int> GetQuantities()
	{
		var quantities = new List<int>();

		foreach(var pair in itemStacks)
		{
			quantities.Add(pair.Value);
		}

		return quantities;
	}

	public int GetPrice(Item item)
	{
		return itemPrices[item];
	}

	public List<int> GetPrices()
	{
		var prices = new List<int>();

		foreach(var pair in itemPrices)
		{
			prices.Add(pair.Value);
		}

		return prices;
	}

	public void Consume(Item item, int amount)
	{
		if(itemStacks.ContainsKey(item))
		{
			if(itemStacks[item] >= amount) itemStacks[item] -= amount;
			if(itemStacks[item] == 0) itemStacks.Remove(item);
		}
	}

	public void Supply(Item item, int amount)
	{
		if(itemStacks.ContainsKey(item)) itemStacks[item] += amount;
		else itemStacks.Add(item, amount);
	}
}