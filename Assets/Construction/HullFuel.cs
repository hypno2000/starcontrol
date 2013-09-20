using UnityEngine;
using System;
using System.Collections.Generic;

public class HullFuel {

	public List<FuelConstructionModule> fuelTanks;

	public HullFuel() {
		fuelTanks = new List<FuelConstructionModule>();
	}

	public float GetFuelLeft() {
		float fuel = 0f;
		foreach (var mod in fuelTanks) {
			if (!mod.isActive) {
				continue;
			}
			fuel += mod.fuelLeft;
		}
		return fuel;
	}

	public float GetMaxFuel() {
		int fuel = 0;
		foreach (var mod in fuelTanks) {
			if (!mod.isActive) {
				continue;
			}
			fuel += mod.GetStats().capacity;
		}
		return (float)fuel;
	}

	public bool RemoveFuel(float toRemove) {
		if (toRemove > GetFuelLeft()) {
			return false;
		}
		foreach (var mod in fuelTanks) {
			if (!mod.isActive) {
				continue;
			}
			toRemove = mod.Remove(toRemove);
			if (toRemove == 0f) {
				return true;
			}
		}
		return false;
	}

	public void AddFuel(float toAdd) {
		foreach (var mod in fuelTanks) {
			if (!mod.isActive) {
				continue;
			}
			toAdd = mod.Add(toAdd);
			if (toAdd == 0f) {
				return;
			}
		}
	}

}


