using UnityEngine;
using System;

public class HotSeatShip : Ship, LeaveAware {

	public void Save() {
		if (game == null) {
			return;
		}
		game.hullManifest = CreateManifest();
	}

	public void OnLeave() {
		Save();
	}

	override protected void Update() {
		base.Update();

		if (propulsionSystem == null) {
			return;
		}

		// accelerating
		if (Input.GetKey(KeyCode.T)) {
			propulsionSystem.Thrust();
		}
		else {
			propulsionSystem.StopThrusters();
		}

		// turning left
		if (Input.GetKey(KeyCode.F)) {
			propulsionSystem.ManeuverLeft();
		}
		else {
			propulsionSystem.StopLeftManeuverers();
		}

		// turning right
		if (Input.GetKey(KeyCode.H)) {
			propulsionSystem.ManeuverRight();
		}
		else {
			propulsionSystem.StopRightManeuverers();
		}

		// back
		if (Input.GetKey(KeyCode.G)) {
			propulsionSystem.ManeuverBack();
		}
		else {
			propulsionSystem.StopBackManeuverers();
		}

		// fire weapons
		if (Input.GetKey(KeyCode.Tab)) {
			weaponSystem.Fire();
		}

	}

}
	