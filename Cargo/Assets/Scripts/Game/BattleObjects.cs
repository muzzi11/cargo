using UnityEngine;
using System.Collections;

public class BattleObjects : MonoBehaviour
{
	public enum ShipType : int
	{
		Fighter,
		Raider
	}

	public ShipPrefab fighterPrefab, raiderPrefab;
	public GameObject damageTextPrefab;
	public Laserbeam laserbeamPrefab;
	public Sprite[] shipSprites;
	public ShipType[] shipTypes;

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	public ShipPrefab InstantiateShip(int shipIndex, Vector2 position)
	{
		if(shipIndex < 0 || shipIndex > shipSprites.Length) Debug.LogError("Ship index out of bounds");

		ShipPrefab shipPrefab;

		switch(shipTypes[shipIndex])
		{
		case ShipType.Fighter:
			shipPrefab = fighterPrefab;
			break;

		case ShipType.Raider:
			shipPrefab = raiderPrefab;
			break;

		default:
			shipPrefab = fighterPrefab;
			break;
		}

		var ship = Instantiate(shipPrefab, position, Quaternion.identity) as ShipPrefab;
		ship.GetComponent<SpriteRenderer>().sprite = shipSprites[shipIndex];

		return ship;
	}

	public void InstantiateDamageText(string text, Vector2 position)
	{
		var damageText = Instantiate(damageTextPrefab, position, Quaternion.identity) as GameObject;
		damageText.guiText.text = text;
	}

	public void InstantiateLaserbeam(Vector2 start, Vector2 end)
	{
		var laserbeam = Instantiate(laserbeamPrefab) as Laserbeam;
		laserbeam.SetRay(start, end);
	}
}
