using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

abstract public class ConstructionModule : Module {

	protected Vector3 mouseStart;
	protected Hull hull;
	protected Inventory inventory;
	[HideInInspector]
	public InventorySlot inventorySlot;
	protected ConstructionMode constructionMode;
	protected bool firstClick = false;
	protected PieMenu menu;

	override protected void Start() {
		base.Start();
		hull = FindObjectOfType(typeof(Hull)) as Hull;
		inventory = FindObjectOfType(typeof(Inventory)) as Inventory;
		constructionMode = ConstructionMode.Idle;
	}

	void OnMouseDown() {
		mouseStart = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
		switch (constructionMode) {
		case ConstructionMode.Idle:
			SendMessage("OnSelect", "Move");
			break;
		}
	}
	
	protected void OnMouseDrag() {

		switch (constructionMode) {
		case ConstructionMode.Move:
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(mousePos.x + mouseStart.x, mousePos.y + mouseStart.y, transform.position.z);

			try {
				
				// snap to inventory slot
				foreach (InventorySlot slot in inventory.slots) {
					float dist = Vector3.Distance(transform.position, slot.transform.position);
					if (dist < .3f) {
						hull.Unequip(this);
						inventory.AddModule(this, slot);
					}
					if (dist >= .3f) {
						inventory.RemoveModule(this, slot);
					}
				}
				
			} catch (InvalidOperationException) {
				// ignore
			}
			break;
		}
		
	}

	protected void OnMouseUp() {
		switch (constructionMode) {
		case ConstructionMode.Move:
			SendMessage("OnSelect", "Idle");
			break;
		}
	}

	override protected void Update() {
		base.Update();
		switch (constructionMode) {
		case ConstructionMode.Rotate:
			Quaternion initial = transform.localRotation;
			transform.LookAt(
				Camera.main.ScreenToWorldPoint(Input.mousePosition),
				new Vector3(0f, 0f, -1f)
			);
			transform.localRotation = new Quaternion(initial.x, initial.y, transform.localRotation.z, transform.localRotation.w);
			if (Input.GetMouseButtonUp(0)) {
				if (firstClick) {
					firstClick = false;
				} else {
					SendMessage("OnSelect", "Idle");
				}
			}
			break;
		}
	}

	void OnSelect(string command) {
		switch (command) {
		case "Rotate":
			constructionMode = ConstructionMode.Rotate;
			firstClick = true;
			break;
		case "Move":
			constructionMode = ConstructionMode.Move;
			break;
		case "Idle":
			constructionMode = ConstructionMode.Idle;
			break;
		}
	}

	public void OnEquip() {
		if (menu != null) {
			menu.mainMenu = true;
		}
	}

	public void OnUnequip() {
		if (menu != null) {
			menu.mainMenu = false;
		}
	}
	
}
