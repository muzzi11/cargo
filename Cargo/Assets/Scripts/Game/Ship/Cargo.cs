using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CargoRecord
{
	private static readonly char[] seperator = {';'};

	private Item item;
	private int quantity, purchasePrice;
	private string origin;

	public CargoRecord(Item item, int quantity, int purchasePrice, string origin)
	{
		this.item = item;
		this.quantity = quantity;
		this.purchasePrice = purchasePrice;
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

	public int PurchasePrice
	{
		get { return purchasePrice; }
	}

	public string Origin
	{
		get { return origin; }
	}

	public string Serialize()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append(item.id);
		builder.Append(seperator);
		builder.Append(quantity);
		builder.Append(seperator);
		builder.Append(purchasePrice);
		builder.Append(seperator);
		builder.Append(origin);

		return builder.ToString();
	}

	public static CargoRecord Deserialize(string str)
	{
		string[] prop = str.Split(seperator);
		if(prop.Length != 4)
		{
			Debug.LogError("Propery count doesn't match: " + prop.Length.ToString());
			return null;
		}

		bool failed = false;
		int id, quantity, price;

		failed |= !int.TryParse(prop[0], out id);
		failed |= !int.TryParse(prop[1], out quantity);
		failed |= !int.TryParse(prop[2], out price);

		if(failed)
		{
			Debug.LogError("Failed to parse properties.");
			return null;
		}

		return new CargoRecord(new Item(id), quantity, price, prop[3]);
	}
}

public class Cargo
{
	private static readonly char[] seperator = {'@'};

	public List<CargoRecord> records = new List<CargoRecord>();
	private int maxVolume, currentvolume;

	public Cargo(int maxVolume)
	{
		this.maxVolume = maxVolume;
		if(SaveGame.gameData != null) Deserialize(SaveGame.gameData.serializedCargo);
	}

	public bool AddItem(Item item, int quantity, int purchasePrice, string origin)
	{
		if (currentvolume + item.Volume * quantity > maxVolume) return false;
		currentvolume += item.Volume * quantity;

		foreach (CargoRecord record in records)
		{
			if (record.Item.Equals(item) && record.Origin.Equals(origin))
			{
				record.Quantity += quantity;
				SaveGame.SaveCargo(Serialize());
				return true;
			}
		}
		records.Add(new CargoRecord(item, quantity, purchasePrice, origin));
		records = records.OrderBy(rec => rec.Item.Name).ToList();

		SaveGame.SaveCargo(Serialize());
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
				if (record.Quantity > quantity)				
					record.Quantity -= quantity;
				else
					temp = record;
				break;
			}
		}
		if (temp != null)
			records.Remove(temp);

		SaveGame.SaveCargo(Serialize());
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

	public int GetQuantity(Item item, string origin)
	{
		foreach (CargoRecord record in records)
			if (record.Item.Equals(item) && record.Origin.Equals(origin)) return record.Quantity;
		return 0;
	}

	public List<int> GetPrices()
	{
		List<int> prices = new List<int> ();
		foreach (CargoRecord record in records)
			prices.Add(record.PurchasePrice);
		return prices;
	}

	public List<string> GetOrigins()
	{
		List<string> origins = new List<string> ();
		foreach (CargoRecord record in records)
			origins.Add(record.Origin);
		return origins;
	}

	public string Serialize()
	{
		StringBuilder builder = new StringBuilder();

		foreach(CargoRecord record in records)
		{
			builder.Append(record.Serialize());
			builder.Append(seperator);
		}
		if(records.Count > 0) builder.Length -= 1;

		return builder.ToString();
	}

	public void Deserialize(string str)
	{
		if(string.IsNullOrEmpty(str)) return;

		string[] serializedData = str.Split(seperator);
		foreach(string data in serializedData)
		{
			var record = CargoRecord.Deserialize(data);
			if(record != null) AddItem(record.Item, record.Quantity, record.PurchasePrice, record.Origin);
		}
	}
}