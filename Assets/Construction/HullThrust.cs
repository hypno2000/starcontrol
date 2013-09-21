using UnityEngine;
using System;
using System.Collections.Generic;

public class HullThrust {

	public List<ThrusterConstructionModule> thrusters;
	public List<ManeuveringConstructionModule> maneuverers;

	public HullThrust() {
		thrusters = new List<ThrusterConstructionModule>();
		maneuverers = new List<ManeuveringConstructionModule>();
	}

	public float GetMaxThrust() {
		float thrust = 0f;
		foreach (var mod in thrusters) {
			Debug.Log("here");
			thrust += mod.GetStats().thrust;
		}
		Debug.Log("Thrust: " + thrust);
		return thrust;
	}

}


