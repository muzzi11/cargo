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
		if (currentvolume + item.Volume > maxVolume) return false;

		compartments.Add(item, quantity);
		currentvolume += item.Volume;
		return true;
	}

	public void RemoveItem(Item item)
	{
		compartments.Remove(item);
	}

	public int GetWeight()
	{
		int weight = 0;
		foreach (Item item in compartments.Keys)
			weight += item.Weight;

		return weight;
	}

	public List<Item> GetItems()
	{
		return compartments.Keys.ToList();
	}

	public List<int> GetQuantities()
	{
		return compartments.Values.ToList();
	}
}