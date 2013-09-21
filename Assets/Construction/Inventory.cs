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
		int rowCount = 2;
		int colCount = 8;
		slots = new List<InventorySlot>();
		for (int row = 0; row < rowCount; row++) {
			for (int col = 0; col < colCount; col++) {
				InventorySlot slot = Instantiate(slotPrefab) as InventorySlot;
				slot.transform.parent = transform;
				slot.transform.localPosition = new Vector3(col * 1.6f, row * -2.2f, 0f);
				slots.Add(slot);
			}
		}

		width = colCount * 1.6f - 1.6f;
		height = rowCount * 2.2f - 2.2f;

		Reset();
	}

	public void Clear() {
		// clear current stuff
		foreach (var slot in slots) {
			var mod = slot.module;
			if (mod != null) {
				slot.number = 1;
				RemoveModule(mod);
				Destroy(mod.gameObject);
			}
		}
	}

	public void Reset() {
		Clear();

		// no manifest, create test inventory
		if (scene.inventoryManifest == null) {
			HullConstructionModule modulePrefab = game.GetConstructionModulePrefab(ModuleType.Hull) as HullConstructionModule;
			HullConstructionModule module = Instantiate(modulePrefab) as HullConstructionModule;
			module.manifest = new ModuleManifest("Basic hull");
			AddModules(module, 20);

			ThrusterConstructionModule thrusterModulePrefab = game.GetConstructionModulePrefab(ModuleType.Thruster) as ThrusterConstructionModule;
			var thrusterModule = Instantiate(thrusterModulePrefab) as ThrusterConstructionModule;
			thrusterModule.manifest = new ModuleManifest("Basic thruster");
			AddModules (thrusterModule, 15);

			ManeuveringConstructionModule maneuveringModulePrefab = game.GetConstructionModulePrefab(ModuleType.Maneuvering) as ManeuveringConstructionModule;
			var maneuveringModule = Instantiate(maneuveringModulePrefab) as ManeuveringConstructionModule;
			maneuveringModule.manifest = new ModuleManifest("Basic turner");
			AddModules(maneuveringModule, 15);

			WeaponConstructionModule weaponModulePrefab = game.GetConstructionModulePrefab(ModuleType.Weapon) as WeaponConstructionModule;
			var weaponModule = Instantiate(weaponModulePrefab) as WeaponConstructionModule;
			weaponModule.manifest = new ModuleManifest("Missile weapon");
			AddModules (weaponModule, 15);

			BatteryConstructionModule batteryModulePrefab = game.GetConstructionModulePrefab(ModuleType.Battery) as BatteryConstructionModule;
			var batteryModule = Instantiate(batteryModulePrefab) as BatteryConstructionModule;
			batteryModule.manifest = new ModuleManifest("Battery");
			AddModules (batteryModule, 15);

			CrewConstructionModule crewModulePrefab = game.GetConstructionModulePrefab(ModuleType.Crew) as CrewConstructionModule;
			var crewModule = Instantiate(crewModulePrefab) as CrewConstructionModule;
			crewModule.manifest = new ModuleManifest("Crew");
			AddModules (crewModule, 15);

			FuelConstructionModule fuelModulePrefab = game.GetConstructionModulePrefab(ModuleType.Fuel) as FuelConstructionModule;
			FuelConstructionModule fuelModule = Instantiate(fuelModulePrefab) as FuelConstructionModule;
			fuelModule.manifest = new ModuleManifest("Fuel");
			AddModules (fuelModule, 15);

			PowerConstructionModule powerModulePrefab = game.GetConstructionModulePrefab(ModuleType.Power) as PowerConstructionModule;
			PowerConstructionModule powerModule = Instantiate(powerModulePrefab) as PowerConstructionModule;
			powerModule.manifest = new ModuleManifest("Power");
			AddModules (powerModule, 15);
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
		foreach (var slotManifest in manifest.slots) {
			var moduleManifest = slotManifest.module;
			ConstructionModule module = Instantiate(
				game.GetConstructionModulePrefab(moduleManifest.type),
				moduleManifest.position,
				moduleManifest.rotation
				) as ConstructionModule;
			module.manifest = moduleManifest;

			var slot = slots[moduleManifest.inventorySlotIndex];

			slot.number += slotManifest.count - 1;
			AddModule(module, slot);

			Debug.Log("Created " + slotManifest.count + " " + module.GetModuleType() + " modules with name: " + moduleManifest.name);
		}
	}

	public void AddModule(ConstructionModule module) {
		foreach (InventorySlot slot in slots) {
			Debug.Log(module.manifest.name);
			if (slot.available || (module.manifest.name == slot.module.manifest.name)) {
				if(slot.module!=null)
					Debug.Log("inside slot: " + slot.module.manifest.name);
				else
					Debug.Log(slot.available);
				AddModule(module, slot);
				return;
			}
		}
	}

	public void AddModules(ConstructionModule module, int num){
		foreach (InventorySlot slot in slots) {
			if (slot.available || (module.manifest.name == slot.module.manifest.name)) {
				AddModule(module, slot);
				slot.number += num-1;
				return;
			}
		}
	}

	public void AddModule(ConstructionModule module, InventorySlot slot) {
		if(!slot.available && module.manifest.name != slot.module.manifest.name){
			Debug.Log("Addmodule shouldn't happen");	
			return;
		}

		if (module.inventorySlot == slot) {
			module.transform.localPosition = Vector2.zero;
			return;
		}
		if (!slot.available ) {
			if(module.manifest.name == slot.module.manifest.name){
				Destroy(module.gameObject);
				slot.number++;
			}
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
		slot.number++;
	}

	public void RemoveModule(ConstructionModule module) {
		if (module.inventorySlot == null) {
			return;
		}
		InventorySlot slot = module.inventorySlot;

		if(slot.number > 1){
			var prefab = game.GetConstructionModulePrefab(module.GetModuleType());
			var newModuleInstance = Instantiate(prefab) as ConstructionModule;

			newModuleInstance.transform.parent = slot.transform;
			newModuleInstance.transform.localPosition = Vector2.zero;
			newModuleInstance.inventorySlot = slot;
			newModuleInstance.manifest = new ModuleManifest(module);
			newModuleInstance.manifest.name = module.manifest.name;
			slot.module = newModuleInstance;
			module.inventorySlot = null;
		}
		else{
			slot.module = null;
			module.inventorySlot = null;
			modules.Remove(module);
			slot.Activate();
		}
		if(slot.number > 0)
			slot.number--;
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
		foreach (var slot in slots) {
			if (slot.module == null) {
				continue;
			}
			var slotManifest = new InventorySlotManifest();
			ModuleManifest moduleManifest = slot.module.manifest;
			slotManifest.module = moduleManifest;
			slotManifest.count = slot.number;
			moduleManifest.inventorySlotIndex = slots.FindIndex((x) => x == slot);
			moduleManifest.type = (ModuleType)slot.module.GetType().GetField("type").GetRawConstantValue();
			manifest.slots.Add(slotManifest);
		}
		return manifest;
	}

	public void OnLeave() {
		Save();
	}

}
