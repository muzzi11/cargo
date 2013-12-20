using UnityEngine;
using System.Collections.Generic;

struct ItemMaterialQuality
{
	public readonly string name;
	public readonly float valueMultiplier;

	public ItemMaterialQuality(string name, float valueMultiplier)
	{
		this.name = name;
		this.valueMultiplier = valueMultiplier;
	}
}

struct ItemMaterial
{
	public readonly string name;
	public readonly int weight, volume, price;

	public ItemMaterial(string name, int weight, int volume, int price)
	{
		this.name = name;
		this.weight = weight;
		this.volume = volume;
		this.price = price;
	}
}

public static class ItemDatabase
{
	private static readonly ItemMaterialQuality[] qualities = new ItemMaterialQuality[]
	{
		new ItemMaterialQuality("Refined", 1.0f),
		new ItemMaterialQuality("Pure", 1.5f)
	};

	private static readonly ItemMaterial[] materials = new ItemMaterial[]
	{
		new ItemMaterial("Iron Ore", 1, 1, 4),
		new ItemMaterial("Bauxite Ore", 1, 1, 6),
		new ItemMaterial("Chromite Ore", 1, 1, 13),
		new ItemMaterial("Argentite Ore", 1, 1, 15)
	};

	private static int GetID(int qualityIndex, int materialIndex)
	{
		return (materialIndex << 8) | qualityIndex;
	}

	private static int GetQualityIndex(int id)
	{
		return id & 0xFF;
	}

	private static int GetMaterialIndex(int id)
	{
		return id >> 8;
	}

	public static string GetName(int id)
	{
		int q = GetQualityIndex(id), m = GetMaterialIndex(id);

		return qualities[q].name + " " + materials[m].name;
	}

	public static int GetBasePrice(int id)
	{
		int q = GetQualityIndex(id), m = GetMaterialIndex(id);

		return Mathf.RoundToInt(qualities[q].valueMultiplier * materials[m].price);
	}

	public static int GetWeight(int id)
	{
		return materials[GetMaterialIndex(id)].weight;
	}

	public static int GetVolume(int id)
	{
		return materials[GetMaterialIndex(id)].volume;
	}

	public static List<Item> GetAllItems()
	{
		var items = new List<Item>();

		for(int q = 0; q < qualities.Length; ++q)
		{
			for(int m = 0; m < materials.Length; ++m)
			{
				items.Add(new Item(GetID(q, m)));
			}
		}

		return items;
	}
}
