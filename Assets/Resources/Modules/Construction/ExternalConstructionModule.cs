using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

abstract public class ExternalConstructionModule : ConstructionModule {

	[HideInInspector]
	public HullSlot hullSlot;

	override protected void Start() {
		base.Start();
		menu = GetComponent<PieMenu>();
		if (menu != null && hullSlot == null) {
			menu.mainMenu = false;
		}

	}
	
	new void OnMouseDrag() {

		base.OnMouseDrag();

		switch (constructionMode) {
		case ConstructionMode.Move:

			try {

				// snap to hull slot
				foreach (KeyValuePair<Vector3, HullSlot> pair in hull.slots) {
					float dist = Vector3.Distance(transform.position, pair.Value.transform.position);
					if (dist < .3f) {
						inventory.RemoveModule(this);
						hull.Equip(this, pair.Value);
					}
					if (dist >= .3f) {
						hull.Unequip(this, pair.Value);
					}
				}

			} catch (InvalidOperationException) {
				// ignore
			}
			break;
		}

	}

	new void OnMouseUp() {

		switch (constructionMode) {
		case ConstructionMode.Move:
			if (hullSlot != null) {
				transform.position = hullSlot.transform.position;
			} else if (inventorySlot != null) {
				transform.position = inventorySlot.transform.position;
			} else {
				inventory.AddModule(this);
			}
			break;
		}

		base.OnMouseUp();

	}

}
