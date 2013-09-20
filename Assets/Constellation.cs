using UnityEngine;
using System.Collections.Generic;

public class Constellation : SceneAware<Scene> {

	public int Index;
	public ConstellationManifest Manifest;
	public List<Star> Stars;
	public List<FleetShip> Fleet;

}