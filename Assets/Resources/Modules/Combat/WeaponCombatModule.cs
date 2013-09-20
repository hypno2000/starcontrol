using UnityEngine;
using System.Collections;

public class WeaponCombatModule : ExternalCombatModule {
	
	public const ModuleType type = ModuleType.Weapon;

	public FirePoint firePoint;
	public ProjectileWeapon weapon;
	public float lastFireTime = 0;
	public ShipWeaponSystem system;

	override protected void Start() {
		base.Start();

		// setup weapon
//		weapon = firePoint.gameObject.AddComponent<RayWeapon>();
//		weapon.range = GetStats().range;
//		weapon.damage = GetStats().damage;
//		weapon.cooldown = GetStats().cooldown / 4f;
//		var audioSource = weapon.gameObject.AddComponent<AudioSource>();
//		audioSource.clip = audio.clip;
		var stats = GetStats();
		weapon = firePoint.gameObject.AddComponent<ProjectileWeapon>();
		weapon.range = stats.range;
		weapon.damage = stats.directDamage;
		weapon.cooldown = stats.cooldown;
		weapon.projectileType = ProjectileType.AM39Missile;
		weapon.projectileLaunchSpeed = stats.projectileLaunchSpeed;
		weapon.projectileAcceleration = stats.projectileAcceleration;
		weapon.projectileTopSpeed = stats.projectileTopSpeed;
		weapon.projectileDuration = stats.projectileDuration;
		var audioSource = weapon.gameObject.AddComponent<AudioSource>();
		audioSource.clip = audio.clip;

	}

	public WeaponCombatModule() {
		stats = new WeaponModuleStats();
	}

	public WeaponModuleStats GetStats() {
		return (WeaponModuleStats) stats;
	}

	protected override void OnDestroy() {
		if (system == null) {
			return;
		}
		system.RemoveWeapon(this);
	}

}
