using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class MapStar : Star {

	override protected void Start() {
		base.Start();
		transform.parent = constellation.transform;
		transform.localPosition = manifest.position;

		List<ParticleSystem> particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
		foreach (var ps in particleSystems) {
			ps.startSize = ps.startSize * ((float)manifest.size + 1f);
		}
		transform.localScale = transform.localScale * ((float)manifest.size + 1f);
	}

	void OnTriggerEnter(Collider other) {
		CombatModule mod = other.gameObject.GetComponent<CombatModule>();
		if (mod != null && !mod.ship.isLerping) {
			Ship ship = mod.ship;
			ship.rigidbody.velocity = Vector3.zero;
			ship.rigidbody.angularVelocity = Vector3.zero;
			ship.constellation = constellation;
			ship.star = this;
			StartCoroutine(mod.ship.LerpPosition(transform.position, 1f));
			StartCoroutine(mod.ship.LerpScale(new Vector3(.1f, .1f, .1f), 1f));
			StartCoroutine(mod.ship.LerpRotation(new Vector3(0f, 0f, 360f * 3f), 1f, () => {
				ship.transform.position = Vector3.zero;
				game.Navigate();
			}));

//			StartCoroutine(scene.cam.LerpSize(2f, 1f, () => game.ChangeScene("SolarSystem")));
		}
	}

}
