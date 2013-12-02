using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		GUI.Button(new Rect(Screen.width/2, Screen.height/2, 100, 100), "bla");
	}
}
