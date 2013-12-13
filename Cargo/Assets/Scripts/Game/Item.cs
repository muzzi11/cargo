using UnityEngine;

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