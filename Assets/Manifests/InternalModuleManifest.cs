using UnityEngine;

public class InternalModuleManifest : ModuleManifest {

	public InternalModuleManifest() {

	}
	
	public InternalModuleManifest(MonoBehaviour behaviour) : base(behaviour) {
		
	}
	
	public InternalModuleManifest(Vector3 position, Quaternion rotation) : base(position, rotation) {
		
	}
	
}