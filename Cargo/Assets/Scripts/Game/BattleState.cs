using UnityEngine;
using System.Collections.Generic;

public interface BattleListener
{
	void ShipDestroyed();
}

public class BattleState : State
{
	private enum Action
	{
		Attack,
		Flee
	}

	private enum Windows : int
	{
		None = -1,
		Intro,
		Flee
	}

	// Helper class to keep track of turns
	class Turns
	{
		const float turnDelay = 2.0f;
		private float turnStartTime = Time.time;
		private bool playerTurn = true, ready = true, end = false;

		public void Update()
		{
			if(!end && Time.time > turnStartTime) ready = true;
		}

		public void SwitchTurns()
		{
			turnStartTime = Time.time + turnDelay;
			ready = false;
			playerTurn = !playerTurn;
		}

		// Ends all turns, no more turns after this point
		public void EndTurns()
		{
			turnStartTime = Time.time + turnDelay;
			end = true;
			ready = false;
		}

		// True when End() is called and turnDelay has passed
		public bool Finished
		{
			get{ return end && Time.time > turnStartTime; }
		}

		public bool PlayerTurn
		{
			get{ return playerTurn && ready; }
		}

		public bool EnemyTurn
		{
			get{ return !playerTurn && ready; }
		}
	}

	private readonly int collisionMask;
	private readonly GUI.WindowFunction[] windowFunctions;
	private Windows currentWindow = Windows.Intro;

	private readonly int width, height;
	private readonly State returnToState;

	private const float fleeChance = 0.2f;

	private Ship playerShip, enemyShip;

	private BattleObjects battleObjects;
	private GameObject playerNode;
	private ShipPrefab playerObject, enemyObject;
	
	private Turns turns = new Turns();

	private readonly BattleListener listener;

	public BattleState(BattleListener listener, State returnToState, Ship ship)
	{
		this.listener = listener;
		this.returnToState = returnToState;
		width = Screen.width;
		height = Screen.height;

		playerShip = ship;
		enemyShip = Ship.GenerateRandomShip(1);

		collisionMask = LayerMask.NameToLayer("Ships");

		playerNode = GameObject.Find("Player Node");
		battleObjects = GameObject.Find("Battle Objects").GetComponent<BattleObjects>();

		playerObject = battleObjects.InstantiateShip(0, GetPlayerPosition());
		enemyObject = battleObjects.InstantiateShip(2, GetEnemyPosition());

		FacePlayerAndEnemy();

		// hide player node
		playerNode.renderer.enabled = false;
	}

	private Vector2 GetPlayerPosition()
	{
		float cameraHeight = -Camera.main.transform.position.z;
		var screenPosition = new Vector3(100, 175, cameraHeight);

		return Camera.main.ScreenToWorldPoint(screenPosition);
	}

	private Vector2 GetEnemyPosition()
	{
		float cameraHeight = -Camera.main.transform.position.z;
		var screenPosition = new Vector3(width - 100, height - 125, cameraHeight);
		
		return Camera.main.ScreenToWorldPoint(screenPosition);
	}

	// Rotate ships to face eachother
	private void FacePlayerAndEnemy()
	{
		Vector2 diff = enemyObject.transform.position - playerObject.transform.position;
		float angle = Mathf.Rad2Deg*Mathf.Atan2(diff.x, diff.y);
		playerObject.transform.eulerAngles = new Vector3(0, 0, -angle);
		enemyObject.transform.eulerAngles = new Vector3(0, 0, 180 - angle);
	}

	public State UpdateState()
	{
		string playerHull = string.Format(StringTable.hullCaption, playerShip.Hull, playerShip.MaxHull);
		string playerShield = string.Format(StringTable.shieldCaption, playerShip.Shield, playerShip.MaxShield);
		string enemyHull = string.Format(StringTable.hullCaption, enemyShip.Hull, enemyShip.MaxHull);
		string enemyShield = string.Format(StringTable.shieldCaption, enemyShip.Shield, enemyShip.MaxShield);

		turns.Update();

		GUILayout.BeginArea(new Rect(5, 5, width-10, height-10));
		{
			GUILayout.BeginHorizontal(StringTable.boxStyle, GUILayout.ExpandWidth(true));
			{
				GUILayout.Label(enemyHull, StringTable.hullLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label(enemyShield, StringTable.shieldLabelStyle);
			}
			GUILayout.EndHorizontal();

			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical(StringTable.boxStyle, GUILayout.ExpandWidth(true));
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(playerHull, StringTable.hullLabelStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label(playerShield, StringTable.shieldLabelStyle);
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
					if(turns.PlayerTurn)
					{
						if(GUILayout.Button(StringTable.attackCaption)) Attack(playerShip, enemyShip, playerObject, enemyObject);
						if(GUILayout.Button(StringTable.fleeCaption)) Flee();
					}
					else
					{
						GUILayout.Label(StringTable.statusText, StringTable.hullLabelStyle);
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
		
		if(currentWindow != Windows.None)
		{
			GUI.ModalWindow(1, new Rect(0, height/4, width, height/2), ModalWindow, StringTable.battleWindowTitles[(int)currentWindow]);
		}
		else
		{
			if(turns.EnemyTurn) Attack(enemyShip, playerShip, enemyObject, playerObject);
		}

		if(turns.Finished && Event.current.type == EventType.Repaint)
		{
			playerNode.renderer.enabled = true;
			ShipPrefab.Destroy(playerObject.gameObject);
			ShipPrefab.Destroy(enemyObject.gameObject);

			if(playerShip.Alive) playerShip.ReplenishShield();
			else listener.ShipDestroyed();

			return returnToState;
		}

		return this;
	}

	private void Attack(Ship attacker, Ship target, ShipPrefab attackerObject, ShipPrefab targetObject)
	{
		int damage = target.TakeDamage(attacker.Damage);
		string damageText = damage > 0 ? damage.ToString() : StringTable.absorbCaption;
		var position = Camera.main.WorldToViewportPoint(targetObject.transform.position);

		if(!target.Alive) turns.EndTurns();

		battleObjects.InstantiateDamageText(damageText, position);

		var laserPostions = attackerObject.GetLaserbeamPositions();
		Vector2 targetPos = targetObject.transform.position;
		foreach(var laserPos in laserPostions)
		{
			var hit = Physics2D.Linecast(laserPos, targetPos, ~(1 << collisionMask));
			battleObjects.InstantiateLaserbeam(laserPos, hit.point);
		}

		turns.SwitchTurns();
	}

	private void Flee()
	{
		if(Random.value <= fleeChance) turns.EndTurns();
		else currentWindow = Windows.Flee;

		turns.SwitchTurns();
	}

	private void ModalWindow(int id)
	{
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);
			GUILayout.Label(StringTable.battleWindowTexts[(int)currentWindow], StringTable.normalLabelStyle);
			GUILayout.FlexibleSpace();

			if(GUILayout.Button(StringTable.okCaption)) currentWindow = Windows.None;
		}
		GUILayout.EndVertical();
	}
}
