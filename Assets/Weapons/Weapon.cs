using UnityEngine;

abstract public class Weapon : SceneAware<Scene> {

	public float cooldown;
	public float lastFireTime = 0;
	public float range;
	public float damage;

	virtual public void Fire() {
		lastFireTime = Time.time;
	}

	public bool IsOnCooldown() {
		return GetCooldownRemaining() != 0;
	}

	public float GetCooldownRemaining() {
		if (lastFireTime == 0) {
			return 0;
		}
		return Mathf.Max(0f, (lastFireTime + cooldown) - Time.time);
	}

}