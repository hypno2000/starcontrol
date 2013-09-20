using UnityEngine;

public class ModuleManifest : PositionalManifest {
	
	public int inventorySlotIndex;
	public ModuleType type;
	public float hitPointsLeft;
	public float contents;

	public ModuleStats stats;

	public ModuleManifest() {

	}

	public ModuleManifest(MonoBehaviour behaviour) : base(behaviour) {

	}

	public ModuleManifest(Vector3 position, Quaternion rotation) : base(position, rotation) {

	}

}