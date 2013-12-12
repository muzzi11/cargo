using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public GUISkin guiSkin;
	private State currentState;

	void Start()
	{
		currentState = new TitleScreenState();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}
	
	void OnGUI()
	{
		GUI.skin = guiSkin;
		currentState = currentState.UpdateState();
	}
}
