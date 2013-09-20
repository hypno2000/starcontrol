using UnityEngine;
using System;
using System.Collections.Generic;

public class ShipPropulsionSystem {

	protected Ship ship;
	public List<ThrusterCombatModule> thrusters;
	public List<ManeuveringCombatModule> leftManeuverers;
	public List<ManeuveringCombatModule> rightManeuverers;
	public List<ManeuveringCombatModule> backManeuverers;
	public List<FuelCombatModule> fuelTanks;

	public ShipPropulsionSystem(Ship ship) {
		this.ship = ship;
		thrusters = new List<ThrusterCombatModule>();
		leftManeuverers = new List<ManeuveringCombatModule>();
		rightManeuverers = new List<ManeuveringCombatModule>();
		backManeuverers = new List<ManeuveringCombatModule>();
		fuelTanks = new List<FuelCombatModule>();
	}

	public void AddThruster(ThrusterCombatModule module) {
		module.system = this;
		thrusters.Add(module);
		ship.modules.Add(module);
		ship.rigidbody.mass += module.stats.mass;
	}

	public void RemoveThruster(ThrusterCombatModule module) {
		thrusters.Remove(module);
		ship.modules.Remove(module);
		module.system = null;
		ship.rigidbody.mass -= module.stats.mass;
		ship.CalcBounds();
	}

	public void AddManeuverer(ManeuveringCombatModule module) {
		module.system = this;
		float angle = module.transform.localRotation.eulerAngles.z;
		if (angle >= 225) {
			rightManeuverers.Add(module);
		} else if (angle >= 0 && angle <= 135) {
			leftManeuverers.Add(module);
		} else {
			backManeuverers.Add(module);
		}
		ship.modules.Add(module);
		ship.rigidbody.mass += module.stats.mass;
	}

	public void RemoveManeuverer(ManeuveringCombatModule module) {
		leftManeuverers.Remove(module);
		rightManeuverers.Remove(module);
		backManeuverers.Remove(module);
		ship.modules.Remove(module);
		module.system = null;
		ship.rigidbody.mass -= module.stats.mass;
		ship.CalcBounds();
	}

	public void AddFuelTank(FuelCombatModule module) {
		module.system = this;
		fuelTanks.Add(module);
		ship.modules.Add(module);
		ship.rigidbody.mass += module.stats.mass;
	}

	public void RemoveFuelTank(FuelCombatModule module) {
		fuelTanks.Remove(module);
		ship.modules.Remove(module);
		module.system = null;
		ship.rigidbody.mass -= module.stats.mass;
		ship.CalcBounds();
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

	// forward
	public void Thrust() {
		foreach (ThrusterCombatModule mod in thrusters) {
			if (!mod.isActive) {
				continue;
			}
			if (RemoveFuel(mod.GetStats().consumption * mod.transform.lossyScale.x * Time.deltaTime / 60f)) {
//				mod.rigidbody.AddForce(mod.transform.up * Time.deltaTime * mod.GetStats().thrust * 600);
				ship.rigidbody.AddForceAtPosition(mod.transform.up * mod.transform.lossyScale.x * Time.deltaTime * mod.GetStats().thrust * 600f, mod.transform.position);
				if (!mod.effect.isActive) {
					mod.effect.startThruster();
				}
			}
			else if (mod.effect != null && mod.effect.isActive) {
				ship.StartCoroutine(mod.effect.stopThruster());
			}
		}
	}

	public void StopThrusters() {
		foreach (ThrusterCombatModule mod in thrusters) {
			if (!mod.isActive) {
				continue;
			}
			if (mod.effect != null && mod.effect.isActive) {
				ship.StartCoroutine(mod.effect.stopThruster());
			}
		}
	}

	// left
	public void ManeuverLeft() {
		foreach (ManeuveringCombatModule mod in leftManeuverers) {
			if (!mod.isActive) {
				continue;
			}
			if (RemoveFuel(mod.GetStats().consumption * mod.transform.lossyScale.x * Time.deltaTime / 60f)) {
//				mod.rigidbody.AddForce(mod.transform.up * Time.deltaTime * mod.GetStats().thrust * 400);
				ship.rigidbody.AddForceAtPosition(mod.transform.up * mod.transform.lossyScale.x * Time.deltaTime * mod.GetStats().thrust * 400f, mod.transform.position);
				if (!mod.effect.isActive) {
					mod.effect.startThruster();
				}
			}
			else if (mod.effect != null && mod.effect.isActive) {
				ship.StartCoroutine(mod.effect.stopThruster());
			}
		}
	}

	public void StopLeftManeuverers() {
		foreach (ManeuveringCombatModule mod in leftManeuverers) {
			if (!mod.isActive) {
				continue;
			}
			if (mod.effect != null && mod.effect.isActive) {
				ship.StartCoroutine(mod.effect.stopThruster());
			}
		}
	}

	// right
	public void ManeuverRight() {
		foreach (ManeuveringCombatModule mod in rightManeuverers) {
			if (!mod.isActive) {
				continue;
			}
			if (RemoveFuel(mod.GetStats().consumption * mod.transform.lossyScale.x * Time.deltaTime / 60f)) {
//				mod.rigidbody.AddForce(mod.transform.up * Time.deltaTime * mod.GetStats().thrust * 400);
				ship.rigidbody.AddForceAtPosition(mod.transform.up * mod.transform.lossyScale.x * Time.deltaTime * mod.GetStats().thrust * 400f, mod.transform.position);
				if (!mod.effect.isActive) {
					mod.effect.startThruster();
				}
			}
			else if (mod.effect != null && mod.effect.isActive) {
				ship.StartCoroutine(mod.effect.stopThruster());
			}
		}
	}

	public void StopRightManeuverers() {
		foreach (ManeuveringCombatModule mod in rightManeuverers) {
			if (!mod.isActive) {
				continue;
			}
			if (mod.effect != null && mod.effect.isActive) {
				ship.StartCoroutine(mod.effect.stopThruster());
			}
		}
	}

	// back
	public void ManeuverBack() {
		foreach (ManeuveringCombatModule mod in backManeuverers) {
			if (!mod.isActive) {
				continue;
			}
			if (RemoveFuel(mod.GetStats().consumption * mod.transform.lossyScale.x * Time.deltaTime / 60f)) {
//				mod.rigidbody.AddForce(mod.transform.up * Time.deltaTime * mod.GetStats().thrust * 400);
				ship.rigidbody.AddForceAtPosition(mod.transform.up * mod.transform.lossyScale.x * Time.deltaTime * mod.GetStats().thrust * 400f, mod.transform.position);
				if (!mod.effect.isActive) {
					mod.effect.startThruster();
				}
			}
			else if (mod.effect != null && mod.effect.isActive) {
				ship.StartCoroutine(mod.effect.stopThruster());
			}
		}
	}

	public void StopBackManeuverers() {
		foreach (ManeuveringCombatModule mod in backManeuverers) {
			if (!mod.isActive) {
				continue;
			}
			if (mod.effect != null && mod.effect.isActive) {
				ship.StartCoroutine(mod.effect.stopThruster());
			}
		}
	}

}


