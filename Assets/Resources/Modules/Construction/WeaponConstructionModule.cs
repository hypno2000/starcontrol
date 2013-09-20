using UnityEngine;
using System.Collections;

public class WeaponConstructionModule : ExternalConstructionModule {
	
	public const ModuleType type = ModuleType.Weapon;

	public WeaponConstructionModule() {
		stats = new WeaponModuleStats();
	}

	public WeaponModuleStats GetStats() {
		return (WeaponModuleStats) stats;
	}

}
