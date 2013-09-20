using UnityEngine;
using System.Collections.Generic;

public class ShipWeaponSystem {

	public Ship ship;
	public List<WeaponCombatModule> weapons;

	public ShipWeaponSystem(Ship ship) {
		this.ship = ship;
		weapons = new List<WeaponCombatModule>();
	}

	public void AddWeapon(WeaponCombatModule module) {
		module.system = this;
		weapons.Add(module);
		ship.modules.Add(module);
		ship.rigidbody.mass += module.stats.mass;
	}

	public void RemoveWeapon(WeaponCombatModule module) {
		weapons.Remove(module);
		ship.modules.Remove(module);
		module.system = null;
		ship.rigidbody.mass -= module.stats.mass;
		ship.CalcBounds();
	}

	public bool Fire() {
		bool fired = false;

		// with all weapons
		foreach (WeaponCombatModule mod in weapons) {
			if (!mod.isActive || mod.weapon.IsOnCooldown() || !ship.energySystem.Consume(mod.GetStats().energyConsumption)) {
				continue;
			}

			// kick back
			ship.rigidbody.AddForceAtPosition(-mod.transform.up * mod.transform.lossyScale.x * 100f * mod.GetStats().cooldown, mod.transform.position);
//			mod.rigidbody.AddForce(-mod.transform.up * 100f * mod.GetStats().cooldown);

			// fire!
			mod.weapon.Fire();
			fired = true;

		}

		return fired;

	}


}
