using UnityEngine;
using System.Collections;

public class BattleState : State
{
	State returnToState;

	public BattleState(State returnToState)
	{
		this.returnToState = returnToState;
	}

	public State UpdateState()
	{
		return this;
	}
}
