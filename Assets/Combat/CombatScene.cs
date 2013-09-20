using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CombatScene : Scene {

	public Constellation constellationPrefab;
	private List<GameObject> rootObjects;
	private Ship playerShip;
	private Ship enemyShip;
	private CrewGauge crewGauge;
	private FuelGauge fuelGauge;
	private EnergyGauge energyGauge;
	public Constellation constellation;
	public LocalStar star;
	public Planet planet;

	override protected void Awake() {
		base.Awake();
		if (game.hullManifest != null) return;
		game.ChangeScene("Construction");
		return;
	}

	void Start() {
		rootObjects = new List<GameObject>();
		foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject))) {
			if (obj.transform.parent == null) {
				rootObjects.Add(obj);
			}
		}
		playerShip = FindObjectOfType(typeof(PlayerShip)) as Ship;
		enemyShip = FindObjectOfType(typeof(AiShip)) as Ship;
		crewGauge = FindObjectOfType(typeof(CrewGauge)) as CrewGauge;
		fuelGauge = FindObjectOfType(typeof(FuelGauge)) as FuelGauge;
		energyGauge = FindObjectOfType(typeof(EnergyGauge)) as EnergyGauge;
	}

	void Update() {
		if (playerShip.crewSystem == null) return;
		crewGauge.SetCurrent(playerShip.crewSystem.GetCrewLeft());
		crewGauge.SetMax(playerShip.crewSystem.GetMaxCrew());
		fuelGauge.SetCurrent(playerShip.propulsionSystem.GetFuelLeft());
		fuelGauge.SetMax(playerShip.propulsionSystem.GetMaxFuel());
		energyGauge.SetCurrent(playerShip.energySystem.GetEnergyLeft());
		energyGauge.SetMax(playerShip.energySystem.GetMaxEnergy());
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
			enemyShip.propulsionSystem.AddFuel(1);
		}

		GUI.EndGroup();

	}

	override protected Constellation Construct(ConstellationManifest conMf) {
		Constellation con;
		if (constellations.TryGetValue(conMf.position, out con)) {
			return con;
		}
		con = Instantiate(constellationPrefab) as Constellation;
		if (con == null) {
			return null;
		}
		con.Manifest = conMf;
		con.transform.localPosition = conMf.position;
		//		foreach (var starMf in conMf.stars) {
		//			MapStar starPf = game.GetMapStarPrefab(starMf.color);
		//			MapStar star = Instantiate(starPf) as MapStar;
		//			star.manifest = starMf;
		//			star.constellation = con;
		//			con.stars.Add(star);
		//		}
		constellations.Add(con.Manifest.position, con);
		return con;
	}

}
