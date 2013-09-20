using UnityEngine;
using System.Collections;

public class ManeuveringCombatModule : ExternalCombatModule {
	
	public const ModuleType type = ModuleType.Maneuvering;

	[HideInInspector]
	public ThrusterEffect effect;

	public ShipPropulsionSystem system;

	public ManeuveringCombatModule() {
		stats = new ManeuveringModuleStats();
	}

	public ManeuveringModuleStats GetStats() {
		return (ManeuveringModuleStats) stats;
	}

	override protected void Start() {
		base.Start();
		effect = GetComponentInChildren<ThrusterEffect>();
	}

	protected override void OnDestroy() {
		if (system == null) {
			return;
		}
		system.RemoveManeuverer(this);
	}
	
}
