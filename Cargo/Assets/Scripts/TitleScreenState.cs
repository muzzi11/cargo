using UnityEngine;
using System.Collections;

public class TitleScreenState : State
{
	private int width, height;
	private OptionsState optionsState;

	public TitleScreenState()
	{
		width = Screen.width;
		height = Screen.height;
		optionsState = new OptionsState (this);
	}

	public State UpdateState()
	{
		GUI.BeginGroup (new Rect (width / 2 - 100, height - 200, 200, 100));
		{
			if(GUI.Button (new Rect (0, 0, 200, 40), "Start Game", "titleScreenButton")) Application.LoadLevel(1);

			if(GUI.Button (new Rect (0, 50, 200, 40), "Options", "titleScreenButton")) return optionsState;
		}
		GUI.EndGroup ();

		return this;
	}
}
