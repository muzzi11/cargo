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
	public readonly int weight, volume, value;

	public ItemMaterial(string name, int weight, int volume, int value)
	{
		this.name = name;
		this.weight = weight;
		this.volume = volume;
		this.value = value;
	}
}

public static class ItemFactory
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

	public static int GetBaseValue(int id)
	{
		int q = GetQualityIndex(id), m = GetMaterialIndex(id);

		return Mathf.RoundToInt(qualities[q].valueMultiplier * materials[m].value);
	}

	public static Item GenerateRandomItem()
	{
		var item = new Item();
		int q = Random.Range(0, qualities.Length);
		int m = Random.Range(0, materials.Length);

		item.Name = qualities[q].name + " " + materials[m].name;
		item.Volume = materials[m].volume;
		item.Weight = materials[m].weight;
		item.ID = GetID(q, m);

		return item;
	}
}
