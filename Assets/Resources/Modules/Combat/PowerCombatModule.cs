using UnityEngine;
using System.Collections;

public class PowerCombatModule : InternalCombatModule {

	public const ModuleType type = ModuleType.Power;
	public ShipEnergySystem system;

	public PowerCombatModule() {
		stats = new PowerModuleStats();
	}

	public PowerModuleStats GetStats() {
		return (PowerModuleStats)stats;
	}

	override protected void Start() {
		base.Start();
	}

	protected override void OnDestroy() {
		if (system == null) {
			return;
		}
		system.RemovePowerPlant(this);
	}
}
