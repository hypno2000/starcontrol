using UnityEngine;
using System.Collections;

public class ThrusterCombatModule : ExternalCombatModule {
	
	public const ModuleType type = ModuleType.Thruster;

	[HideInInspector]
	public ThrusterEffect effect;

	public ShipPropulsionSystem system;

	public ThrusterCombatModule() {
		stats = new ThrusterModuleStats();
	}

	public ThrusterModuleStats GetStats() {
		return (ThrusterModuleStats) stats;
	}
	
	override protected void Start() {
		base.Start();
		effect = GetComponentInChildren<ThrusterEffect>();
	}

	protected override void OnDestroy() {
		if (system == null) {
			return;
		}
		system.RemoveThruster(this);
	}
	
}
