using UnityEngine;
using System.Collections;

public class HullConstructionModule : ExternalConstructionModule {
	
	public const ModuleType type = ModuleType.Hull;

	[HideInInspector]
	public InternalConstructionModule internalModule;

	[HideInInspector]
	public bool available;

	public HullConstructionModule() {
		stats = new HullModuleStats();
	}

	public HullModuleStats GetStats() {
		return (HullModuleStats) stats;
	}

	protected void Awake() {
		available = true;
	}

	public void Activate() {
		if (internalModule != null) {
			return;
		}
		gameObject.layer = 0;
		available = true;
	}

	public void Deactivate() {
		gameObject.layer = 2;
		available = false;
	}

}
