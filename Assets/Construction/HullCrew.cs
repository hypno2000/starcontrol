using UnityEngine;
using System;
using System.Collections.Generic;

public class HullCrew {

	public List<CrewConstructionModule> crewPods;

	public HullCrew() {
		crewPods = new List<CrewConstructionModule>();
	}

	public int GetCrewLeft() {
		int persons = 0;
		foreach (var mod in crewPods) {
			if (!mod.isActive) {
				continue;
			}
			persons += mod.crewLeft;
		}
		return persons;
	}

	public int GetMaxCrew() {
		int persons = 0;
		foreach (var mod in crewPods) {
			if (!mod.isActive) {
				continue;
			}
			persons += mod.GetStats().capacity;
		}
		return persons;
	}

	public bool RemovePersons(int toRemove) {
		foreach (var mod in crewPods) {
			if (!mod.isActive) {
				continue;
			}
			toRemove = mod.Remove(toRemove);
			if (toRemove == 0) {
				return false;
			}
		}
		return true;
	}

	public void AddPersons(int toAdd) {
		foreach (var mod in crewPods) {
			if (!mod.isActive) {
				continue;
			}
			toAdd = mod.Add(toAdd);
			if (toAdd == 0) {
				return;
			}
		}
	}

}


