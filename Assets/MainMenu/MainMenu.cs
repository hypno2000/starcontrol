using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public GUISkin skin;
	
	void OnGUI() {
		GUI.skin = skin;
			
		// Make a group on the center of the screen
		GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));
		// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

		// We'll make a box so you can see where the group is on-screen.
		GUI.Box(new Rect(0, 0, 200, 200), "Main Menu");
		if (GUI.Button(new Rect(10, 40, 180, 30), "Start Game")) {
			Application.LoadLevel("Construction");
		}
		
		// End the group we started above. This is very important to remember!
		GUI.EndGroup();

	}
	
}
