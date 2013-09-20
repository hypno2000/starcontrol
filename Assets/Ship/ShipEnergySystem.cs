using UnityEngine;
using System;
using System.Collections.Generic;

public class ShipEnergySystem {

	protected Ship ship;
	public List<BatteryCombatModule> batteries;
	public List<PowerCombatModule> powerPlants;

	public ShipEnergySystem(Ship ship) {
		this.ship = ship;
		batteries = new List<BatteryCombatModule>();
		powerPlants = new List<PowerCombatModule>();
	}

	public void AddBattery(BatteryCombatModule module) {
		module.system = this;
		batteries.Add(module);
		ship.modules.Add(module);
		ship.rigidbody.mass += module.stats.mass;
	}

	public void RemoveBattery(BatteryCombatModule module) {
		batteries.Remove(module);
		ship.modules.Remove(module);
		module.system = null;
		ship.rigidbody.mass -= module.stats.mass;
		ship.CalcBounds();
	}

	public void AddPowerPlant(PowerCombatModule module) {
		module.system = this;
		powerPlants.Add(module);
		ship.modules.Add(module);
		ship.rigidbody.mass += module.stats.mass;
	}

	public void RemovePowerPlant(PowerCombatModule module) {
		powerPlants.Remove(module);
		ship.modules.Remove(module);
		module.system = null;
		ship.rigidbody.mass -= module.stats.mass;
		ship.CalcBounds();
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


