using UnityEngine;
using System.Collections;

public class Projectile : SceneAware<Scene> {

	public ProjectileType type;
	public float damage;
	public float range;
	public float launchSpeed;
	public float acceleration;
	public float topSpeed;
	public float duration;
	private Vector3 startPosition;
	private float startTime;

	override protected void Start() {
		base.Start();
		rigidbody.velocity = transform.up * launchSpeed;
		startPosition = transform.position;
		startTime = Time.time;
	}

	void OnTriggerEnter(Collider collider) {
		Damageable obj = Game.FindDamageable(collider.gameObject);
		if (obj != null) {
			obj.TakeDamage(damage);
			Detonate();
		}
	}

	void Update() {

		// speed
		if (rigidbody.velocity.magnitude < topSpeed) {
			rigidbody.velocity = rigidbody.velocity + transform.up * acceleration * Time.deltaTime;
		}
		
		// distance check
		if (Vector3.Distance(startPosition, transform.position) >= range) {
			Detonate();
			return;
		}

		// duration check
		if (Time.time - startTime >= duration) {
			Detonate();
		}

	}

	void Detonate() {
		Debug.Log("Booom!");
		Destroy(gameObject);
	}

}
