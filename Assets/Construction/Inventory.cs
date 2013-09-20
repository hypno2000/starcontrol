using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : SceneAware<ConstructionScene>, LeaveAware {

	public InventorySlot slotPrefab;
	[HideInInspector]
	public List<ConstructionModule> modules;
	[HideInInspector]
	public List<InventorySlot> slots;
	[HideInInspector]
	public float width;
	[HideInInspector]
	public float height;

	override protected void Start() {
		base.Start();

		// create slots
		int rowCount = 5;
		int colCount = 20;
		slots = new List<InventorySlot>();
		for (int row = 0; row < rowCount; row++) {
			for (int col = 0; col < colCount; col++) {
				InventorySlot slot = Instantiate(slotPrefab) as InventorySlot;
				slot.transform.parent = transform;
				slot.transform.localPosition = new Vector3(col * 1.2f, row * -1.2f, 0f);
				slots.Add(slot);
			}
		}

		width = colCount * 1.2f - 1.2f;
		height = rowCount * 1.2f - 1.2f;

		Reset();
	}

	public void Clear() {
		// clear current stuff
		foreach (var slot in slots) {
			var mod = slot.module;
			if (mod != null) {
				RemoveModule(mod);
				Destroy(mod.gameObject);
			}
		}
	}

	public void Reset() {
		Clear();

		// no manifest, create test inventory
		if (scene.inventoryManifest == null) {
			HullConstructionModule module = game.GetConstructionModulePrefab(ModuleType.Hull) as HullConstructionModule;
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			AddModule(Instantiate(module) as HullConstructionModule);
			
			ThrusterConstructionModule thrusterModule = game.GetConstructionModulePrefab(ModuleType.Thruster) as ThrusterConstructionModule;
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			AddModule(Instantiate(thrusterModule) as ThrusterConstructionModule);
			
			ManeuveringConstructionModule maneuveringModule = game.GetConstructionModulePrefab(ModuleType.Maneuvering) as ManeuveringConstructionModule;
			AddModule(Instantiate(maneuveringModule) as ManeuveringConstructionModule);
			AddModule(Instantiate(maneuveringModule) as ManeuveringConstructionModule);
			AddModule(Instantiate(maneuveringModule) as ManeuveringConstructionModule);
			AddModule(Instantiate(maneuveringModule) as ManeuveringConstructionModule);
			AddModule(Instantiate(maneuveringModule) as ManeuveringConstructionModule);
			AddModule(Instantiate(maneuveringModule) as ManeuveringConstructionModule);
			AddModule(Instantiate(maneuveringModule) as ManeuveringConstructionModule);
			AddModule(Instantiate(maneuveringModule) as ManeuveringConstructionModule);

			WeaponConstructionModule weaponModule = game.GetConstructionModulePrefab(ModuleType.Weapon) as WeaponConstructionModule;
			AddModule(Instantiate(weaponModule) as WeaponConstructionModule);
			AddModule(Instantiate(weaponModule) as WeaponConstructionModule);
			AddModule(Instantiate(weaponModule) as WeaponConstructionModule);
			AddModule(Instantiate(weaponModule) as WeaponConstructionModule);
			AddModule(Instantiate(weaponModule) as WeaponConstructionModule);
			AddModule(Instantiate(weaponModule) as WeaponConstructionModule);
			AddModule(Instantiate(weaponModule) as WeaponConstructionModule);

			BatteryConstructionModule batteryModule = game.GetConstructionModulePrefab(ModuleType.Battery) as BatteryConstructionModule;
			AddModule(Instantiate(batteryModule) as BatteryConstructionModule);
			AddModule(Instantiate(batteryModule) as BatteryConstructionModule);
			AddModule(Instantiate(batteryModule) as BatteryConstructionModule);
			AddModule(Instantiate(batteryModule) as BatteryConstructionModule);
			AddModule(Instantiate(batteryModule) as BatteryConstructionModule);
			AddModule(Instantiate(batteryModule) as BatteryConstructionModule);
			AddModule(Instantiate(batteryModule) as BatteryConstructionModule);
			AddModule(Instantiate(batteryModule) as BatteryConstructionModule);
			AddModule(Instantiate(batteryModule) as BatteryConstructionModule);

			CrewConstructionModule crewModule = game.GetConstructionModulePrefab(ModuleType.Crew) as CrewConstructionModule;
			AddModule(Instantiate(crewModule) as CrewConstructionModule);
			AddModule(Instantiate(crewModule) as CrewConstructionModule);
			AddModule(Instantiate(crewModule) as CrewConstructionModule);
			AddModule(Instantiate(crewModule) as CrewConstructionModule);
			AddModule(Instantiate(crewModule) as CrewConstructionModule);

			FuelConstructionModule fuelModule = game.GetConstructionModulePrefab(ModuleType.Fuel) as FuelConstructionModule;
			AddModule(Instantiate(fuelModule) as FuelConstructionModule);
			AddModule(Instantiate(fuelModule) as FuelConstructionModule);
			AddModule(Instantiate(fuelModule) as FuelConstructionModule);
			AddModule(Instantiate(fuelModule) as FuelConstructionModule);
			AddModule(Instantiate(fuelModule) as FuelConstructionModule);
			AddModule(Instantiate(fuelModule) as FuelConstructionModule);

			PowerConstructionModule powerModule = game.GetConstructionModulePrefab(ModuleType.Power) as PowerConstructionModule;
			AddModule(Instantiate(powerModule) as PowerConstructionModule);
			AddModule(Instantiate(powerModule) as PowerConstructionModule);
			AddModule(Instantiate(powerModule) as PowerConstructionModule);
			AddModule(Instantiate(powerModule) as PowerConstructionModule);
			AddModule(Instantiate(powerModule) as PowerConstructionModule);
			AddModule(Instantiate(powerModule) as PowerConstructionModule);
			AddModule(Instantiate(powerModule) as PowerConstructionModule);
			AddModule(Instantiate(powerModule) as PowerConstructionModule);
		}

		// create inventory based on manifest
		else {
			Construct(scene.inventoryManifest);
		}
	}

	void Update() {
		Vector3 screen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
		transform.position = new Vector3(-width / 2f, screen.y - .7f, 0f);
	}
	
	/**
	 * construct invetory based on manifest
	 */
	public void Construct(InventoryManifest manifest) {
		modules = new List<ConstructionModule>();
		foreach (ModuleManifest moduleManifest in manifest.modules) {
			ConstructionModule module = Instantiate(
				game.GetConstructionModulePrefab(moduleManifest.type),
				moduleManifest.position,
				moduleManifest.rotation
			) as ConstructionModule;
			AddModule(module, slots[moduleManifest.inventorySlotIndex]);
		}
	}
	
	public void AddModule(ConstructionModule module) {
		foreach (InventorySlot slot in slots) {
			if (slot.available) {
				AddModule(module, slot);
				return;
			}
		}
	}
	
	public void AddModule(ConstructionModule module, InventorySlot slot) {
		if (module.inventorySlot == slot) {
			module.transform.localPosition = Vector2.zero;
			return;
		}
		if (!slot.available) {
			return;
		}
		if (module.inventorySlot != null) {
			if (module.inventorySlot == slot) {
				return;
			}
			RemoveModule(module);
		}
		module.transform.parent = slot.transform;
		module.transform.localPosition = Vector2.zero;
		module.inventorySlot = slot;
		slot.module = module;
		modules.Add(module);
		slot.Deactivate();
	}
	
	public void RemoveModule(ConstructionModule module) {
		if (module.inventorySlot == null) {
			return;
		}
		InventorySlot slot = module.inventorySlot;
		slot.module = null;
		module.inventorySlot = null;
		modules.Remove(module);
		slot.Activate();
	}
	
	public void RemoveModule(ConstructionModule module, InventorySlot slot) {
		if (module.inventorySlot != slot) {
			return;
		}
		RemoveModule(module);
	}
	
	public void Save() {
		game.inventoryManifest = scene.inventoryManifest = CreateManifest();
	}
	
	/**
	 * Save state to manifest
	 */
	public InventoryManifest CreateManifest() {
		InventoryManifest manifest = new InventoryManifest(this);
		foreach (ConstructionModule module in modules) {
			ModuleManifest moduleManifest = new ModuleManifest(module);
			moduleManifest.inventorySlotIndex = slots.FindIndex((x) => x == module.inventorySlot);
			moduleManifest.type = (ModuleType)module.GetType().GetField("type").GetRawConstantValue();
			manifest.modules.Add(moduleManifest);
		}
		return manifest;
	}
	
	public void OnLeave() {
		Save();
	}

}
