using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public int x = 5, y = 5;
	public GUIStyle button; 
	public GUISkin mySkin;

	private State currentState;
	
	// Use this for initialization
	void Start () {
		currentState = new TitleScreenState ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnGUI()
	{
		/*
		GUI.skin = mySkin;
		if(GUI.Button(new Rect(x,y,150,100), "Start game"))
		{
			Application.LoadLevel(1);
		}*/

		currentState = currentState.UpdateState ();
	}
}
