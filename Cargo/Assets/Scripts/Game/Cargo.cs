using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Cargo
{
	public Dictionary<Item, int> compartments = new Dictionary<Item, int>();
	private int maxVolume, currentvolume;

	public Cargo(int maxVolume)
	{
		this.maxVolume = maxVolume;
	}

	public bool AddItem(Item item, int quantity)
	{
		if (currentvolume + item.Volume * quantity > maxVolume) return false;

		if (compartments.ContainsKey(item))
			compartments[item] += quantity;
		else compartments.Add(item, quantity);

		currentvolume += item.Volume * quantity;
		return true;
	}

	public void RemoveItem(Item item, int quantity)
	{
		if (compartments[item] < quantity)
			compartments[item] -= quantity;
		else compartments.Remove(item);

		currentvolume -= item.Volume * quantity;
	}

	public int GetWeight()
	{
		int weight = 0;
		foreach (Item item in compartments.Keys)
			weight += item.Weight;

		return weight;
	}

	public int GetRemainingSpace()
	{
		return maxVolume - currentvolume;
	}

	public List<Item> GetItems()
	{
		return compartments.Keys.ToList();
	}

	public List<int> GetQuantities()
	{
		return compartments.Values.ToList();
	}

	public int GetQuantity(Item item)
	{
		return compartments[item];
	}
}