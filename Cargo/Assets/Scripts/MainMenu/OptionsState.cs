using UnityEngine;
using System.Collections;

public class OptionsState : State
{
	private int width, height;
	private State returnToState;

	enum AudioMute : int
	{
		On,
		Off
	};
	private AudioMute mode = AudioMute.Off;
	private AudioMute Mode
	{
		set 
		{ 
			mode = value; 
			if (mode == AudioMute.On) AudioListener.pause = true; 
			else AudioListener.pause = false;
		}
	}

	public OptionsState(State returnToState)
	{
		width = Screen.width;
		height = Screen.height;
		this.returnToState = returnToState;
	}

	public State UpdateState()
	{
		GUILayout.BeginArea(new Rect(0, 0, width, height));
		{
			GUILayout.BeginVertical();
			{
				GUILayout.FlexibleSpace();
				GUILayout.Label(StringTable.muteCaption, StringTable.titleScreenLabelStye);

				string[] toolbarStrings = {StringTable.onCaption, StringTable.offCaption};
				Mode = (AudioMute)GUILayout.Toolbar((int)mode, toolbarStrings);

				if(GUILayout.Button(StringTable.backCaption, StringTable.titleScreenButtonStyle)) return returnToState;
				GUILayout.Space(50);
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();

		return this;
	}
}
