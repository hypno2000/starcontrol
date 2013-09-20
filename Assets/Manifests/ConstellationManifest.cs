using UnityEngine;
using System.Collections.Generic;

public class ConstellationManifest : PositionalManifest {

	public int index;
	public Bounds bounds;
	public List<StarManifest> stars;
	public List<FleetShipManifest> fleet;

}