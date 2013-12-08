using UnityEngine;
using System.Collections;

public class TitleScreenState : State
{
	private int width, height;
	private OptionsState optionsState;
	private string startGameCaption = "Start Game";
	private string optionsCaption = "Option";
	private string buttonStyle = "titleScreenButton";

	public TitleScreenState()
	{
		width = Screen.width;
		height = Screen.height;
		optionsState = new OptionsState(this);
	}

	public State UpdateState()
	{
		GUI.BeginGroup(new Rect(width / 2 - 100, height - 200, 200, 100));
		{
			if(GUI.Button(new Rect(0, 0, 200, 40), startGameCaption, buttonStyle)) Application.LoadLevel(1);

			if(GUI.Button(new Rect(0, 50, 200, 40), optionsCaption, buttonStyle)) return optionsState;
		}
		GUI.EndGroup();

		return this;
	}
}
