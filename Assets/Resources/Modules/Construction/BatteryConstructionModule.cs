using UnityEngine;
using System.Collections;

public class BatteryConstructionModule : InternalConstructionModule {
	
	public const ModuleType type = ModuleType.Battery;

	[HideInInspector]
	public float energyLeft = 0f;

	private RelativeScaling contents;

	public BatteryConstructionModule() {
		stats = new BatteryModuleStats();
	}

	public BatteryModuleStats GetStats() {
		return (BatteryModuleStats) stats;
	}

	override protected void Start() {
		base.Start();
		contents = GetComponentInChildren<RelativeScaling>() as RelativeScaling;
	}

	override protected void Update() {
		base.Update();
		contents.max = GetStats().capacity;
		contents.current = energyLeft;
	}

	public float Remove(float energy) {
		float leftOver = energy - energyLeft;
		if (leftOver > 0f) {
			energyLeft = 0f;
			return leftOver;
		}
		else {
			energyLeft -= energy;
			return 0f;
		}
	}

	public float Add(float energy) {
		float leftOver = energy - ((float)GetStats().capacity - energyLeft);
		if (leftOver > 0f) {
			energyLeft = (float)GetStats().capacity;
			return leftOver;
		}
		else {
			energyLeft += energy;
			return 0f;
		}
	}

}
