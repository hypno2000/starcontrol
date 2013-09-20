using UnityEngine;

public class BeamWeapon : Weapon {

	override public void Fire() {
		if (IsOnCooldown()) {
			return;
		}
		base.Fire();

		Effect effect = Instantiate(game.GetEffectPrefab(EffectLabel.BlueBeam), transform.position, transform.rotation) as Effect;
		effect.transform.parent = transform;
		audio.Play();
		effect.SetRange(range);
		effect.Fade(.5f);

		Vector3 direction = transform.TransformDirection(Vector3.up);
//		Debug.DrawRay(transform.position, direction * range, Color.red, 100f);

		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, range)) {
			Damageable obj = Game.FindDamageable(hit.collider.gameObject);
			if (obj != null) {
				effect.SetRange(hit.distance);
				obj.TakeDamage(damage);
			}
		}

	}

}