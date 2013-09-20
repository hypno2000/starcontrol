using UnityEngine;
using System;
using System.Collections.Generic;

public class HullManifest : PositionalManifest {

	public Vector3 velocity;
	public Vector3 angularVelocity;
	public List<ExternalModuleManifest> modules = new List<ExternalModuleManifest>();
	public int seed = 1;
	public Vector3? constellation = null;
	public int? star = null;
	public int? planet = null;

	public HullManifest() {
		
	}

	public HullManifest(MonoBehaviour behaviour) : base(behaviour) {
		
	}

}

