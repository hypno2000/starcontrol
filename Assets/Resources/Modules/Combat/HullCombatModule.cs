using UnityEngine;
using System.Collections;

public class HullCombatModule : ExternalCombatModule {

	public const ModuleType type = ModuleType.Hull;

	[HideInInspector]
	public InternalCombatModule internalModule;

	public HullCombatModule() {
		stats = new HullModuleStats();
	}

	public HullModuleStats GetStats() {
		return (HullModuleStats) stats;
	}

	override protected void Start() {
		base.Start();
	}

	protected override void OnDestroy() {
		if (ship == null) {
			return;
		}
		ship.modules.Remove(this);
	}

}
