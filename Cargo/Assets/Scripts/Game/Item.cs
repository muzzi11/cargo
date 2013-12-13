using UnityEngine;

public class Item
{
	public int ID;
	public string name;
	public float volume;
	public float weight;	
	public int quantity;
	public int baseValue;
}

public class ItemStack
{
	public Item item;
	public int quantity;
}