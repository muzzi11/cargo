using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	public Rect cameraBounds;

	private Ship ship;
	private State currentState;
	private GameObject playerNode;

	// Use this for initialization
	void Start()
	{
		ship = new Ship(new Vector2(15, 15));
		currentState = new NavigationState(ship);
		playerNode = GameObject.Find("Player Node");
	}
	
	// Update is called once per frame
	void Update()
	{
		ship.Update();
		playerNode.transform.position = ship.GetPosition();
		UpdateCameraPosition();
	}

	void OnGUI()
	{
		currentState = currentState.UpdateState();
	}

	private void UpdateCameraPosition()
	{
		Vector3 cameraPos = ship.GetPosition();

		cameraPos.x = Mathf.Clamp(cameraPos.x, cameraBounds.xMin, cameraBounds.xMax);
		cameraPos.y = Mathf.Clamp(cameraPos.y, cameraBounds.yMin, cameraBounds.yMax);
		cameraPos.z = Camera.main.transform.position.z;
		
		Camera.main.transform.position = cameraPos;
	}
}
