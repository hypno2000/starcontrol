using UnityEngine;
using System;

public class WeaponModuleStats : ExternalModuleStats {

	public float cooldown = .1f;
	public int range = 100;
	public int directDamage = 3;
	public int splashDamage = 3;
	public int splashRadius = 3;
	public int energyConsumption = 1; // per shot
	public int projectileLaunchSpeed = 60;
	public int projectileAcceleration = 0;
	public int projectileTopSpeed = 60;
	public int projectileDuration = 5;

}
