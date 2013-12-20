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
		optionsState = new OptionsState(this);
	}

	public State UpdateState()
	{
		GUILayout.BeginArea(new Rect(0, 0, width, height));
		{
			GUILayout.FlexibleSpace();

			if(GUILayout.Button(StringTable.startGameCaption, StringTable.titleScreenButtonStyle)) Application.LoadLevel(1);
			if(GUILayout.Button(StringTable.optionsCaption, StringTable.titleScreenButtonStyle)) return optionsState;

			GUILayout.Space(50);
		}
		GUILayout.EndArea();

		return this;
	}
}
