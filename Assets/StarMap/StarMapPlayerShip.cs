using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarMapPlayerShip : PlayerShip {

	override protected void Start() {
		base.Start();
		transform.rotation = game.hullManifest.rotation;
		transform.position = game.hullManifest.position;
		if (rigidbody.velocity == Vector3.zero) {
			transform.localScale = new Vector3(.1f, .1f, .1f);
			StartCoroutine(LerpPosition(transform.position + transform.up * (3f + bounds.size.y / 2), 1f));
			StartCoroutine(LerpScale(new Vector3(1f, 1f, 1f), 1f));
			StartCoroutine(LerpRotation(new Vector3(0f, 0f, transform.localRotation.eulerAngles.z - 360f * 3f), 1f));
		}
	}

}
