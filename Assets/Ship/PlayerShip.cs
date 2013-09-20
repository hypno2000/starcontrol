using UnityEngine;
using System;

public class PlayerShip : Ship, LeaveAware {

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
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			propulsionSystem.Thrust();
		}
		else {
			propulsionSystem.StopThrusters();
		}

		// turning left
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			propulsionSystem.ManeuverLeft();
		}
		else {
			propulsionSystem.StopLeftManeuverers();
		}

		// turning right
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			propulsionSystem.ManeuverRight();
		}
		else {
			propulsionSystem.StopRightManeuverers();
		}

		// back
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			propulsionSystem.ManeuverBack();
		}
		else {
			propulsionSystem.StopBackManeuverers();
		}

		// fire weapons
		if (Input.GetKey(KeyCode.Space)) {
			weaponSystem.Fire();
		}

	}

}
	