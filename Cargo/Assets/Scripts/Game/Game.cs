using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	public Rect cameraBounds;
	public GUISkin guiSkin;

	private Ship ship;
	private Cargo cargo;
	private Space space;
	private Balance balance;
	private State currentState;
	private GameObject playerNode;

	// Use this for initialization
	void Start()
	{
		ship = new Ship(new Vector2(0, 0));
		cargo = new Cargo(200);
		space = GameObject.Find("Space").GetComponent<Space>();
		space.GeneratePlanets(cameraBounds);
		balance = new Balance();
		currentState = new NavigationState(space, ship, balance, cargo);
		currentState = new BattleState(currentState, ship);
		playerNode = GameObject.Find("Player Node");
	}
	
	// Update is called once per frame
	void Update()
	{
		ship.Update();
		playerNode.transform.position = ship.Position;
		UpdateCameraPosition();

		if(Input.GetKeyDown(KeyCode.Escape)) Application.LoadLevel(0);
	}

	void OnGUI()
	{
		GUI.skin = guiSkin;
		currentState = currentState.UpdateState();
	}

	private void UpdateCameraPosition()
	{
		Vector3 cameraPos = ship.Position;

		cameraPos.x = Mathf.Clamp(cameraPos.x, cameraBounds.xMin, cameraBounds.xMax);
		cameraPos.y = Mathf.Clamp(cameraPos.y, cameraBounds.yMin, cameraBounds.yMax);
		cameraPos.z = Camera.main.transform.position.z;
		
		Camera.main.transform.position = cameraPos;
	}
}
