using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public GUISkin mySkin;
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
		GUI.skin = mySkin;
		currentState = currentState.UpdateState();
	}
}
