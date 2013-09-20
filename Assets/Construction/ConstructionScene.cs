using UnityEngine;
using System;
using System.Collections;

public class ConstructionScene : Scene {

	[HideInInspector]
	public HullManifest hullManifest;
	[HideInInspector]
	public InventoryManifest inventoryManifest;

	private Hull hull;
	private Inventory inventory;
	private CrewGauge crewGauge;
	private FuelGauge fuelGauge;
	private EnergyGauge energyGauge;

	override protected void Awake() {
		base.Awake();
		hullManifest = game.hullManifest;
		inventoryManifest = game.inventoryManifest;
	}

	protected void Start() {

		hull = FindObjectOfType(typeof(Hull)) as Hull;
		inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
		crewGauge = FindObjectOfType(typeof(CrewGauge)) as CrewGauge;
		fuelGauge = FindObjectOfType(typeof(FuelGauge)) as FuelGauge;
		energyGauge = FindObjectOfType(typeof(EnergyGauge)) as EnergyGauge;
	}

	void OnDeserialized() {
		hull.Reset();
		inventory.Reset();
	}

	void Update() {
		if (hull.crew != null) {
			crewGauge.SetCurrent(hull.crew.GetCrewLeft());
			crewGauge.SetMax(hull.crew.GetMaxCrew());
			fuelGauge.SetCurrent(hull.fuel.GetFuelLeft());
			fuelGauge.SetMax(hull.fuel.GetMaxFuel());
			energyGauge.SetCurrent(hull.energy.GetEnergyLeft());
			energyGauge.SetMax(hull.energy.GetMaxEnergy());
		}
	}

	void OnGUI() {
		GUI.BeginGroup(new Rect(10, 10, Screen.width - 20, 40));
		if (GUI.Button(new Rect(0, 0, 180, 40), "Combat")) {
			game.ChangeScene("Combat");
		}
		if (GUI.Button(new Rect(190, 0, 180, 40), "Navigate")) {
			game.Navigate();
		}
		GUI.EndGroup();

		GUILayout.BeginArea(new Rect(10, 10, 250, Screen.height - 20));
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Clear")) {
			hullManifest = null;
			inventoryManifest = null;
			hull.Reset();
			inventory.Reset();
		}
		if (GUILayout.Button("Reset")) {
			hull.Reset();
			inventory.Reset();
		}
		if (GUILayout.Button("Save")) {
			// Save the game with a prefix of Game
			var t = DateTime.Now;
			hull.Save();
			inventory.Save();
			LevelSerializer.SaveGame("Game");
			Radical.CommitLog();
			Debug.Log(string.Format("{0:0.000}", (DateTime.Now - t).TotalSeconds));
		}

		// Check to see if there is resume info
		if (LevelSerializer.CanResume) {
			if (GUILayout.Button("Resume")) {
				LevelSerializer.Resume();
			}
		}

		if (LevelSerializer.SavedGames.Count > 0) {
			GUILayout.Label("Available saved games");
			// Look for saved games under the given player name
			foreach (var g in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) {
				if (GUILayout.Button(g.Caption)) {
					g.Load();
				}

			}
		}

		if (GUILayout.Button("Delete All")) {
			PlayerPrefs.DeleteAll();
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();

	}

	override protected Constellation Construct(ConstellationManifest conMf) {
		return null;
	}

}
