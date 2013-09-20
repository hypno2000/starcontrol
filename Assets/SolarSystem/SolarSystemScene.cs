using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SolarSystemScene : Scene {

	public SolarSystemPlayerShip playerShipPrefab;
	public Constellation constellationPrefab;
	public BuildCircleMesh orbitLinePrefab;
	public SolarSystemPlayerShip playerShip;
	private Ship enemyShip;
	private CrewGauge crewGauge;
	private FuelGauge fuelGauge;
	private EnergyGauge energyGauge;
	public Constellation constellation;
	public LocalStar star;
	public Planet planet;

	override protected void Awake() {
		base.Awake();
		if (game.hullManifest == null) {
			game.ChangeScene("Construction");
			return;
		}
	}

	void Start() {
		crewGauge = FindObjectOfType(typeof(CrewGauge)) as CrewGauge;
		fuelGauge = FindObjectOfType(typeof(FuelGauge)) as FuelGauge;
		energyGauge = FindObjectOfType(typeof(EnergyGauge)) as EnergyGauge;

		// player ship
		playerShip = Instantiate(playerShipPrefab) as SolarSystemPlayerShip;
	}

	void Update() {
		if (playerShip != null && playerShip.crewSystem != null) {
			crewGauge.SetCurrent(playerShip.crewSystem.GetCrewLeft());
			crewGauge.SetMax(playerShip.crewSystem.GetMaxCrew());
			fuelGauge.SetCurrent(playerShip.propulsionSystem.GetFuelLeft());
			fuelGauge.SetMax(playerShip.propulsionSystem.GetMaxFuel());
			energyGauge.SetCurrent(playerShip.energySystem.GetEnergyLeft());
			energyGauge.SetMax(playerShip.energySystem.GetMaxEnergy());
		}
	}
	
	void OnGUI() {
		GUI.BeginGroup(new Rect(10, 10, 190, Screen.height - 10));

		// button to go back to construction scene
		if (GUI.Button(new Rect(0, 0, 180, 40), "Space Station")) {
			game.ChangeScene("Construction");
		}

		// add crew members
		if (GUI.Button(new Rect(0, 45, 180, 40), "Add crew member")) {
			playerShip.crewSystem.AddPersons(1);
		}

		// remove crew members
		if (GUI.Button(new Rect(0, 90, 180, 40), "Remove crew member")) {
			playerShip.crewSystem.RemovePersons(1);
		}

		// add fuel
		if (GUI.Button(new Rect(0, 135, 180, 40), "Add fuel")) {
			playerShip.propulsionSystem.AddFuel(1);
		}

		GUI.EndGroup();

	}

	override protected Constellation Construct(ConstellationManifest conMf) {
		Constellation con;
		if (constellations.TryGetValue(conMf.position, out con)) {
			return con;
		}
		con = Instantiate(constellationPrefab) as Constellation;
		con.Manifest = conMf;
		con.transform.localPosition = Vector3.zero;

		// stars
		foreach (var starMf in conMf.stars) {
			LocalStar starPf = game.GetLocalStarPrefab(starMf.color);
			LocalStar star = Instantiate(starPf) as LocalStar;
			star.manifest = starMf;
			con.Stars.Add(star);
			star.constellation = con;
			if (game.hullManifest.star != starMf.index) {
				if (star.gameObject.activeSelf) {
					star.gameObject.SetActive(false);
				}
				continue;
			}
			if (!star.gameObject.activeSelf) {
				star.gameObject.SetActive(true);
			}

			this.star = star;
			star.transform.position = Vector3.zero;
			star.transform.parent = star.constellation.transform;
			star.transform.localPosition = Vector3.zero;

			// planets
			GameObject orbits = new GameObject("Orbits");
			orbits.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
			star.planets = new List<Planet>();
			foreach (PlanetManifest planetMf in starMf.planets) {
				GameObject orbit = new GameObject("Orbit " + (planetMf.index + 1));
				orbit.transform.parent = orbits.transform;
				orbit.transform.localScale = new Vector3(1f, 1f, 1f);
				orbit.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, planetMf.orbitPosition));
				BuildCircleMesh line = Instantiate(orbitLinePrefab) as BuildCircleMesh;
				line.transform.parent = orbit.transform;
				line.transform.localScale = new Vector3(1f, 1f, 1f);
				line.transform.localRotation = Quaternion.identity;
				line.innerRadius = planetMf.orbitDistance - 10;
				Planet planet = Instantiate(game.GetPlanetPrefab(planetMf.type)) as Planet;
				star.planets.Add(planet);
				planet.transform.parent = orbit.transform;
				planet.transform.localPosition = new Vector3(planetMf.orbitDistance, 0f, 0f);
				planet.transform.localScale = new Vector3(planetMf.size, planetMf.size, planetMf.size);
				planet.transform.localRotation = Quaternion.identity;
				planet.index = planetMf.index;
				planet.star = star;
				if (game.hullManifest.planet == planetMf.index) {
					this.planet = planet;
				}
			}
			Planet lastPlanet = star.planets[star.planets.Count - 1];

			// size
			star.transform.localScale = star.transform.localScale * ((float)starMf.size + 1f);
			List<ParticleSystem> particleSystems = new List<ParticleSystem>(star.GetComponentsInChildren<ParticleSystem>());
			foreach (var ps in particleSystems) {
				ps.startSize = ps.startSize * ((float)starMf.size + 1f);
			}

			// set camera
			float scaleFactor = lastPlanet.transform.lossyScale.x / lastPlanet.transform.localScale.x;
			Camera.main.orthographicSize = lastPlanet.transform.localPosition.x * scaleFactor * Camera.main.aspect;
	//		Camera.main.transform.position = new Vector3(
	//			Camera.main.transform.position.x,
	//			-(Camera.main.orthographicSize + 45f) * 0.484f,
	//			Camera.main.transform.position.z
	//		);

			// ship position
			if (playerShip.transform.position == Vector3.zero) {
				playerShip.transform.position = new Vector3(
					0f,
					-lastPlanet.transform.localPosition.x * scaleFactor - 6f,
					0f
				);
			}
			else if (playerShip.manifest.planet != null) {
				playerShip.transform.position = star.planets[(int)playerShip.manifest.planet].transform.position;
			}

		}

		constellations.Add(con.Manifest.position, con);
		return con;
	}

//	void Construct() {
//		int i = 0;
//
//		// constellations
//		foreach (var pair in game.universeManifest.constellations) {
//			ConstellationManifest conMf = pair.Value;
//			i++;
//			Constellation con = Instantiate(constellationPrefab) as Constellation;
//			con.manifest = conMf;
//			constellations.Add(con.manifest.position, con);
//			con.transform.localPosition = Vector3.zero;
//			if (game.hullManifest.constellation != conMf.position) {
//				if (con.gameObject.activeSelf) {
//					con.gameObject.SetActive(false);
//				}
//				continue;
//			}
//			if (!con.gameObject.activeSelf) {
//				con.gameObject.SetActive(true);
//			}
//			con.stars = new List<Star>();
//			constellation = con;
//
//			// stars
//			foreach (var starMf in conMf.stars) {
//				LocalStar starPf = game.GetLocalStarPrefab(starMf.color);
//				LocalStar star = Instantiate(starPf) as LocalStar;
//				star.manifest = starMf;
//				con.stars.Add(star);
//				star.constellation = con;
//				if (game.hullManifest.star != starMf.index) {
//					if (star.gameObject.activeSelf) {
//						star.gameObject.SetActive(false);
//					}
//				}
//				else if (!star.gameObject.activeSelf) {
//					star.gameObject.SetActive(true);
//				}
//			}
//		}
//	}
	
}
