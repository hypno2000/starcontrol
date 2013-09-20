using UnityEngine;
using System.Collections;

public class PlanetGravity : MonoBehaviour {

	void OnTriggerStay(Collider other) {
		if (!other.attachedRigidbody.constantForce) {
			return;
		}
	 
		Vector3 direction = -(other.attachedRigidbody.transform.position - transform.position);
	 
		if (other.attachedRigidbody) {
			other.attachedRigidbody.constantForce.force = direction;
		}
	}

	void OnTriggerExit(Collider other) {
		if (!other.attachedRigidbody.constantForce) {
			return;
		}
		other.attachedRigidbody.constantForce.force = Vector3.zero;
	}

}
