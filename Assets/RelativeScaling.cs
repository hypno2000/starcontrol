using UnityEngine;
using System;

public class RelativeScaling : MonoBehaviour {

	public Vector3 direction;
	[HideInInspector]
	public float max = 10f;
	[HideInInspector]
	public float current = 2f;
	private Vector3 originalScale;

	void Start() {
		originalScale = transform.localScale;
	}

	void Update() {
		float val;
		if (max == 0f) {
			val = 0f;
		}
		else {
			val = current / max;
		}
		Vector3 newScale = new Vector3();
		if (direction.x == 1) {
			newScale.x = originalScale.x * val;
		}
		else {
			newScale.x = originalScale.x;
		};
		if (direction.y == 1) {
			newScale.y = originalScale.y * val;
		}
		else {
			newScale.y = originalScale.y;
		};
		if (direction.z == 1) {
			newScale.z = originalScale.z * val;
		}
		else {
			newScale.z = originalScale.z;
		}
		transform.localScale = newScale;
	}

}


