using UnityEngine;
using System;

public class AiShip : Ship {

	public AiObjective objective;

	override protected void Awake() {
		base.Awake();
		objective = gameObject.AddComponent<SeekAndDestroyObjective>();
		objective.ship = this;
		objective.opponentShip = FindObjectOfType(typeof(PlayerShip)) as Ship;
	}

}
