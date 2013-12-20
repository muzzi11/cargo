using UnityEngine;
using System.Collections;

public class GameOverState : State
{
	private int width, height; 

	public GameOverState()
	{
		width = Screen.width;
		height = Screen.height;
	}

	public State UpdateState()
	{
		GUI.Window(1, new Rect(0, 0, width, height), Window, StringTable.windowCaption);
		return this;
	}

	private void Window(int id)
	{
		GUILayout.FlexibleSpace();
		GUILayout.Label(StringTable.windowText, StringTable.normalLabelStyle);
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Main menu")) Application.LoadLevel(0);
	}
}
