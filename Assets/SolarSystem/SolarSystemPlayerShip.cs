using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarSystemPlayerShip : PlayerShip {

	override protected void OnExitView() {
		if (isLerping) {
			return;
		}
		if (constellation == null || this.star == null) {
			return;
		}
		Vector3 direction = rigidbody.velocity.normalized;
		if (planet == null) {
	//		transform.position = constellation.manifest.position + star.manifest.position + direction * (3f + bounds.size.y / 2);
			transform.position = constellation.Manifest.position + star.manifest.position;
			LookAt(transform.position + direction);
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			star = null;
			game.Navigate();
		}
		else {
			StartCoroutine(planet.LerpLeave(30f, .7f, () => Debug.Log("Tere")));
			planet = null;
		}
	}

}
