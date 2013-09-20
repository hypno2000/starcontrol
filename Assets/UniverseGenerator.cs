using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UniverseGenerator {

	// planet settings
	public float firstOrbit = 350f;
	public float orbitGap = 200f;
	public float orbitLineWidth = 20f;
	public float minPlanetSize = 15f;
	public float maxPlanetSize = 45f;

	// star settings
	public float minStarGap = 5f;
	public float maxStarGap = 120f;
	public int minPlanets = 3;
	public int maxPlanets = 12;
	public int minStars = 2;
	public int maxStars = 10;
	public float constellationSize = 100f;

	public UniverseManifest universe {get; private set;}
	private int currentPlanetCount;
	private int currentStarCount;
	private int maxStarColor;
	private int maxPlanetType;
	private int maxRace;
	private int maxHullClass;
	private int dist;
	private int randomSeed;

	public UniverseGenerator(int seed) {
		randomSeed = seed;
		currentStarCount = 0;
		maxStarColor = Enum.GetValues(typeof(StarColor)).Length;
		maxPlanetType = Enum.GetValues(typeof(PlanetType)).Length;
		maxRace = Enum.GetValues(typeof(Race)).Length;
		maxHullClass = Enum.GetValues(typeof(HullClass)).Length;

		universe = new UniverseManifest();
		universe.constellations = new Dictionary<Vector3, ConstellationManifest>();
		universe.bounds = new Bounds(Vector3.zero, Vector3.zero);
	}

	public List<ConstellationManifest> GetConstellations(Bounds bounds) {
		Vector3[] corners = {
			new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, 0f),
			new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, 0f),
			new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, 0f),
			new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, 0f)
		};
		return GetConstellations(corners);
	}

	public List<ConstellationManifest> GetConstellations(Vector3[] positions) {
		List<ConstellationManifest> consts = new List<ConstellationManifest>();
		foreach (var pos in positions) {
			consts.Add(GetConstellation(pos));
		}
		return consts;
	}

	public ConstellationManifest GetConstellation(Vector3 pos) {
		Vector3 normalizedPos = new Vector3(
			Mathf.Round(pos.x / constellationSize) * constellationSize,
			Mathf.Round(pos.y / constellationSize) * constellationSize
		);
		ConstellationManifest constellation;
		if (universe.constellations.TryGetValue(normalizedPos, out constellation)) {
			return constellation;
		}
		constellation = GenerateConstellation(normalizedPos);
		universe.constellations.Add(constellation.position, constellation);
		return constellation;
	}

	public ConstellationManifest GenerateConstellation(Vector3 normalizedPos) {
		ConstellationManifest constellation = new ConstellationManifest();

		// position
		constellation.position = normalizedPos;
		constellation.bounds = new Bounds(constellation.position, new Vector3(constellationSize, constellationSize, 1f));

		// set seed
		int origSeed = UnityEngine.Random.seed;
		UnityEngine.Random.seed =
			randomSeed +
			(int)(constellation.position.x / constellationSize) +
			(int)(constellation.position.y / constellationSize) * 100000;

		// stars
		constellation.stars = new List<StarManifest>();
		int starCount = UnityEngine.Random.Range(minStars, maxStars);
		for (int i = 0 ; i < starCount ; i++) {
			StarManifest star = GenerateStar();
			do {
				star.position = new Vector3(
					UnityEngine.Random.Range(-constellationSize / 2f + minStarGap, constellationSize / 2f - minStarGap),
					UnityEngine.Random.Range(-constellationSize / 2f + minStarGap, constellationSize / 2f - minStarGap)
				);
			}
			while (!StarRequirements(constellation, star));
			star.index = i;
			constellation.stars.Add(star);
		}

		// Fleet
		constellation.fleet = new List<FleetShipManifest>();
		int fleetShipCount = UnityEngine.Random.Range(1, 4);
		for (int i = 0 ; i < fleetShipCount ; i++) {
			constellation.fleet.Add(GenerateFleetShip());
		}

		// restore previous seed
		UnityEngine.Random.seed = origSeed;

		return constellation;
	}

	private bool StarRequirements(ConstellationManifest constellation, StarManifest star) {
		foreach (var st in constellation.stars) {
			float dist = Vector3.Distance(st.position, star.position);
			if (minStarGap > dist) {
				return false;
			}
		}
		return true;
	}

	public StarManifest GenerateStar() {
		StarManifest star = new StarManifest();
		star.planets = new List<PlanetManifest>();
		star.color = (StarColor)UnityEngine.Random.Range(0, maxStarColor - 1);
		int starCount = UnityEngine.Random.Range(minStars, maxStars);
		for (int i = 0 ; i < starCount ; i++) {
			star.planets.Add(GeneratePlanet(i));
		}

		// star is size based on nr of planets
		if (star.planets.Count > 8) {
			star.size = StarSize.Supergiant;
		}
		else if (star.planets.Count > 5) {
			star.size = StarSize.Giant;
		}
		else {
			star.size = StarSize.Dwarf;
		}

		currentStarCount++;
		return star;
	}

	public PlanetManifest GeneratePlanet(int orbitIndex) {
		PlanetManifest planet = new PlanetManifest();
		planet.index = orbitIndex;
		planet.size = UnityEngine.Random.Range(minPlanetSize, maxPlanetSize);
		planet.type = (PlanetType)UnityEngine.Random.Range(0, maxPlanetType);
		planet.orbitDistance = firstOrbit + orbitIndex * orbitGap;
		planet.orbitPosition = UnityEngine.Random.Range(0f, 360f);
		currentPlanetCount++;
		return planet;
	}

	public FleetShipManifest GenerateFleetShip() {
		FleetShipManifest ship = new FleetShipManifest();
		ship.race = (Race) UnityEngine.Random.Range(0, maxRace - 1);
		ship.hullClass = (HullClass) UnityEngine.Random.Range(0, maxHullClass - 1);
		ship.position = new Vector3(
			UnityEngine.Random.Range(-constellationSize / 2f + minStarGap, constellationSize / 2f - minStarGap),
			UnityEngine.Random.Range(-constellationSize / 2f + minStarGap, constellationSize / 2f - minStarGap)
		);
		return ship;
	}

}
