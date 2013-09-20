using UnityEngine;
using System.Collections;

public class MenuItemActions : MonoBehaviour {
	
	void OnSelect(string command) {
		Debug.Log("A Menu Command Received: " + command);	
	}
	
}
