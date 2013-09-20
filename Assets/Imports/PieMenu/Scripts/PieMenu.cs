using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieMenu : MonoBehaviour
{

	public bool mainMenu = true;
	public string menuName = "PieMenu";
	public PieMenuItem[] menuItems;
	
	public float iconSize = 64f;
	public float spacing = 12f;
	public float speed = 8f;
	public GUISkin skin;
	public Rect tooltipSize = new Rect(0,0,150,40);
	
	[HideInInspector]
	public float scale;
	[HideInInspector]
	public float angle;
	
	PieMenuManager manager;
	
	void Awake () {
		manager = PieMenuManager.Instance;
	}
	
	void Start() {
		var foundMainMenu = false;
		foreach(var i in gameObject.GetComponents<PieMenu>()) {
			if(i.mainMenu) {
				if(!foundMainMenu)
					foundMainMenu = true;
				else
					Debug.LogError("Only one PieMenu should be marked as Main Menu per game object.");
			}
		}
	}
	
	void OnMouseUp() {
		if(mainMenu)
			manager.Show(this);
	}
	
	void OnSelect(string cmd) {
		if(cmd == menuName) {
			manager.Show(this);
		}
	}
	
}

[System.Serializable]
public class PieMenuItem {
	public string command;
	public Texture2D icon;
	public string tooltip;
}
