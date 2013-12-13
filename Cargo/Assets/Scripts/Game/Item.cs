using UnityEngine;

public struct IDQuantityPair
{
	public int id, quantity;

	public IDQuantityPair(int id, int quantity)
	{
		this.id = id;
		this.quantity = quantity;
	}
}

public class Item
{
	public int id;
	public string name;

	public int Volume
	{
		get
		{
			return ItemTable.GetVolume(id);
		}
	}

	public int Weight
	{
		get
		{
			return ItemTable.GetWeight(id);
		}
	}
}

public class ItemStack
{
	public Item item;
	public int quantity;
}