using UnityEngine;
using System.Collections;

public class OptionsState : State
{
	private int width, height;
	private Texture bgTexture;
	private State returnToState;

	public OptionsState(State returnToState)
	{
		width = Screen.width;
		height = Screen.height;
		bgTexture = Resources.Load<Texture> ("titlescreen");
		this.returnToState = returnToState;
	}

	public State UpdateState()
	{
		GUI.DrawTexture (new Rect (0, 0, width, height), bgTexture, ScaleMode.StretchToFill);

		if (GUI.Button (new Rect (10, 10, 100, 40), "Back")) return returnToState;

		return this;
	}
}
