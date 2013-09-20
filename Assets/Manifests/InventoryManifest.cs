using UnityEngine;
using System.Collections.Generic;

public class InventoryManifest : PositionalManifest {
	
	public List<ModuleManifest> modules = new List<ModuleManifest>();

	public InventoryManifest() {

	}

	public InventoryManifest(MonoBehaviour behaviour) {
		
	}
	
}