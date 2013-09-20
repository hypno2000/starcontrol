using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Ship : SceneAware<Scene> {

	// location
	public Constellation constellation;
	public Star star;
	public Planet planet;

	// parts
	[HideInInspector]
	public List<CombatModule> modules;
	[HideInInspector]
	public ShipPropulsionSystem propulsionSystem;
	[HideInInspector]
	public ShipEnergySystem energySystem;
	[HideInInspector]
	public ShipWeaponSystem weaponSystem;
	[HideInInspector]
	public ShipCrewSystem crewSystem;
	[HideInInspector]
	public Bounds bounds;
	[HideInInspector]
	public float maxLength;
	[HideInInspector]
	public bool inView;
	[HideInInspector]
	public HullManifest manifest;

	virtual protected void Awake() {
		bounds = new Bounds(Vector3.zero, Vector3.zero);
	}

	override protected void Start() {
		base.Start();
		if (game.hullManifest == null) {
			game.hullManifest = new HullManifest();
			game.hullManifest.modules.Add(new ExternalModuleManifest(Vector3.zero, new Quaternion(0f, 0f, 0f, 0f)));
		}
		propulsionSystem = new ShipPropulsionSystem(this);
		energySystem = new ShipEnergySystem(this);
		weaponSystem = new ShipWeaponSystem(this);
		crewSystem = new ShipCrewSystem(this);
		Construct(game.hullManifest);
	}

	public void CalcBounds() {

		// find a center for bounds
		Vector3 center = Vector3.zero;
		foreach (var mod in modules) {
		    center += mod.collider.bounds.center;
		}
		center /= modules.Count; // center is average center of modules

		// calculate the bounds
		bounds = new Bounds(center, Vector3.zero);
		foreach (var mod in modules) {
			bounds.Encapsulate(mod.collider.bounds);
		}
		maxLength = Mathf.Max(bounds.size.x, bounds.size.y);
	}

	virtual protected void OnEnterView() {

	}

	virtual protected void OnExitView() {

	}

	virtual protected void Update() {

		// generate energy
		if (energySystem != null) {
			energySystem.Generate(Time.deltaTime);
		}

		// enter/exit to view callbacks
		bounds.center = transform.position;
		bool nowInView = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), bounds);
		if (inView) {
			if (!nowInView) {
				inView = false;
				OnExitView();
			}
		}
		else {
			if (nowInView) {
				inView = true;
				OnEnterView();
			}
		}
	}

	/**
	 * construct ship based on manifest
	 */
	virtual protected void Construct(HullManifest manifest) {
		this.manifest = manifest;
		rigidbody.velocity = manifest.velocity;
		rigidbody.angularVelocity = manifest.angularVelocity;
		transform.position = manifest.position;
		transform.localRotation = manifest.rotation;

		// location
		if (manifest.constellation != null) {
			constellation = scene.GetConstellation((Vector3)manifest.constellation);
			if (manifest.star != null && constellation.Stars.Count > 0) {
				star = constellation.Stars[(int)manifest.star];
				if (manifest.planet != null && star.planets.Count > 0) {
					planet = star.planets[(int)manifest.planet];
				}
			}
		}

		// parts
		foreach (ExternalModuleManifest extModManifest in manifest.modules) {
			ExternalCombatModule extMod = Instantiate(game.GetCombatModulePrefab(extModManifest.type)) as ExternalCombatModule;
			extMod.transform.parent = transform;
			extMod.transform.localPosition = extModManifest.position;
			extMod.transform.localRotation = extModManifest.rotation;
			extMod.stats = extModManifest.stats;
			extMod.hitPointsLeft = extModManifest.hitPointsLeft;
			extMod.ship = this;
			if (extMod is ThrusterCombatModule) {
				propulsionSystem.AddThruster((ThrusterCombatModule)extMod);
			}
			else if (extMod is ManeuveringCombatModule) {
				propulsionSystem.AddManeuverer((ManeuveringCombatModule)extMod);
			}
			else if (extMod is WeaponCombatModule) {
				weaponSystem.AddWeapon((WeaponCombatModule)extMod);
			}
			else if (extMod is HullCombatModule) {
				modules.Add(extMod);

				// internal module mounted on top of hull module
				InternalModuleManifest intModManifest = extModManifest.internalModule;
				if (intModManifest != null) {
					InternalCombatModule intMod = Instantiate(game.GetCombatModulePrefab(intModManifest.type)) as InternalCombatModule;
					intMod.transform.parent = transform;
					intMod.transform.localPosition = intModManifest.position;
					intMod.transform.localRotation = intModManifest.rotation;
					intMod.stats = intModManifest.stats;
					intMod.hitPointsLeft = intModManifest.hitPointsLeft;
					intMod.SetContents(intModManifest.contents);
					intMod.ship = this;
					((HullCombatModule)extMod).internalModule = intMod;
					if (intMod is CrewCombatModule) {
						crewSystem.AddCrewPod((CrewCombatModule)intMod);
					}
					else if (intMod is BatteryCombatModule) {
						energySystem.AddBattery((BatteryCombatModule)intMod);
					}
					else if (intMod is PowerCombatModule) {
						energySystem.AddPowerPlant((PowerCombatModule)intMod);
					}
					else if (intMod is FuelCombatModule) {
						propulsionSystem.AddFuelTank((FuelCombatModule)intMod);
					}
				}
			}
		}
		CalcBounds();
	}

	/**
	 * Save state to manifest
	 */
	public HullManifest CreateManifest() {
		HullManifest manifest = new HullManifest(this);
		manifest.velocity = rigidbody.velocity;
		manifest.angularVelocity = rigidbody.angularVelocity;

		// location
		if (constellation != null) {
			manifest.constellation = constellation.Manifest.position;
			if (star != null) {
				manifest.star = star.manifest.index;
				if (planet != null) {
					manifest.planet = planet.index;
				}
			}
		}

		// ship modules
		foreach (var extMod in modules) {
			if (extMod is InternalCombatModule || extMod.hitPointsLeft == 0) {
				continue;
			}
			ExternalModuleManifest extModManifest = new ExternalModuleManifest(
				extMod.transform.localPosition,
				extMod.transform.localRotation
			);
			extModManifest.type = (ModuleType) extMod.GetType().GetField("type").GetRawConstantValue();
			extModManifest.stats = extMod.stats;
			extModManifest.hitPointsLeft = extMod.hitPointsLeft;

			// internal module
			manifest.modules.Add(extModManifest);
			if (extMod is HullCombatModule) {
				InternalCombatModule intMod = ((HullCombatModule)extMod).internalModule;
				if (intMod != null && intMod.hitPointsLeft > 0) {
					InternalModuleManifest intModManifest = new InternalModuleManifest(extMod);
					intModManifest.type = (ModuleType) intMod.GetType().GetField("type").GetRawConstantValue();
					intModManifest.stats = intMod.stats;
					intModManifest.hitPointsLeft = intMod.hitPointsLeft;
					intModManifest.contents = intMod.GetContents();
					extModManifest.internalModule = intModManifest;
				}
			}
		}
		this.manifest = manifest;
		return manifest;
	}

	public void LookAt(Vector3 direction) {
		Quaternion initial = transform.localRotation;
		transform.LookAt(
			direction,
			new Vector3(0f, 0f, -1f)
		);
		transform.localRotation = new Quaternion(initial.x, initial.y, transform.localRotation.z, transform.localRotation.w);
	}

}
