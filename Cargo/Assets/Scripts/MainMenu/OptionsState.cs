using UnityEngine;
using System.Collections;

public class OptionsState : State
{
	private int width, height;
	private State returnToState;

	public OptionsState(State returnToState)
	{
		width = Screen.width;
		height = Screen.height;
		this.returnToState = returnToState;
	}

	public State UpdateState()
	{
		GUILayout.BeginArea(new Rect(0, 0, width, height));
		{
			if(GUILayout.Button("Back", "titleScreenButton")) return returnToState;
		}
		GUILayout.EndArea();

		return this;
	}
}
