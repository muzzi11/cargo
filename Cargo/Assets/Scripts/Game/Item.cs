using UnityEngine;

public struct Item
{
	public readonly int id;

	public Item(int id)
	{
		this.id = id;
	}

	public string Name
	{
		get
		{
			return ItemDatabase.GetName(id);
		}
	}

	public int Volume
	{
		get
		{
			return ItemDatabase.GetVolume(id);
		}
	}

	public int Weight
	{
		get
		{
			return ItemDatabase.GetWeight(id);
		}
	}

	public int BasePrice
	{
		get
		{
			return ItemDatabase.GetBasePrice(id);
		}
	}
}