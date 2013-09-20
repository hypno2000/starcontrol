using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public Texture2D cursorImage;
	public Material rotateMaterial;
	public Material movementMaterial;
	public Texture2D loadingScreen;
	public UniverseGenerator universeGenerator;
	private bool loading = false;

//    private int cursorWidth = 32;
//    private int cursorHeight = 32;
	
	public InventoryManifest inventoryManifest;
	public HullManifest hullManifest;
	public UniverseManifest universeManifest;

	// prefabs
	private Dictionary<ModuleType, CombatModule> combatModulePrefabs;
	private Dictionary<ModuleType, ConstructionModule> constructionModulePrefabs;
	private Dictionary<EffectLabel, Effect> effectPrefabs;
	private Dictionary<PlanetType, Planet> planetPrefabs;
	private Dictionary<StarColor, MapStar> mapStarPrefabs;
	private Dictionary<StarColor, LocalStar> localStarPrefabs;
	private Dictionary<Tuple<Race, HullClass>, FleetShip> fleetShipPrefabs;
	private Dictionary<ProjectileType, Projectile> projectilePrefabs;

	void Awake() {
		DontDestroyOnLoad(this);
		LoadModules();
		LoadEffects();
		LoadPlanets();
		LoadMapStars();
		LoadLocalStars();
		LoadFleetShips();
		LoadProjectiles();
		universeGenerator = new UniverseGenerator(1);
		universeManifest = universeGenerator.universe;
	}

	/**
	 * load module prefabs
	 */
	void LoadModules() {
		combatModulePrefabs = new Dictionary<ModuleType, CombatModule>();
		constructionModulePrefabs = new Dictionary<ModuleType, ConstructionModule>();
		var mods = Resources.LoadAll("Modules", typeof(GameObject));
		foreach (GameObject obj in mods) {
			CombatModule mod = obj.GetComponent<CombatModule>();
			if (mod != null) {
				ModuleType type = (ModuleType)mod.GetType().GetField("type").GetRawConstantValue();
				combatModulePrefabs.Add(type, mod);
				continue;
			}

			ConstructionModule constructionMod = obj.GetComponent<ConstructionModule>();
			if (constructionMod != null) {
				ModuleType type = (ModuleType)constructionMod.GetType().GetField("type").GetRawConstantValue();
				constructionModulePrefabs.Add(type, constructionMod);
				continue;
			}
		}
	}

	/**
	 * load effect prefabs
	 */
	void LoadEffects() {
		effectPrefabs = new Dictionary<EffectLabel, Effect>();
		var effs = Resources.LoadAll("Effects", typeof(GameObject));
		foreach (GameObject eff in effs) {
			Effect effect = eff.GetComponent<Effect>();
			if (effect != null) {
				EffectLabel name = (EffectLabel)effect.GetType().GetField("label").GetRawConstantValue();
				effectPrefabs.Add(name, effect);
				continue;
			}
		}
	}

	/**
	 * load planet prefabs
	 */
	void LoadPlanets() {
		planetPrefabs = new Dictionary<PlanetType, Planet>();
		var objects = Resources.LoadAll("Planets", typeof(GameObject));
		foreach (GameObject obj in objects) {
			Planet planet = obj.GetComponent<Planet>();
			if (planet != null) {
				PlanetType type = (PlanetType)planet.GetType().GetField("type").GetRawConstantValue();
				planetPrefabs.Add(type, planet);
				continue;
			}
		}
	}

	/**
	 * load local star prefabs
	 */
	void LoadLocalStars() {
		localStarPrefabs = new Dictionary<StarColor, LocalStar>();
		var objects = Resources.LoadAll("Stars/Local", typeof(GameObject));
		foreach (GameObject obj in objects) {
			LocalStar star = obj.GetComponent<LocalStar>();
			if (star != null) {
				StarColor color = (StarColor)star.GetType().GetField("color").GetRawConstantValue();
				localStarPrefabs.Add(color, star);
				continue;
			}
		}
	}

	/**
	 * load map star prefabs
	 */
	void LoadMapStars() {
		mapStarPrefabs = new Dictionary<StarColor, MapStar>();
		var objects = Resources.LoadAll("Stars/Map", typeof(GameObject));
		foreach (GameObject obj in objects) {
			MapStar star = obj.GetComponent<MapStar>();
			if (star != null) {
				StarColor color = (StarColor)star.GetType().GetField("color").GetRawConstantValue();
				mapStarPrefabs.Add(color, star);
				continue;
			}
		}
	}

	/**
	 * load Fleet ship prefabs
	 */
	void LoadFleetShips() {
		fleetShipPrefabs = new Dictionary<Tuple<Race, HullClass>, FleetShip>();
		var objects = Resources.LoadAll("Fleet", typeof(GameObject));
		foreach (GameObject obj in objects) {
			FleetShip ship = obj.GetComponent<FleetShip>();
			if (ship != null) {
				Race race = (Race)ship.GetType().GetField("race").GetRawConstantValue();
				HullClass hullClass = (HullClass)ship.GetType().GetField("hullClass").GetRawConstantValue();
				fleetShipPrefabs.Add(new Tuple<Race, HullClass>(race, hullClass), ship);
				continue;
			}
		}
	}

	/**
	 * load projectile prefabs
	 */
	void LoadProjectiles() {
		projectilePrefabs = new Dictionary<ProjectileType, Projectile>();
		var objects = Resources.LoadAll("Projectiles", typeof(GameObject));
		foreach (GameObject obj in objects) {
			Projectile projectile = obj.GetComponent<Projectile>();
			if (projectile != null) {
				projectilePrefabs.Add(projectile.type, projectile);
				continue;
			}
		}
	}

	public IEnumerator LoadScene(string scene, Action whenDone = null) {
		yield return Application.LoadLevelAsync(scene);
		if (whenDone != null) {
			whenDone();
		} else {
			ChangeScene(scene);
		}
	}

	void InvokeOnLeave() {
		foreach (MonoBehaviour item in FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[]) {
			if (item is LeaveAware) {
				((LeaveAware)item).OnLeave();
			}
		}
	}

	public void ChangeScene(string scene) {
		loading = true;
		InvokeOnLeave();
		Application.LoadLevel(scene);
	}

	public void Navigate() {
		loading = true;
		InvokeOnLeave();
		if (hullManifest.planet != null) {
			Application.LoadLevel("SolarSystem");
		}
		else if (hullManifest.star != null) {
			Application.LoadLevel("SolarSystem");
		}
		else if (hullManifest.modules.Count == 0) {
			Application.LoadLevel("Construction");
		}
		else {
			Application.LoadLevel("StarMap");
		}
	}
	
	public CombatModule GetCombatModulePrefab(ModuleType type) {
		return combatModulePrefabs[type];
	}

	public ConstructionModule GetConstructionModulePrefab(ModuleType type) {
		return constructionModulePrefabs[type];
	}

	public Effect GetEffectPrefab(EffectLabel type) {
		return effectPrefabs[type];
	}

	public Planet GetPlanetPrefab(PlanetType type) {
		return planetPrefabs[type];
	}

	public MapStar GetMapStarPrefab(StarColor color) {
		return mapStarPrefabs[color];
	}

	public LocalStar GetLocalStarPrefab(StarColor color) {
		return localStarPrefabs[color];
	}

	public FleetShip GetFleetShipPrefab(Race race, HullClass hullClass) {
		return fleetShipPrefabs[new Tuple<Race, HullClass>(race, hullClass)];
	}
	
	public FleetShip GetFleetShipPrefab(FleetShipManifest manifest) {
		return fleetShipPrefabs[new Tuple<Race, HullClass>(manifest.race, manifest.hullClass)];
	}

	public Projectile GetProjectilePrefab(ProjectileType type) {
		return projectilePrefabs[type];
	}

	void OnGUI() {
		if (loading) {
			Vector3 scale = new Vector3(Screen.width / 1024f, Screen.height / 1024f, 1f);
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
			GUI.depth = -10;
			GUI.Box(new Rect(0, 0, 1024, 1024), loadingScreen);
		}
		if (!Application.isLoadingLevel)
			loading = false;
	}

	public static Damageable FindDamageable(GameObject obj) {
		Damageable res;
		while (obj != null) {
			res = obj.GetComponent(typeof(Damageable)) as Damageable;
			if (res != null) {
				return res;
			}
			if (obj.transform.parent == null) {
				return null;
			}
			obj = obj.transform.parent.gameObject;
		}
		return null;
	}
}
