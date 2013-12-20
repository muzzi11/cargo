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
		GUILayout.BeginArea(new Rect(0, 0, width, height));
		{
			GUILayout.FlexibleSpace();

			if(GUILayout.Button(startGameCaption, buttonStyle)) Application.LoadLevel(1);
			if(GUILayout.Button(optionsCaption, buttonStyle)) return optionsState;

			GUILayout.Space(50);
		}
		GUILayout.EndArea();

		return this;
	}
}
