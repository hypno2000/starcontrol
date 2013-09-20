using UnityEngine;
using System.Collections;

public class FleetShip : SceneAware<Scene>, Damageable {

	public FleetShipManifest manifest;
	public float angleToTarget;
	public Vector3 angularVelocity;
	public bool turnLeft = false;
	public bool turnRight = false;
	public FirePoint firePoint;
	public float lastFireTime = 0;
	public GameObject target;

	override protected void Start() {
		base.Start();
		target = (FindObjectOfType(typeof(PlayerShip)) as PlayerShip).gameObject;
	}

	protected float GetAngleToTarget() {
		Quaternion initial = transform.localRotation;
		transform.LookAt(
			target.transform.position,
			-Vector3.forward
		);

		// get a "right vector" for each rotation
		Vector3 forwardA = initial * Vector2.right;
		Vector3 forwardB = transform.localRotation * Vector2.right;

		// get a numeric angle for each vector, on the X-Y plane (relative to world right)
		float angleA = Mathf.Atan2(forwardA.x, forwardA.y) * Mathf.Rad2Deg;
		float angleB = Mathf.Atan2(forwardB.x, forwardB.y) * Mathf.Rad2Deg;

		// get the signed difference in these angles
		float angle = Mathf.DeltaAngle(angleA, angleB);

		transform.localRotation = initial;
		angleToTarget = angle;
		angularVelocity = rigidbody.angularVelocity;
		return angle;
	}

	virtual protected void Update() {
		float angle = GetAngleToTarget();
		float distance = Vector3.Distance(transform.position, target.transform.position);
		float detectWindow = 140f / distance;

		// accelerate if far enough and angle and speed is ok
		if (distance > 6f && angle > -20f && angle < 20f) {
			rigidbody.AddForce(transform.up * Time.deltaTime * 300f);
		}

		turnLeft = false;
		turnRight = false;

		// breaking
		if (rigidbody.angularVelocity.z > .5f) {
			turnLeft = true;
		}
		else if (rigidbody.angularVelocity.z < -.5f) {
			turnRight = true;
		}

		// normal turning
		if (!turnLeft && !turnRight) {
			if (angle > 0f) {
				turnLeft = true;
			}
			else if (angle < 0f) {
				turnRight = true;
			}
		}

		// turn
		if (turnLeft) {
			Vector3 angles = rigidbody.transform.eulerAngles;
			rigidbody.transform.rotation = Quaternion.Euler(
				angles.x,
				angles.y,
				angles.z - 1f
			);
		}
		else if (turnRight) {
			Vector3 angles = rigidbody.transform.eulerAngles;
			rigidbody.transform.rotation = Quaternion.Euler(
				angles.x,
				angles.y,
				angles.z + 1f
			);
		}

		// fire when angle is good
		if (angle > -detectWindow && angle < detectWindow && distance <= 10f) {
			Fire();
		}

	}

	public bool IsOnCooldown() {
		return GetCooldownRemaining() != 0;
	}

	public float GetCooldownRemaining() {
		if (lastFireTime == 0) {
			return 0;
		}
		return Mathf.Max(0f, (lastFireTime + 1f) - Time.time);
	}

	public void Fire() {
		if (IsOnCooldown()) {
			return;
		}
		lastFireTime = Time.time;

		Effect effect = Instantiate(game.GetEffectPrefab(EffectLabel.HolyFire), firePoint.transform.position, transform.rotation) as Effect;
		effect.transform.parent = transform;
		audio.Play();

		Vector3 direction = transform.TransformDirection(Vector3.up);
//		Debug.DrawRay(firePoint.transform.position, direction * GetStats().range, Color.red, 100f);

		RaycastHit hit;
		if (Physics.Raycast(firePoint.transform.position, direction, out hit, 10f)) {
			Damageable obj = Game.FindDamageable(hit.collider.gameObject);
			if (obj != null) {
				obj.TakeDamage(1);
			}
		}
	}

	public static CombatModule FindModule(GameObject obj) {
		CombatModule mod;
		while (obj != null) {
			mod = obj.GetComponent<CombatModule>();
			if (mod != null) {
				return mod;
			}
			if (obj.transform.parent == null) {
				return null;
			}
			obj = obj.transform.parent.gameObject;
		}
		return null;
	}

	public float TakeDamage(float damage) {
		Die();
		return damage;
	}

	public void Die() {

		// get random explosion prefab
		Effect prefab = game.GetEffectPrefab(Module.explosions[Random.Range(1, 10)]);

		// instantiate the effect
		Effect effect = Instantiate(prefab) as Effect;
		effect.transform.position = transform.position;
		var sprite = effect.GetComponent<HTSpriteSheet>();
		sprite.sizeStart = new Vector3(15f, 15f, 0f);
		sprite.sizeEnd = new Vector3(15f, 15f, 0f);

		// completely destroy the module after some time
		Destroy(gameObject);
	}
}
