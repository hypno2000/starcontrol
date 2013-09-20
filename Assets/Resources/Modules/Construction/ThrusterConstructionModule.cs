using UnityEngine;
using System.Collections;

public class ThrusterConstructionModule : ExternalConstructionModule {
	
	public const ModuleType type = ModuleType.Thruster;

	public ThrusterConstructionModule() {
		stats = new ThrusterModuleStats();
	}

	public ThrusterModuleStats GetStats() {
		return (ThrusterModuleStats) stats;
	}

}
