using UnityEngine;
using System;

public class PositionalManifest {
	
	public Vector3 position;
	public Quaternion rotation;
	
	public PositionalManifest() {
		
	}
	
	public PositionalManifest(MonoBehaviour behaviour) {
		this.position = behaviour.transform.localPosition;
		this.rotation = behaviour.transform.localRotation;
	}
	
	public PositionalManifest(GameObject gameObject) {
		this.position = gameObject.transform.localPosition;
		this.rotation = gameObject.transform.localRotation;
	}
	
	public PositionalManifest(Transform transform) {
		this.position = transform.localPosition;
		this.rotation = transform.localRotation;
	}
	
	public PositionalManifest(Vector3 position, Quaternion rotation) {
		this.position = position;
		this.rotation = rotation;
	}
}