using UnityEngine;
using System.Collections.Generic;

abstract public class Star : SceneAware<Scene> {

	public StarManifest manifest;
	public Constellation constellation;
	public List<Planet> planets;

	public StarColor GetStarColor() {
		return (StarColor) GetType().GetField("color").GetRawConstantValue();
	}

}