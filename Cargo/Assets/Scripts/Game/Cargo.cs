using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CargoRecord
{
	private Item item;
	private int quantity, costPrice;
	private string origin;

	public CargoRecord(Item item, int quantity, int costPrice, string origin)
	{
		this.item = item;
		this.quantity = quantity;
		this.costPrice = costPrice;
		this.origin = origin;
	}

	public Item Item
	{
		get { return item; }
	}

	public int Quantity
	{
		get { return quantity; }
		set { quantity = value; }
	}

	public int CostPrice
	{
		get { return costPrice; }
	}

	public string Origin
	{
		get { return origin; }
	}
}

public class Cargo
{
	public List<CargoRecord> records = new List<CargoRecord>();
	private int maxVolume, currentvolume;

	public Cargo(int maxVolume)
	{
		this.maxVolume = maxVolume;
	}

	public bool AddItem(Item item, int quantity, int costPrice, string origin)
	{
		if (currentvolume + item.Volume * quantity > maxVolume) return false;
		currentvolume += item.Volume * quantity;

		foreach (CargoRecord record in records)
		{
			if (record.Item.Equals(item) && record.Origin.Equals(origin))
			{
				record.Quantity += quantity;
				return true;
			}
		}
		records.Add(new CargoRecord(item, quantity, costPrice, origin));
		records = records.OrderBy(rec => rec.Item.Name).ToList();
		return true;
	}

	public void RemoveItem(Item item, int quantity, string origin)
	{
		currentvolume -= item.Volume * quantity;

		CargoRecord temp = null;
		foreach (CargoRecord record in records)
		{
			if (record.Item.Equals(item) && record.Origin.Equals(origin))
			{
				if (record.Quantity < quantity)				
					record.Quantity -= quantity;
				else
					temp = record;
				break;
			}
		}
		if (temp != null)
			records.Remove(temp);
	}

	public int GetWeight()
	{
		int weight = 0;
		foreach (CargoRecord record in records)
			weight += record.Item.Weight;

		return weight;
	}

	public int GetRemainingSpace()
	{
		return maxVolume - currentvolume;
	}

	public List<Item> GetItems()
	{
		List<Item> items = new List<Item>();
		foreach (CargoRecord record in records)
			items.Add(record.Item);
		return items;
	}

	public List<int> GetQuantities()
	{
		List<int> quantities = new List<int>();
		foreach (CargoRecord record in records)
			quantities.Add(record.Quantity);
		return quantities;
	}

	public int GetQuantity(Item item)
	{
		foreach (CargoRecord record in records)
			if (record.Item.Equals(item)) return record.Quantity;
		return 0;
	}

	public List<int> GetPrices()
	{
		List<int> prices = new List<int> ();
		foreach (CargoRecord record in records)
			prices.Add(record.CostPrice);
		return prices;
	}

	public List<string> GetOrigins()
	{
		List<string> origins = new List<string> ();
		foreach (CargoRecord record in records)
			origins.Add(record.Origin);
		return origins;
	}

}