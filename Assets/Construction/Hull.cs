using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Hull : SceneAware<ConstructionScene>, LeaveAware {

	public HullSlot slotPrefab;
	[HideInInspector]
	public Dictionary<Vector3, HullSlot> slots;
	[HideInInspector]
	public Dictionary<Vector3, HullConstructionModule> hullModules;
	private Inventory inventory;
	[HideInInspector]
	public HullCrew crew;
	[HideInInspector]
	public HullFuel fuel;
	[HideInInspector]
	public HullEnergy energy;
	[HideInInspector]
	public HullThrust thrust;
	[HideInInspector]
	public Bounds bounds;
	[HideInInspector]
	public float maxLength;
	[HideInInspector]
	public HullManifest manifest;

	override protected void Start() {
		base.Start();
		inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
		crew = new HullCrew();
		fuel = new HullFuel();
		energy = new HullEnergy();
		thrust = new HullThrust();

		Reset();
	}

	void Update() {

		// generate energy
		if (energy != null) {
			energy.Generate(Time.deltaTime);
		}
	}

	public void CalcBounds() {

		// find a center for bounds
		Vector3 center = Vector3.zero;
		foreach (var pair in hullModules) {
		    center += pair.Value.collider.bounds.center;
		}
		center /= hullModules.Count; // center is average center of modules

		// calculate the bounds
		bounds = new Bounds(center, Vector3.zero);
		foreach (var pair in hullModules) {
			bounds.Encapsulate(pair.Value.collider.bounds);
		}
		maxLength = Mathf.Max(bounds.size.x, bounds.size.y);

		float newSize = Mathf.Max(6f, maxLength + 4f);
		if (newSize != Camera.main.orthographicSize) {
//			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			Camera.main.orthographicSize = Mathf.Max(6f, maxLength + 4f);
//			transform.position -= mousePos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}


	}

	/**
	 * construct hull based on manifest
	 */
	void Construct(HullManifest manifest) {
		this.manifest = manifest;
		foreach (ExternalModuleManifest extModManifest in manifest.modules) {
			HullSlot slot;
			if (!slots.TryGetValue(extModManifest.position, out slot)) {
				slot = Instantiate(slotPrefab) as HullSlot;
				slot.transform.parent = transform;
				slot.transform.localPosition = extModManifest.position;
				slots.Add(extModManifest.position, slot);
			}
			ExternalConstructionModule extMod = Instantiate(game.GetConstructionModulePrefab(extModManifest.type)) as ExternalConstructionModule;
			extMod.transform.localRotation = extModManifest.rotation;
			extMod.stats = extModManifest.stats;
			extMod.hitPointsLeft = extModManifest.hitPointsLeft;
			Equip(extMod, slot);
			if (extMod is HullConstructionModule) {
				if (extModManifest.internalModule != null) {
					InternalConstructionModule intMod = Instantiate(game.GetConstructionModulePrefab(extModManifest.internalModule.type)) as InternalConstructionModule;
					intMod.stats = extModManifest.internalModule.stats;
					intMod.hitPointsLeft = extModManifest.internalModule.hitPointsLeft;
					intMod.SetContents(extModManifest.internalModule.contents);
					Equip(intMod, (HullConstructionModule)extMod);
				}
			}

		}
	}

	/**
	 * Save state to manifest
	 */
	public HullManifest CreateManifest() {
		HullManifest manifest = new HullManifest();
		manifest.position = this.manifest.position;
		manifest.rotation = this.manifest.rotation;
		manifest.velocity = this.manifest.velocity;
		manifest.angularVelocity = this.manifest.angularVelocity;
		manifest.constellation = this.manifest.constellation;
		manifest.star = this.manifest.star;
		manifest.planet = this.manifest.planet;
		foreach (KeyValuePair<Vector3, HullSlot> pair in slots) {

			// external module
			ExternalConstructionModule extMod = pair.Value.module;
			if (extMod == null) {
				continue;
			}
			ExternalModuleManifest extModManifest = new ExternalModuleManifest(
				extMod.hullSlot.transform.localPosition,
				extMod.transform.localRotation
			);
			extModManifest.type = (ModuleType) extMod.GetType().GetField("type").GetRawConstantValue();
			extModManifest.stats = extMod.stats;
			extModManifest.hitPointsLeft = extMod.hitPointsLeft;

			// internal module
			manifest.modules.Add(extModManifest);
			if (extMod is HullConstructionModule) {
				InternalConstructionModule intMod = ((HullConstructionModule)extMod).internalModule;
				if (intMod != null) {
					InternalModuleManifest intModManifest = new InternalModuleManifest(extMod.hullSlot);
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

	/**
	 * Wipe out everything
	 */
	public void Clear() {
		// delete current stuff
		if (slots != null) {
			foreach (var pair in slots) {
				var slot = pair.Value;
				if (slot.module != null) {
					ExternalConstructionModule extMod = slot.module;
					if (slot.module is HullConstructionModule) {
						InternalConstructionModule intMod = ((HullConstructionModule)extMod).internalModule;
						if (intMod != null) {
							Unequip(intMod);
							Destroy(intMod.gameObject);
						}
					}
					Unequip(extMod);
					Destroy(extMod.gameObject);
				}
				Destroy(slot.gameObject);
			}
		}
		slots = new Dictionary<Vector3, HullSlot>();
		hullModules = new Dictionary<Vector3, HullConstructionModule>();
	}

	/**
	 * Reset to last loaded state
	 */
	public void Reset() {
		Clear();

		// no manifest, use scene
		if (scene.hullManifest == null) {
			HullSlot slot = Instantiate(slotPrefab) as HullSlot;
			slot.transform.parent = transform;
			slot.transform.localPosition = Vector3.zero;
			slots.Add(Vector3.zero, slot);
			manifest = new HullManifest();
			manifest.position = Vector3.zero;
			manifest.rotation = Quaternion.identity;
			manifest.velocity = Vector3.zero;
			manifest.angularVelocity = Vector3.zero;
		}
		
		// manifest exists, construct hull based on manifest
		else {
			Construct(scene.hullManifest);
		}
	}

	public void Save() {
		game.hullManifest = scene.hullManifest = CreateManifest();
	}

	public void OnLeave() {
		Save();
	}

	public void Equip(ExternalConstructionModule module, HullSlot slot) {
		if (module.hullSlot == slot) {
			module.transform.localPosition = Vector2.zero;
			return;
		}
		if (!slot.available) {
			return;
		}
		if (module.hullSlot != null) {
			Unequip(module);
		}
		slot.ActivateAdjacents();
		module.transform.parent = slot.transform;
		module.transform.localPosition = Vector2.zero;
		slot.module = module;
		module.hullSlot = slot;
		slot.Deactivate();
		inventory.RemoveModule(module);
		if (module is HullConstructionModule) {
			hullModules.Add(module.transform.position, (HullConstructionModule)module);
		}
		module.OnEquip();
		CalcBounds();
	}

	public void Equip(InternalConstructionModule module, HullConstructionModule hullModule) {
		if (module.hullModule == hullModule) {
			module.transform.localPosition = Vector2.zero;
			return;
		}
		if (!hullModule.available) {
			return;
		}
		if (module.hullModule != null) {
			Unequip(module);
		}
		module.transform.parent = hullModule.transform;
		module.transform.localPosition = Vector2.zero;
		hullModule.internalModule = module;
		module.hullModule = hullModule;
		hullModule.Deactivate();
		inventory.RemoveModule(module);
		module.OnEquip();
		if (module is BatteryConstructionModule) {
			energy.batteries.Add((BatteryConstructionModule)module);
		}
//		else if (module is ThrusterConstructionModule) {
//			thrust.thrusters.Add((ThrusterConstructionModule)module);
//		}
//		else if (module is ManeuveringConstructionModule) {
//			thrust.maneuverers.Add((ManeuveringConstructionModule)module);
//		}
		else if (module is PowerConstructionModule) {
			energy.powerPlants.Add((PowerConstructionModule)module);
		}
		else if (module is FuelConstructionModule) {
			fuel.fuelTanks.Add((FuelConstructionModule)module);
		}
		else if (module is CrewConstructionModule) {
			crew.crewPods.Add((CrewConstructionModule)module);
		}
	}

	public void Unequip(ExternalConstructionModule module) {
		if (module.hullSlot == null) {
			return;
		}
		if (module is HullConstructionModule) {
			if (((HullConstructionModule)module).internalModule != null) {
				return;
			}
			hullModules.Remove(module.hullSlot.transform.position);
		}
		HullSlot slot = module.hullSlot;
		slot.module = null;
		module.hullSlot = null;
		slot.Activate();
		slot.DeactivateAdjacents();
		module.OnUnequip();
		CalcBounds();
	}

	public void Unequip(InternalConstructionModule module) {
		if (module.hullModule == null) {
			return;
		}
		if (module is BatteryConstructionModule) {
			energy.batteries.Remove((BatteryConstructionModule)module);
		}
		else if (module is PowerConstructionModule) {
			energy.powerPlants.Remove((PowerConstructionModule)module);
		}
		else if (module is FuelConstructionModule) {
			fuel.fuelTanks.Remove((FuelConstructionModule)module);
		}
		else if (module is CrewConstructionModule) {
			crew.crewPods.Remove((CrewConstructionModule)module);
		}
		HullConstructionModule hullModule = module.hullModule;
		hullModule.internalModule = null;
		module.hullModule = null;
		hullModule.Activate();
		module.OnUnequip();
	}

	public void Unequip(ConstructionModule module) {
		if (module is InternalConstructionModule) {
			Unequip((InternalConstructionModule)module);
		}
		else if (module is ExternalConstructionModule) {
			Unequip((ExternalConstructionModule)module);
		}
	}

	public void Unequip(InternalConstructionModule module, HullConstructionModule hullModule) {
		if (module.hullModule != hullModule) {
			return;
		}
		Unequip(module);
	}

	public void Unequip(ExternalConstructionModule module, HullSlot slot) {
		if (module.hullSlot != slot) {
			return;
		}
		Unequip(module);
	}
	
	public float GetTotalMass() {
		var mass = 0f;
		foreach (var slot in slots.Values) {
			if (slot.module) {
				mass += slot.module.stats.mass;
			}
		}
		return mass;
	}
	
	public float GetFuelConsumption() {
		var res = 0f;
		foreach (var slot in slots.Values) {
			if (slot.module is ThrusterConstructionModule) {
				res += ((ThrusterModuleStats)slot.module.stats).consumption;
			}
			else if (slot.module is ManeuveringConstructionModule) {
				res += ((ManeuveringModuleStats)slot.module.stats).consumption;
			}
		}
		return res;
	}

	public float GetEnergyConsumption() {
		var res = 0f;
		foreach (var slot in slots.Values) {
			if (slot.module is WeaponConstructionModule) {
				res += ((WeaponModuleStats)slot.module.stats).energyConsumption;
			}
		}
		return res;
	}

	public int GetAccelerationScore() {
		// todo
		//GetTotalMass();
		//thrust.GetMaxThrust();
		return 0;
	}

	public int GetMassScore() {
		// todo
		//GetTotalMass();
		return 0;
	}
	
	public int GetFuelScore() {
		// todo
		//fuel.GetMaxFuel();
		//GetFuelConsumption();
		return 0;
	}
	
	public int GetEnergyScore() {
		// todo
		//energy.GetMaxEnergy();
		//energy.GetMaxGeneration();
		//GetEnergyConsumption();
		energy.Generate();
		return 0;
	}


}
