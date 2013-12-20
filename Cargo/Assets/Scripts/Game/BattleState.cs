using UnityEngine;
using System.Collections.Generic;

public class BattleState : State
{
	private enum Action
	{
		Attack,
		Flee
	}

	class Turn
	{
		private float turnStartTime = Time.time;
		private bool playerTurn = true, ready = true;

		public void Update()
		{
			if(Time.time >= turnStartTime) ready = true;
		}

		public void SwitchTurns()
		{
			const float turnDelay = 2.0f;
			turnStartTime = Time.time + turnDelay;
			ready = false;
			playerTurn = !playerTurn;
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
	private const string attackCaption = "Attack";
	private const string fleeCaption = "Flee";
	private const string absorbCaption = "Absorb";
	private const string hullCaption = "HULL {0}/{1}";
	private const string shieldCaption = "SHIELD {0}/{1}";
	private const string hullLabelStyle = "hullLabel";
	private const string shieldLabelStyle = "shieldLabel";
	private const string boxStyle = "box";

	private readonly int width, height;
	private readonly State returnToState;

	private Ship playerShip, enemyShip;

	private BattleObjects battleObjects;
	private GameObject playerNode;
	private ShipPrefab playerObject, enemyObject;
	
	private Turn turn = new Turn();
	private bool battleOver = false;

	public BattleState(State returnToState, Ship ship)
	{
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
		string playerHull = string.Format(hullCaption, playerShip.Hull, playerShip.MaxHull);
		string playerShield = string.Format(shieldCaption, playerShip.Shield, playerShip.MaxShield);
		string enemyHull = string.Format(hullCaption, enemyShip.Hull, enemyShip.MaxHull);
		string enemyShield = string.Format(shieldCaption, enemyShip.Shield, enemyShip.MaxShield);

		turn.Update();

		GUILayout.BeginArea(new Rect(5, 5, width-10, height-10));
		{
			GUILayout.BeginHorizontal(boxStyle, GUILayout.ExpandWidth(true));
			{
				GUILayout.Label(enemyHull, hullLabelStyle);
				GUILayout.FlexibleSpace();
				GUILayout.Label(enemyShield, shieldLabelStyle);
			}
			GUILayout.EndHorizontal();

			GUILayout.FlexibleSpace();

			GUILayout.BeginVertical(boxStyle, GUILayout.ExpandWidth(true));
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(playerHull, hullLabelStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Label(playerShield, shieldLabelStyle);
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
					if(GUILayout.Button(attackCaption)) PerformAction(Action.Attack);
					if(GUILayout.Button(fleeCaption)) PerformAction(Action.Flee);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();

		if(turn.EnemyTurn)
		{
			Attack(enemyShip, playerShip, enemyObject, playerObject);
			turn.SwitchTurns();
		}

		if(battleOver)
		{
			playerNode.renderer.enabled = true;
			ShipPrefab.Destroy(playerObject.gameObject);
			ShipPrefab.Destroy(enemyObject.gameObject);
		}

		return battleOver ? returnToState : this;
	}

	private void PerformAction(Action action)
	{
		if(turn.PlayerTurn)
		{
			switch(action)
			{
			case Action.Attack:
				Attack(playerShip, enemyShip, playerObject, enemyObject);
				if(!enemyShip.Alive) battleOver = true;
				break;

			case Action.Flee:
				break;
			}

			turn.SwitchTurns();
		}
	}

	private void Attack(Ship attacker, Ship target, ShipPrefab attackerObject, ShipPrefab targetObject)
	{
		int damage = target.TakeDamage(attacker.Damage);
		string damageText = damage > 0 ? damage.ToString() : absorbCaption;
		var position = Camera.main.WorldToViewportPoint(targetObject.transform.position);

		battleObjects.InstantiateDamageText(damageText, position);

		var laserPostions = attackerObject.GetLaserbeamPositions();
		Vector2 targetPos = targetObject.transform.position;
		foreach(var laserPos in laserPostions)
		{
			var hit = Physics2D.Linecast(laserPos, targetPos, ~(1 << collisionMask));
			Debug.Log(laserPos);
			Debug.Log(hit.point);
			battleObjects.InstantiateLaserbeam(laserPos, hit.point);
		}
	}
}
