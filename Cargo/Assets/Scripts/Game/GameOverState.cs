using UnityEngine;
using System.Collections;

public class GameOverState : State
{
	private int width, height;
	private const string 
		windowCaption = "GameOver",
		windowText = "Your ship was destroyed, you failed...",
		normalLabelStyle = "normalLabel";

	public GameOverState()
	{
		width = Screen.width;
		height = Screen.height;
	}

	public State UpdateState()
	{
		GUI.Window(1, new Rect(0, 0, width, height), Window, windowCaption);
		return this;
	}

	private void Window(int id)
	{
		GUILayout.FlexibleSpace();
		GUILayout.Label(windowText, normalLabelStyle);
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Main menu")) Application.LoadLevel(0);
	}
}
