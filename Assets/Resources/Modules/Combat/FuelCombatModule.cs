using UnityEngine;
using System.Collections;

public class FuelCombatModule : InternalCombatModule {

	public const ModuleType type = ModuleType.Fuel;

	[HideInInspector]
	public float fuelLeft = 0f;

	private RelativeScaling contents;

	public ShipPropulsionSystem system;

	public FuelCombatModule() {
		stats = new FuelModuleStats();
	}

	public FuelModuleStats GetStats() {
		return (FuelModuleStats) stats;
	}

	override protected void Start() {
		base.Start();
		contents = GetComponentInChildren<RelativeScaling>() as RelativeScaling;
	}

	override protected void Update() {
		base.Update();
		contents.max = GetStats().capacity;
		contents.current = fuelLeft;
	}

	public float Remove(float toRemove) {
		float leftOver = toRemove - fuelLeft;
		if (leftOver > 0f) {
			fuelLeft = 0f;
			return leftOver;
		}
		else {
			fuelLeft -= toRemove;
			return 0f;
		}
	}

	public float Add(float toAdd) {
		float leftOver = toAdd - (GetStats().capacity - fuelLeft);
		if (leftOver > 0f) {
			fuelLeft = GetStats().capacity;
			return leftOver;
		}
		else {
			fuelLeft += toAdd;
			return 0f;
		}
	}

	protected override void OnDestroy() {
		if (system == null) {
			return;
		}
		system.RemoveFuelTank(this);
	}

}
