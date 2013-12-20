using UnityEngine;
using System.Collections;

public class BattleObjects : MonoBehaviour
{
	public GameObject shipPrefab, damageTextPrefab;
	public Sprite[] shipSprites;

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	public GameObject InstantiateShip(int shipIndex, Vector2 position)
	{
		if(shipIndex < 0 || shipIndex > shipSprites.Length) Debug.LogError("Ship index out of bounds");

		var ship = Instantiate(shipPrefab, position, Quaternion.identity) as GameObject;
		ship.GetComponent<SpriteRenderer>().sprite = shipSprites[shipIndex];

		return ship;
	}

	public void InstantiateDamageText(string text, Vector2 position)
	{
		var damageText = Instantiate(damageTextPrefab, position, Quaternion.identity) as GameObject;
		damageText.guiText.text = text;
	}
}
