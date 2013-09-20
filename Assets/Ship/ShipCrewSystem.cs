using UnityEngine;
using System;
using System.Collections.Generic;

public class ShipCrewSystem {

	protected Ship ship;
	public List<CrewCombatModule> crewPods;

	public ShipCrewSystem(Ship ship) {
		this.ship = ship;
		crewPods = new List<CrewCombatModule>();
	}

	public void AddCrewPod(CrewCombatModule module) {
		module.system = this;
		crewPods.Add(module);
		ship.modules.Add(module);
		ship.rigidbody.mass += module.stats.mass;
	}

	public void RemoveCrewPod(CrewCombatModule module) {
		crewPods.Remove(module);
		ship.modules.Remove(module);
		module.system = null;
		ship.rigidbody.mass -= module.stats.mass;
		ship.CalcBounds();
	}

	public int GetCrewLeft() {
		int persons = 0;
		foreach (var mod in crewPods) {
			persons += mod.crewLeft;
		}
		return persons;
	}

	public int GetMaxCrew() {
		int persons = 0;
		foreach (var mod in crewPods) {
			persons += mod.GetStats().capacity;
		}
		return persons;
	}

	public bool RemovePersons(int toRemove) {
		foreach (var mod in crewPods) {
			toRemove = mod.Remove(toRemove);
			if (toRemove == 0) {
				return false;
			}
		}
		return true;
	}

	public void AddPersons(int toAdd) {
		foreach (var mod in crewPods) {
			toAdd = mod.Add(toAdd);
			if (toAdd == 0) {
				return;
			}
		}
	}

}


