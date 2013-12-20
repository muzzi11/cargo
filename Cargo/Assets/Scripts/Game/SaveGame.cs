using UnityEngine;
using System.Collections;

public class SaveGame
{
	public class Data
	{
		public int hull, shield, seed, balance;
		public Vector2 position;
		public string serializedCargo;
	}

	public static readonly string
		hull = "hull",
		shield = "shield",
		posX = "posX",
		posY = "posY",
		seed = "seed",
		balance = "balance",
		cargo = "cargo",
		mute = "mute",
		saved = "saved";

	public static Data gameData = null;

	public static void SaveShipStatus(int currentHull, int currentShield)
	{
		PlayerPrefs.SetInt(hull, currentHull);
		PlayerPrefs.SetInt(shield, currentShield);
	}

	public static void SavePosition(Vector2 position)
	{
		PlayerPrefs.SetFloat(posX, position.x);
		PlayerPrefs.SetFloat(posY, position.y);
	}

	public static void SaveSeed(int seedValue)
	{
		PlayerPrefs.SetInt(seed, seedValue);
		PlayerPrefs.SetInt(saved, 1);
	}

	public static void SaveBalance(int balanceValue)
	{
		PlayerPrefs.SetInt(balance, balanceValue);
	}

	public static void SaveCargo(string serialized)
	{
		PlayerPrefs.SetString(cargo, serialized);
	}

	public static void SaveSettings(AudioMute muteMode)
	{
		PlayerPrefs.SetInt(mute, (int)muteMode);
	}

	public static AudioMute GetSoundSetting()
	{
		return (AudioMute)PlayerPrefs.GetInt(mute, (int)AudioMute.Off);
	}

	// Use GetData to get the actual data if there is any
	public static void LoadData()
	{
		if(PlayerPrefs.GetInt(saved, 0) != 0)
		{
			gameData = new Data();
			gameData.seed = PlayerPrefs.GetInt(seed);
			gameData.hull = PlayerPrefs.GetInt(hull);
			gameData.shield = PlayerPrefs.GetInt(shield);
			gameData.balance = PlayerPrefs.GetInt(balance);
			gameData.position = new Vector2(PlayerPrefs.GetFloat(posX), PlayerPrefs.GetFloat(posY));
			gameData.serializedCargo = PlayerPrefs.GetString(cargo);
		}
	}

	public static void RemoveSaveGame()
	{
		gameData = null;
		PlayerPrefs.SetInt(saved, 0);
	}
}
