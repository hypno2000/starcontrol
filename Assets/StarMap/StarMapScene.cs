using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StarMapScene : Scene {

	public MapConstellation constellationPrefab;
	[HideInInspector]
	public Ship playerShip;
	[HideInInspector]
	public StarMapCamera cam;

	private Ship enemyShip;
	private CrewGauge crewGauge;
	private FuelGauge fuelGauge;
	private EnergyGauge energyGauge;
//	public Bounds universeBounds;

	override protected void Awake() {
		base.Awake();
		if (game.hullManifest == null) {
			game.ChangeScene("Construction");
		}
		cam = Camera.main.GetComponent<StarMapCamera>();
//		universeBounds = new Bounds(Vector3.zero, Vector3.zero);
//		universeBounds.center = Vector3.zero;
	}

	override protected Constellation Construct(ConstellationManifest conMf) {
		Constellation con;
		if (constellations.TryGetValue(conMf.position, out con)) {
			return con;
		}
		con = Instantiate(constellationPrefab) as MapConstellation;
		con.Manifest = conMf;
		con.transform.localPosition = conMf.position;

		// stars
		foreach (var starMf in conMf.stars) {
			MapStar starPf = game.GetMapStarPrefab(starMf.color);
			MapStar star = Instantiate(starPf) as MapStar;
			star.manifest = starMf;
			star.constellation = con;
			con.Stars.Add(star);
//			universeBounds.Encapsulate(con.transform.position + starMf.position);
		}

		// Fleet
		foreach (var shipMf in conMf.fleet) {
			FleetShip shipPf = game.GetFleetShipPrefab(shipMf);
			FleetShip ship = Instantiate(shipPf) as FleetShip;
			ship.transform.parent = con.transform;
			ship.transform.localPosition = shipMf.position;
			ship.transform.localScale = new Vector3(3f, 3f, 3f);
			ship.manifest = shipMf;
			con.Fleet.Add(ship);
		}

		constellations.Add(con.Manifest.position, con);
		return con;
	}

	void Construct(List<ConstellationManifest> constellationManifests) {
		foreach (var conMf in constellationManifests) {
			Construct(conMf);
		}
	}

	void OnDrawGizmos() {
		if (game == null || game.universeManifest == null) {
			return;
		}

		// camera view bounds
		Bounds bounds = GetCameraBounds();
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(bounds.center, 1f);  // center sphere
		Gizmos.DrawWireCube(bounds.center, bounds.size);

		// generation bounds
		bounds.Expand(3f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(bounds.center, 1f);  // center sphere
		Gizmos.DrawWireCube(bounds.center, bounds.size);

		// constellations manifest bounds
		foreach (var pair in game.universeManifest.constellations) {
			ConstellationManifest con = pair.Value;
			if (constellations.ContainsKey(con.position)) {
				Gizmos.color = Color.green;
			}
			else {
				Gizmos.color = Color.gray;
			}
			Gizmos.DrawSphere(con.bounds.center, 1f);  // center sphere
			Gizmos.DrawWireCube(con.bounds.center, con.bounds.size);
		}

	}

	// generate universe progressively
	void Generate() {
		Bounds bounds = GetCameraBounds();
		bounds.Expand(3f);
		Construct(game.universeGenerator.GetConstellations(bounds));
	}

	void Start() {
		playerShip = FindObjectOfType(typeof(PlayerShip)) as Ship;
		crewGauge = FindObjectOfType(typeof(CrewGauge)) as CrewGauge;
		fuelGauge = FindObjectOfType(typeof(FuelGauge)) as FuelGauge;
		energyGauge = FindObjectOfType(typeof(EnergyGauge)) as EnergyGauge;
	}

	void Update() {
		if (playerShip.crewSystem != null) {
			crewGauge.SetCurrent(playerShip.crewSystem.GetCrewLeft());
			crewGauge.SetMax(playerShip.crewSystem.GetMaxCrew());
			fuelGauge.SetCurrent(playerShip.propulsionSystem.GetFuelLeft());
			fuelGauge.SetMax(playerShip.propulsionSystem.GetMaxFuel());
			energyGauge.SetCurrent(playerShip.energySystem.GetEnergyLeft());
			energyGauge.SetMax(playerShip.energySystem.GetMaxEnergy());

			Generate();
		}
	}

	void OnGUI() {
		GUI.BeginGroup(new Rect(10, 10, 380, Screen.height - 10));

		// button to go back to construction scene
		if (GUI.Button(new Rect(0, 0, 180, 40), "Space Station")) {
			game.ChangeScene("Construction");
		}

//		if (GUI.Button(new Rect(190, 0, 180, 40), "Generate more")) {
//			game.universeGenerator.Generate(50);
//			Construct();
//		}

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
}
