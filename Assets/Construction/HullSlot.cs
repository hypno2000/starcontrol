using UnityEngine;
using System.Collections;

public class HullSlot : MonoBehaviour {
	
	public bool available;
	public ExternalConstructionModule module;
	public HullSlot prefab;
	private Hull hull;
	
	void Awake() {
		hull = GameObject.Find("Hull").GetComponent<Hull>();
		available = true;
	}
	
	public bool HasAdjacentModules() {
		HullSlot slot;
		
		// check north
		slot = North();
		if (slot != null && slot.module != null) {
			return true;
		}
		
		// check south
		slot = South();
		if (slot != null && slot.module != null) {
			return true;
		}
		
		// check east
		slot = East();
		if (slot != null && slot.module != null) {
			return true;
		}
		
		// check west
		slot = West();
		if (slot != null && slot.module != null) {
			return true;
		}
		
		return false;
	}
	
	public void Activate() {
		if (module != null) {
			return;
		}
		renderer.enabled = true;
		gameObject.layer = 0;
		available = true;
	}
	
	public void Deactivate() {
		renderer.enabled = false;
		gameObject.layer = 2;
		available = false;
	}
	
	public HullSlot Adjacent(Vector3 offset) {
		HullSlot slot;
		Vector3 pos = transform.localPosition - offset;
		if (hull.slots.TryGetValue(pos, out slot)) {
			return slot;
		}
		return slot;
	}
	
	public HullSlot EnsureAdjacent(Vector3 offset) {
		HullSlot slot = Adjacent(offset);
		if (slot == null) {
			Vector3 pos = transform.localPosition - offset;
			slot = Instantiate(prefab) as HullSlot;
			slot.transform.parent = hull.transform;
			slot.transform.localPosition = pos;
			hull.slots.Add(pos, slot);
		}
		return slot;
	}
	
	public void ActivateAdjacents() {
		ActivateNorth();
		ActivateSouth();
		ActivateEast();
		ActivateWest();
	}
	
	public void DeactivateAdjacents() {
		DeactivateNorth();
		DeactivateSouth();
		DeactivateEast();
		DeactivateWest();
	}
	
	public HullSlot ActivateNorth() {
		HullSlot slot = EnsureNorth();
		slot.Activate();
		return slot;
	}
	
	public HullSlot ActivateSouth() {
		HullSlot slot = EnsureSouth();
		slot.Activate();
		return slot;
	}
	
	public HullSlot ActivateEast() {
		HullSlot slot = EnsureEast();
		slot.Activate();
		return slot;
	}
	
	public HullSlot ActivateWest() {
		HullSlot slot = EnsureWest();
		slot.Activate();
		return slot;
	}
	
	public HullSlot DeactivateNorth() {
		HullSlot slot = North();
		if (slot == null || slot.HasAdjacentModules()) {
			return slot;
		}
		slot.Deactivate();
		return slot;
	}
	
	public HullSlot DeactivateSouth() {
		HullSlot slot = South();
		if (slot == null || slot.HasAdjacentModules()) {
			return slot;
		}
		slot.Deactivate();
		return slot;
	}
	
	public HullSlot DeactivateEast() {
		HullSlot slot = East();
		if (slot == null || slot.HasAdjacentModules()) {
			return slot;
		}
		slot.Deactivate();
		return slot;
	}
	
	public HullSlot DeactivateWest() {
		HullSlot slot = West();
		if (slot == null || slot.HasAdjacentModules()) {
			return slot;
		}
		slot.Deactivate();
		return slot;
	}
	
	public HullSlot North() {
		return Adjacent(new Vector3(0f, -1f, 0f));
	}
	
	public HullSlot South() {
		return Adjacent(new Vector3(0f, 1f, 0f));
	}
	
	public HullSlot East() {
		return Adjacent(new Vector3(-1f, 0f, 0f));
	}
	
	public HullSlot West() {
		return Adjacent(new Vector3(1f, 0f, 0f));
	}
	
	public HullSlot EnsureNorth() {
		return EnsureAdjacent(new Vector3(0f, -1f, 0f));
	}
	
	public HullSlot EnsureSouth() {
		return EnsureAdjacent(new Vector3(0f, 1f, 0f));
	}
	
	public HullSlot EnsureEast() {
		return EnsureAdjacent(new Vector3(-1f, 0f, 0f));
	}
	
	public HullSlot EnsureWest() {
		return EnsureAdjacent(new Vector3(1f, 0f, 0f));
	}
	
}
	
