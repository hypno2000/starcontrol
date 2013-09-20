using UnityEngine;
using System.Collections;

abstract public class CombatModule : Module {

	public static CombatModule FindModule(GameObject obj) {
		CombatModule mod;
		while (obj != null) {
			mod = obj.GetComponent<CombatModule>();
			if (mod != null) {
				return mod;
			}
			if (obj.transform.parent == null) {
				return null;
			}
			obj = obj.transform.parent.gameObject;
		}
		return null;
	}

}
