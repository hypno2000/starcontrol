using UnityEngine;
using System.Collections;

public class PowerConstructionModule : InternalConstructionModule {
	
	public const ModuleType type = ModuleType.Power;

	public PowerConstructionModule() {
		stats = new PowerModuleStats();
	}

	public PowerModuleStats GetStats() {
		return (PowerModuleStats) stats;
	}

}
