using UnityEngine;
using System.Collections;

public class InventorySlot : MonoBehaviour {

	public bool available;
	public ConstructionModule module;
	public int number = 0;

	void Awake() {
		available = true;
	}

	public void Activate() {
		if (module != null) {
			return;
		}
		//		renderer.enabled = true;
		gameObject.layer = 0;
		available = true;
	}

	public void Deactivate() {
		//		renderer.enabled = false;
		gameObject.layer = 2;
		available = false;
	}

}

