using UnityEngine;
using System.Collections.Generic;

public class Cargo
{
	public List<Item> items = new List<Item>();
	private int maxVolume = 200, currentvolume;

	public bool AddItem(Item item)
	{
		if (currentvolume + item.Volume > maxVolume) return false;

		items.Add (item);
		currentvolume += item.Volume;
		return true;
	}

	public int GetWeight()
	{
		int weight = 0;
		foreach (Item item in items)
			weight += item.Weight;

		return weight;
	}
}