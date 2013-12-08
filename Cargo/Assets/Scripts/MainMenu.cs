using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public GUISkin mySkin;

	private State currentState;
	
	// Use this for initialization
	void Start()
	{
		currentState = new TitleScreenState ();
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
	
	void OnGUI()
	{
		GUI.skin = mySkin;
		currentState = currentState.UpdateState ();
	}
}
