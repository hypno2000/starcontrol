using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

abstract public class InternalConstructionModule : ConstructionModule {

	[HideInInspector]
	public HullConstructionModule hullModule;

	new void OnMouseDrag() {

		base.OnMouseDrag();

		switch (constructionMode) {
		case ConstructionMode.Move:

			try {

				// snap to hull module
				foreach (KeyValuePair<Vector3, HullConstructionModule> pair in hull.hullModules) {
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
			if (hullModule != null) {
				transform.position = hullModule.transform.position;
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
