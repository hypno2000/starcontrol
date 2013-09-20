using UnityEngine;

public class ExternalModuleManifest : ModuleManifest {
	
	public InternalModuleManifest internalModule;
	
	public ExternalModuleManifest() {
		
	}
	
	public ExternalModuleManifest(MonoBehaviour behaviour) : base(behaviour) {
		
	}
	
	public ExternalModuleManifest(Vector3 position, Quaternion rotation) : base(position, rotation) {
		
	}
	
}