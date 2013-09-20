using UnityEngine;
using System;
using System.Collections.Generic;

public class HullEnergy {

	public List<BatteryConstructionModule> batteries;
	public List<PowerConstructionModule> powerPlants;

	public HullEnergy() {
		batteries = new List<BatteryConstructionModule>();
		powerPlants = new List<PowerConstructionModule>();
	}

	public float GetEnergyLeft() {
		float energy = 0f;
		foreach (var mod in batteries) {
			if (!mod.isActive) {
				continue;
			}
			energy += mod.energyLeft;
		}
		return energy;
	}

	public float GetMaxEnergy() {
		float energy = 0f;
		foreach (var mod in batteries) {
			if (!mod.isActive) {
				continue;
			}
			energy += mod.GetStats().capacity;
		}
		return energy;
	}

	public bool Consume(float energy) {
		if (energy > GetEnergyLeft()) {
			return false;
		}
		foreach (var mod in batteries) {
			if (!mod.isActive) {
				continue;
			}
			energy = mod.Remove(energy);
			if (energy == 0f) {
				return true;
			}
		}
		throw new SystemException("This should not happen");
	}

	public void Generate(float time) {

		// get generated energy
		float energy = 0f;
		foreach (var mod in powerPlants) {
			if (!mod.isActive) {
				continue;
			}
			energy += (float)mod.GetStats().power * time;
		}
		if (energy == 0f) {
			return;
		}

		// add it to the batteries
		foreach (var mod in batteries) {
			if (!mod.isActive) {
				continue;
			}
			energy = mod.Add(energy);
			if (energy == 0f) {
				return;
			}
		}

	}

}


