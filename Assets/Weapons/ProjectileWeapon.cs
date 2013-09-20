using UnityEngine;

public class ProjectileWeapon : Weapon {

	public ProjectileType projectileType;
	public float projectileLaunchSpeed;
	public float projectileAcceleration;
	public float projectileTopSpeed;
	public float projectileDuration;
	private Projectile projectilePrefab;

	override protected void Start() {
		base.Start();
		projectilePrefab = game.GetProjectilePrefab(projectileType);
	}

	override public void Fire() {
		if (IsOnCooldown()) {
			return;
		}
		base.Fire();
		Projectile projectile = Instantiate(projectilePrefab) as Projectile;
		var coll = projectile.GetComponent<CapsuleCollider>();
		projectile.transform.position = transform.position + transform.up * (coll.height / 2f + coll.radius);
		projectile.transform.rotation = transform.rotation;
		projectile.damage = damage;
		projectile.range = range;
		projectile.launchSpeed = projectileLaunchSpeed;
		projectile.acceleration = projectileAcceleration;
		projectile.topSpeed = projectileTopSpeed;
		projectile.duration = projectileDuration;
	}

}