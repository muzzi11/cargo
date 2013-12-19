using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
			int value = Mathf.RoundToInt(multiplier * item.BasePrice);
			itemPrices.Add(item, value);

			// Randomly choose an item to be available in this economy
			if(Random.value > 0.5f)
			{
				itemStacks.Add(item, Random.Range(1, 201));
			}
		}
	}

	public List<Item> GetAllItems()
	{
		return itemStacks.Keys.ToList();
	}

	public int GetQuantity(Item item)
	{
		return itemStacks.ContainsKey(item) ? itemStacks[item] : 0;
	}

	public List<int> GetQuantities()
	{
		return itemStacks.Values.ToList();
	}

	public int GetPrice(Item item)
	{
		return itemPrices[item];
	}

	public List<int> GetAllPrices()
	{
		return itemPrices.Values.ToList();
	}

	public List<int> GetPrices(List<Item> items)
	{
		List<int> prices = new List<int>();		
		foreach(Item item in items) prices.Add(GetPrice(item));
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