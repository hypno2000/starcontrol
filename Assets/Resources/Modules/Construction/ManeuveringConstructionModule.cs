using UnityEngine;
using System.Collections;

public class ManeuveringConstructionModule : ExternalConstructionModule {
	
	public const ModuleType type = ModuleType.Maneuvering;

	public ManeuveringConstructionModule() {
		stats = new ManeuveringModuleStats();
	}

	public ManeuveringModuleStats GetStats() {
		return (ManeuveringModuleStats) stats;
	}

}
