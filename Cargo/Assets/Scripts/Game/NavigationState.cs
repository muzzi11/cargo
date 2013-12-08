using UnityEngine;
using System.Collections;

public class NavigationState : State
{
	private Ship ship;
	private Vector3 cursorPos = new Vector3();


	public NavigationState(Ship ship)
	{
		this.ship = ship;
	}

	public State UpdateState()
	{
		cursorPos.Set(Input.mousePosition.x, Input.mousePosition.y, 0);
		cursorPos = Camera.main.ScreenToWorldPoint(cursorPos);
		
		if(Input.GetMouseButtonUp(0)) ship.SetDestination(cursorPos.x, cursorPos.y);

		return this;
	}
}
