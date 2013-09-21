using UnityEngine;
using System.Collections.Generic;

public class InventoryManifest : PositionalManifest {
	
	public List<InventorySlotManifest> slots = new List<InventorySlotManifest>();

	public InventoryManifest() {

	}

	public InventoryManifest(MonoBehaviour behaviour) {
		
	}
	
}